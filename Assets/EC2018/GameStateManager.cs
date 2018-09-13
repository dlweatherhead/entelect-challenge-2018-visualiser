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

        List<string> previousMissiles;

		float rate;

        public GameStateManager(int startRound, GameManager gameManager, UIManager uiManager, Instantiator instantiator, ReplayManager replayManager) {
            this.startRound = startRound;
            this.gameManager = gameManager;
            this.instantiator = instantiator;
            this.uiManager = uiManager;
            this.replayManager = replayManager;

			currentRound = startRound;
			rate = CommandLineUtil.GetRoundStep ();

            previousMissiles = new List<string>();

            Initialise();
        }

        void Initialise() {
            SetReplayPathFromPrefs();
            SetMaxRounds();
            uiManager.SetPlayerNames(GetPlayerName(PlayerType.A), GetPlayerName(PlayerType.B));
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

            uiManager.DisplayFinalGameMessage(gameState, GetPlayerName(PlayerType.A), GetPlayerName(PlayerType.B));

            if(gameState.Players[0].Health < gameState.Players[1].Health) {
                instantiator.InstantiateEndGameAnimations(PlayerType.A);
            } else if(gameState.Players[0].Health > gameState.Players[1].Health) {
                instantiator.InstantiateEndGameAnimations(PlayerType.B);
            }
        }

        string LoadFinalGameResults() {
            var streamReader = new StreamReader(GetFinalRoundPath() + "/" + Constants.Paths.EndGameStateFileName);
            var endGameMessage = streamReader.ReadToEnd();
            streamReader.Close();
            return endGameMessage;
        }

        void PopulateSceneFromGameMap() {
            var map = gameState.GameMap;

            for (int outer = 0; outer < map.Length; outer++) {
                for (int inner = 0; inner < map[outer].Length; inner++) {
                    var cell = map[outer][inner];
                    var x = cell.X;
                    var y = cell.Y;

                    instantiator.InstantiateBuildingsAtLocation(cell.Buildings, x, y);

                    //create list of player A missiles
                    var missilesPlayerA = new List<Missile>();
                    var missilesPlayerB = new List<Missile>();

                    //create list of player B missiles
                    if(cell.Missiles.Count > 0) {
                        for (int m = 0; m < cell.Missiles.Count; m++) {
                            if(cell.Missiles[m].PlayerType == PlayerType.A) {
                                missilesPlayerA.Add(cell.Missiles[m]);
                            } else {
                                missilesPlayerB.Add(cell.Missiles[m]);
                            }
                        }
                    }

                    instantiator.InstantiateMissileAtLocation(missilesPlayerA, x, y, rate);
                    instantiator.InstantiateMissileAtLocation(missilesPlayerB, x, y, rate);

                    if (currentRound == startRound) {
                        instantiator.InstantiateGroundTile(inner, outer, cell.CellOwner);
                    }
                }
            }
        }

        void InstantiateNextRoundNewMissiles() {
            if (CanIncrementRound()) {
                // Get the next state
                var nextRound = currentRound + 1;
                var nextGameState = replayManager.GetGameStateForRound(nextRound);
                var nextGameMap = nextGameState.GameMap;

                // Loop through gamemap, inspecting all missiles
                for (int outer = 0; outer < nextGameMap.Length; outer++) {
                    for (int inner = 0; inner < nextGameMap[outer].Length; inner++) {
                        var cell = nextGameMap[outer][inner];
                        var x = cell.X;
                        var y = cell.Y;
                        var missiles = cell.Missiles;

                        if (missiles.Count > 0) {
                            for (int m = 0; m < missiles.Count; m++) {
                                var missile = missiles[m];
                                if (!previousMissiles.Contains(missile.Id)) {

                                    // Set coordinates to match destination, shifted by speed for player
                                    var m_x = missile.PlayerType == PlayerType.A ? x - missile.Speed : x + missile.Speed;

                                    // Instantiate missile at location
                                    var singleMissileList = new List<Missile> { missile };
                                    instantiator.InstantiateMissileAtLocation(singleMissileList, m_x, y, rate);

                                    //  add new id to previous id list
                                    previousMissiles.Add(missile.Id);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PlayCurrentState() {
            gameState = replayManager.GetGameStateForRound(currentRound);
            uiManager.UpdateUI(gameState.GameDetails, gameState.Players[0], gameState.Players[1]);
            instantiator.ClearScene();

            //InstantiateNextRoundNewMissiles();

            PopulateSceneFromGameMap();

            // Process Tesla hits one round ahead
            if (CanIncrementRound()) {
                var nextRound = currentRound + 1;
                var nextGameState = replayManager.GetGameStateForRound(nextRound);
                if (nextGameState.TeslaHitList.Count > 0) {
                    ProcessTeslaHitList(nextGameState.TeslaHitList);
                }
            }

            if(currentRound > 0) {
                ProcessIronCurtainHitList(gameState.Players);    
            }

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
                instantiator.InstantiateTeslaHit(hitLists[i], gameState.Players[0], gameState.Players[1]);
            }
        }

        void ProcessIronCurtainHitList(List<Player> players) {
            for (int i = 0; i < players.Count; i++) {
                if (players[i].IsIronCurtainActive) {

                    var hitList = gameState.IroncurtainHitList;

                    for (int h = 0; h < hitList.Count; h++) {
                        instantiator.InstantiateExplosion(7.5f, hitList[h].Y, hitList[h].PlayerType);
                    }
                        
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
