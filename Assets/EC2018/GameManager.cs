using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EC2018.Entities;
using EC2018.Enums;
using Newtonsoft.Json;
using EC2018;
using System;

namespace EC2018 {

	[RequireComponent(typeof(Instantiator))]
	public class GameManager : MonoBehaviour {
		public const int StartRound = 0;

		private const int RoundNameLength = 3;
		private const char RoundNamePad = '0';
		private const string MapName = "/JsonMap.json";
		private const string RoundFolderNamePrefix = "Round ";

		public int maxRounds = 1;

		private Bot bot;
		private GameState gameState;
		private int currentRound;
		private Instantiator instantiator;
		private UIManager uiManager;
		private string replayPath;
		private bool isPaused;

		void Start () {
			currentRound = StartRound;
			instantiator = GetComponent<Instantiator> ();
			uiManager = GameObject.FindGameObjectWithTag ("UI Holder").GetComponent<UIManager> ();
			replayPath = GetSelectedReplayPath();
			maxRounds = GetNumberOfRounds ();

			InvokeRepeating ("PopulateCurrentScene", 0, 0.5f);
		}

		public void OnPauseInteraction() {
			if (isPaused) {
				Debug.Log ("Starting...");
				InvokeRepeating ("PopulateCurrentScene", 0.5f, 0.5f);
				isPaused = false;
			} else {
				Debug.Log ("Pausing...");
				CancelInvoke ("PopulateCurrentScene");
				isPaused = true;
			}
		}

		private string GetSelectedReplayPath() {
			return PlayerPrefs.GetString ("SelectedReplay");
		}

		private void PopulateCurrentScene() {

			Debug.Log (currentRound);

			instantiator.ClearScene ();

			string roundName = ConvertRoundToFolderName (currentRound);
			LoadJsonMapForPlayerA ("/" + roundName);

			uiManager.UpdateUI (gameState.GameDetails, gameState.Players [0], gameState.Players [1]);

			PopulateSceneFromGameMap ();

			if (currentRound >= maxRounds) {
				CancelInvoke ("PopulateCurrentScene");
				Debug.Log("Replay Finished!");
			} else {
				if (!isPaused) {
					currentRound++;
				}
			}
		}

		private void PopulateSceneFromGameMap() {
			var map = gameState.GameMap;

			for (int outer = 0; outer < map.Length; outer++) {
				for (int inner = 0; inner < map[outer].Length; inner++) {
					var cell = map [outer] [inner];
					var x = cell.X;
					var y = cell.Y;

					instantiator.InstantiateBuildingsAtLocation (cell.Buildings, x, y);
					instantiator.InstantiateMissileAtLocation (cell.Missiles, x, y, 0.5f);

					// Map is orientated vertically, with inner objects having x, y related to that
					//	orientation. We want to orientate horizontally for easier use in Editor.
					//	inner = x
					//	outer = y
					instantiator.InstantiateGroundTile (inner, outer);
				}
			}
		}

		private int GetNumberOfRounds() {
			return Directory.GetDirectories (replayPath).Length - 1; // Index offset
		}

		// Loading the First Replay Folder we find
		// TODO - Create Folder selector or load newest
		private void LoadJsonMapForPlayerA(string roundName) {
			string[] allPlayersDir = Directory.GetDirectories (replayPath + roundName);
			string firstState = GetFileContents (allPlayersDir [0] + MapName);
			gameState = JsonConvert.DeserializeObject<GameState>(firstState);
		}

		private string GetFileContents(string path) {
			string filePath = path;
			var streamReader = new StreamReader (filePath);
			var contents = streamReader.ReadToEnd ();
			streamReader.Close ();

			return contents;
		}

		private string ConvertRoundToFolderName(int round) {
			return RoundFolderNamePrefix + round.ToString ().PadLeft (RoundNameLength, RoundNamePad);
		}
	}
}
