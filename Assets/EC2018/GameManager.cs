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

namespace EC2018 {

	[RequireComponent(typeof(Instantiator))]
	public class GameManager : MonoBehaviour {
		private const string StartReplayMethod = "StartReplay";

		
		private Instantiator instantiator;
		private UIManager uiManager;
		private GameStateManager gameStateManager;
		private ReplayManager replayManager;

		private bool isPaused;

		void Start () {
			instantiator = GetComponent<Instantiator> ();
			uiManager = GameObject.FindGameObjectWithTag (Constants.Tags.UIHolder).GetComponent<UIManager> ();
			replayManager = GetComponent<ReplayManager> ();
			gameStateManager = new GameStateManager (this, uiManager, instantiator, replayManager);

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
			uiManager.DisplayFinalGameMessage (LoadFinalGameResults());

			HaltAllGameObects ();
		}

		private string LoadFinalGameResults() {
			var streamReader = new StreamReader (gameStateManager.GetFinalRoundPath () + "/" + Constants.Paths.EndGameStateFileName);
			var endGameMessage = streamReader.ReadToEnd ();
			streamReader.Close ();

			return endGameMessage;
		}

		private void HaltAllGameObects() {
			var missiles = GameObject.FindGameObjectsWithTag (Constants.Tags.Missile);
			for (int i = 0; i < missiles.Length; i++) {
				MissileController mc = missiles [i].GetComponent<MissileController> ();
				if(mc != null) {
					mc.Halt ();
				}
			}
		}

		// TODO - Refactor to Run with Update instead, for better separation between Game and State Managers
		private void AttemptToStartReplay() {
			if(gameStateManager.CanIncrementRound()) {
				InvokeRepeating (StartReplayMethod, 0.5f, 0.5f);
			} else if(gameStateManager.IsGameFinished()) {
				StopReplay ();
				ReplayFinished ();
			}
		}

		private void StartReplay() {
			gameStateManager.PlayCurrentState ();
		}

		private void StopReplay() {
			CancelInvoke (StartReplayMethod);
		}
	}
}
