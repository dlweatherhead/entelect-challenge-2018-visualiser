using System.IO;
using EC2018.Entities;
using UnityEngine;
using EC2018.Enums;
using System.Collections.Generic;

namespace EC2018 {
    public class GameStateManager {

        const int RoundNameLength = 3;
        const char RoundNamePad = '0';

        GameState gameState;

        public int maxRounds = 1;

        int startRound;
        int currentRound;
        string replayPath;

        GameManager gameManager;
        UIManager uiManager;
        Instantiator instantiator;
        ReplayManager replayManager;

		float rate;

        public GameStateManager(int startRound, GameManager gameManager, UIManager uiManager, Instantiator instantiator, ReplayManager replayManager) {
            this.startRound = startRound;
            this.gameManager = gameManager;
            this.instantiator = instantiator;
            this.uiManager = uiManager;
            this.replayManager = replayManager;

			currentRound = startRound;
			rate = CommandLineUtil.GetRoundStep ();

            Initialise();
        }

        void Initialise() {
            SetReplayPathFromPrefs();
            SetMaxRounds();

        }

        public string GetFinalRoundPath() {
            return replayPath + "/" + ConvertRoundToFolderName(currentRound);
        }

        void SetMaxRounds() {
            maxRounds = Directory.GetDirectories(replayPath).Length - 1; // Index offset
        }

        public bool CanIncrementRound() {
            return currentRound < maxRounds;
        }

        public void EndGame() {
            if(gameState.Players[0].Health < gameState.Players[1].Health) {
                instantiator.InstantiateEndGameAnimations(PlayerType.A);
            } else if(gameState.Players[0].Health > gameState.Players[1].Health) {
                instantiator.InstantiateEndGameAnimations(PlayerType.B);
            }
        }

        void PopulateSceneFromGameMap() {
            var map = gameState.GameMap;

            for (int outer = 0; outer < map.Length; outer++) {
                for (int inner = 0; inner < map[outer].Length; inner++) {
                    var cell = map[outer][inner];
                    var x = cell.X;
                    var y = cell.Y;

                    instantiator.InstantiateBuildingsAtLocation(cell.Buildings, x, y);
                    instantiator.InstantiateMissileAtLocation(cell.Missiles, x, y, rate);

                    if (currentRound == startRound) {
                        instantiator.InstantiateGroundTile(inner, outer, cell.CellOwner);
                    }
                }
            }
        }

        public void PlayCurrentState() {
            gameState = replayManager.GetGameStateForRound(currentRound);
            uiManager.UpdateUI(gameState.GameDetails, gameState.Players[0], gameState.Players[1]);
            instantiator.ClearScene();
            PopulateSceneFromGameMap();
            if (gameState.TeslaHitList.Count > 0) {
                ProcessTeslaHitList(gameState.TeslaHitList);
            }
            ProcessIronCurtainHitList(gameState.Players);
            if (CanIncrementRound()) {
                currentRound++;
            } else {
                gameManager.ReplayFinished();
            }
        }

        public string GetPlayerName(PlayerType playerType) {
            var allPlayers = Directory.GetDirectories(replayPath + "/" + ConvertRoundToFolderName(0));
            if (playerType == PlayerType.A) {
                return new DirectoryInfo(allPlayers[0]).Name;
            }
            return new DirectoryInfo(allPlayers[1]).Name;
        }

        void ProcessTeslaHitList(List<List<HitList>> hitLists) {
            for (int i = 0; i < hitLists.Count; i++) {
				instantiator.InstantiateTeslaHit(hitLists[i]);
            }
        }

        void ProcessIronCurtainHitList(List<Player> players) {
            for (int i = 0; i < players.Count; i++) {
                var available = players[i].IronCurtainAvailable;
                var lifetime = players[i].ActiveIronCurtainLifetime;
                if (available && lifetime >= -6) {
                    instantiator.ActivateIronCurtain(players[i].PlayerType);
                } else {
                    instantiator.DeactivateIronCurtain(players[i].PlayerType);
                }
            }
        }

        string GetFileContents(string path) {
            string filePath = path;
            var streamReader = new StreamReader(filePath);
            var contents = streamReader.ReadToEnd();
            streamReader.Close();

            return contents;
        }

        string ConvertRoundToFolderName(int round) {
            return Constants.Paths.RoundFolderNamePrefix + round.ToString().PadLeft(RoundNameLength, RoundNamePad);
        }

        void SetReplayPathFromPrefs() {
            replayPath = PlayerPrefs.GetString(Constants.PlayerPrefKeys.SelectedReplay);
        }
    }

}
