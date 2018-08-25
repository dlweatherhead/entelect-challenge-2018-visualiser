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

		GameStateManager gameStateManager;
		bool isPaused;
        bool gameFinished;

		void Start () {
			roundStepTime = CommandLineUtil.GetRoundStep ();

            Instantiator instantiator = GetComponent<Instantiator> ();
            UIManager uiManager = GameObject.FindGameObjectWithTag (Constants.Tags.UIHolder).GetComponent<UIManager> ();
            ReplayManager replayManager = GetComponent<ReplayManager> ();
            gameStateManager = new GameStateManager(startRound, this, uiManager, instantiator, replayManager);

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
            HaltAllGameObects();
			OnPauseInteraction ();
            gameStateManager.EndGame();
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
