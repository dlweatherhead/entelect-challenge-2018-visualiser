using System.IO;
using EC2018.Entities;
using UnityEngine;
using Newtonsoft.Json;

namespace EC2018 {
	public class GameStateManager {

		private const int RoundNameLength = 3;
		private const char RoundNamePad = '0';

		private Bot bot;
		private GameState gameState;

		public int maxRounds = 1;

		private int currentRound;
		private string replayPath;

		// TODO - Separate To Interfaces
		private GameManager gameManager;
		private UIManager uiManager;
		private Instantiator instantiator;

		public GameStateManager(GameManager gameManager, UIManager uiManager, Instantiator instantiator) {
			this.gameManager = gameManager;
			this.instantiator = instantiator;
			this.uiManager = uiManager;

			SetReplayPathFromPrefs();
			SetMaxRounds ();
		}

		private void SetMaxRounds() {
			maxRounds = Directory.GetDirectories (replayPath).Length - 1; // Index offset
		}

		public bool CanIncrementRound() {
			return currentRound < maxRounds;
		}

		public bool IsGameFinished() {
			return currentRound >= maxRounds;
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

		public void PlayCurrentState() {
			LoadJsonMapForPlayerA ("/" + ConvertRoundToFolderName (currentRound));
			uiManager.UpdateUI (gameState.GameDetails, gameState.Players [0], gameState.Players [1]);
			instantiator.ClearScene ();
			PopulateSceneFromGameMap ();
			if (CanIncrementRound()) {
				currentRound++;
			} else if (IsGameFinished()) {
				gameManager.ReplayFinished ();
			}

		}

		// Loading the First Replay Folder we find
		// TODO - Create Folder selector or load newest
		private void LoadJsonMapForPlayerA(string roundName) {
			string[] allPlayersDir = Directory.GetDirectories (replayPath + roundName);
			string firstState = GetFileContents (allPlayersDir [0] + Constants.Paths.MapName);
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
			return Constants.Paths.RoundFolderNamePrefix + round.ToString ().PadLeft (RoundNameLength, RoundNamePad);
		}

		private void SetReplayPathFromPrefs() {
			replayPath = PlayerPrefs.GetString (Constants.PlayerPrefKeys.SelectedReplay);
		}
	}

}
