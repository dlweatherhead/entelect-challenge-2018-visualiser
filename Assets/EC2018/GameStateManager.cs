using System.IO;
using EC2018.Entities;
using UnityEngine;
using Newtonsoft.Json;
using EC2018.Enums;
using System.Collections.Generic;

namespace EC2018 {
	public class GameStateManager {

		private const int RoundNameLength = 3;
		private const char RoundNamePad = '0';

		private Bot bot;
		private GameState gameState;

		public int maxRounds = 1;

		private int startRound;
		private int currentRound;
		private string replayPath;

		// TODO - Separate To Interfaces
		private GameManager gameManager;
		private UIManager uiManager;
		private Instantiator instantiator;
		private ReplayManager replayManager;

		public GameStateManager(int startRound, GameManager gameManager, UIManager uiManager, Instantiator instantiator, ReplayManager replayManager) {
			this.currentRound = startRound;
			this.startRound = startRound;
			this.gameManager = gameManager;
			this.instantiator = instantiator;
			this.uiManager = uiManager;
			this.replayManager = replayManager;

			SetReplayPathFromPrefs();
			SetMaxRounds ();
		}

		public string GetFinalRoundPath() {
			return replayPath + "/" + ConvertRoundToFolderName (currentRound);
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

					if(currentRound == startRound) {
						instantiator.InstantiateGroundTile (inner, outer, cell.CellOwner);
					}
				}
			}
		}

		public void PlayCurrentState() {
			gameState = replayManager.GetGameStateForRound (currentRound);
			uiManager.UpdateUI (gameState.GameDetails, gameState.Players [0], gameState.Players [1]);
			instantiator.ClearScene ();
			PopulateSceneFromGameMap ();
			if(gameState.TeslaHitList.Count > 0) {
				ProcessTeslaHitList (gameState.TeslaHitList[0]);
			}
            ProcessIronCurtainHitList (gameState.Players);
			if (CanIncrementRound()) {
				currentRound++;
			} else if (IsGameFinished()) {
				gameManager.ReplayFinished ();
			}
		}

		public string GetPlayerName(PlayerType playerType) {
			var allPlayers = Directory.GetDirectories (replayPath + "/" + ConvertRoundToFolderName (0));
			if(playerType == PlayerType.A) {
				return new DirectoryInfo(allPlayers [0]).Name;
			} else {
				return new DirectoryInfo(allPlayers [1]).Name;
			}
		}

		private void ProcessTeslaHitList(List<HitList> hitList) {

			var teslaLocation = hitList [hitList.Count - 1];

			for(int i=0; i < hitList.Count - 1; i++) {
				var x = hitList [i].X;
				var y = hitList [i].Y;
				var playerType = hitList [i].PlayerType;
				instantiator.InstantiateTeslaHit (x, y, teslaLocation.PlayerType, teslaLocation.X, teslaLocation.Y);
			}
		}

        private void ProcessIronCurtainHitList(List<Player> players) {
            for (int i = 0; i < players.Count; i++) {
                var available = players[i].IronCurtainAvailable;
                var lifetime = players[i].ActiveIronCurtainLifetime;
                if(available && lifetime >= -6) {
                    instantiator.ActivateIronCurtain(players[i].PlayerType);
                } else {
                    instantiator.DeactivateIronCurtain(players[i].PlayerType);
                }
            }
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
