using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EC2018.Entities;
using EC2018.Enums;
using Newtonsoft.Json;
using EC2018;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EC2018 {

	[RequireComponent(typeof(Instantiator))]
	public class GameManager : MonoBehaviour {
        const string StartReplayMethod = "StartReplay";


        public MusicFadeOut musicFadeOut;
        public int startRound;
		public float roundStepTime = 0.7f;

		Instantiator instantiator;
		UIManager uiManager;
		GameStateManager gameStateManager;
		ReplayManager replayManager;
		bool isPaused;
        bool gameFinished;

		void Start () {
			roundStepTime = CommandLineUtil.GetRoundStep ();

			instantiator = GetComponent<Instantiator> ();
			uiManager = GameObject.FindGameObjectWithTag (Constants.Tags.UIHolder).GetComponent<UIManager> ();
			replayManager = GetComponent<ReplayManager> ();
			gameStateManager = new GameStateManager (startRound, this, uiManager, instantiator, replayManager);

			uiManager.SetPlayerNames (gameStateManager.GetPlayerName(PlayerType.A), gameStateManager.GetPlayerName(PlayerType.B));

			AttemptToStartReplay ();
		}

		public void OnPauseInteraction() {
			if (isPaused) {
				AttemptToStartReplay ();
			} else {
				StopReplay ();
			}
			isPaused = !isPaused;
		}

		public void NavigateToReplayMenu() {
			SceneManager.LoadScene (0, LoadSceneMode.Single);
		}

		public void ReplayFinished() {
            gameFinished = true;
            musicFadeOut.StartFadeOut();
			uiManager.DisplayFinalGameMessage (LoadFinalGameResults());
            HaltAllGameObects();
            gameStateManager.EndGame();
		}

        string LoadFinalGameResults() {
            var streamReader = new StreamReader(gameStateManager.GetFinalRoundPath() + "/" + Constants.Paths.EndGameStateFileName);
            var endGameMessage = streamReader.ReadToEnd();
            streamReader.Close();
            return endGameMessage;
        }

        void HaltAllGameObects() {
            var missiles = GameObject.FindGameObjectsWithTag(Constants.Tags.Missile);
            for (int i = 0; i < missiles.Length; i++) {
                var mc = missiles[i].GetComponent<MissileController>();
                if (mc != null) {
                    mc.Halt();
                }
            }
        }

        void AttemptToStartReplay() {
            if(!gameFinished) {
				InvokeRepeating(StartReplayMethod, 0.5f, roundStepTime);
            }
        }

        void StartReplay() {
            gameStateManager.PlayCurrentState();
        }

        void StopReplay() {
            CancelInvoke(StartReplayMethod);
        }
    }
}
