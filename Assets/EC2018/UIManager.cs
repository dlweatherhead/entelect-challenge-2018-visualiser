﻿using UnityEngine;
using UnityEngine.UI;
using EC2018.Entities;
using EC2018.Enums;

namespace EC2018
{
	public class UIManager : MonoBehaviour {
		public Text PlayerAName;
		public Text PlayerBName;
		public Text HealthA;
		public Text HealthB;
		public Text EnergyA;
		public Text EnergyB;
		public Text ScoreA;
		public Text ScoreB;
		public Text RoundText;

		public GameObject FinalGameHolder;
		public Text FinalGameText;

        public BarrierHealth barrierHealthA;
        public BarrierHealth barrierHealthB;

        GameManager gameManager;

        void Awake() {
			gameManager = GameObject.FindGameObjectWithTag (Constants.Tags.GameManager).GetComponent<GameManager> ();
		}

		void Update() {
			if(Input.GetKey (KeyCode.Space)) {
				gameManager.OnPauseInteraction ();
			}
			if(Input.GetKey (KeyCode.R)) {
				gameManager.NavigateToReplayMenu ();
			}
			if(Input.GetKey (KeyCode.Q)) {
				Application.Quit ();
			}
		}

		public void SetPlayerNames(string playerAName, string playerBName) {
			PlayerAName.text = playerAName;
			PlayerBName.text = playerBName;
		}

		public void UpdateUI(GameDetails gameDetails, Player playerA, Player playerB) {
            barrierHealthA.SetHealth(playerA.Health);
            barrierHealthB.SetHealth(playerB.Health);

			HealthA.text = GetHealth(playerA);
			HealthB.text = GetHealth(playerB);

			EnergyA.text = GetEnergy (playerA);
			EnergyB.text = GetEnergy (playerB);

			ScoreA.text = GetScore (playerA);
			ScoreB.text = GetScore (playerB);

			RoundText.text = gameDetails.Round.ToString();
		}

		public void DisplayFinalGameMessage(string message) {
			FinalGameHolder.SetActive (true);
			FinalGameText.text = message;
		}

        string GetPlayerType(Player player) {
            return player.PlayerType.ToString();
        }

        string GetHealth(Player player) {
            return player.Health.ToString();
        }

        string GetEnergy(Player player) {
            return player.Energy.ToString();
        }

        string GetScore(Player player) {
            return player.Score.ToString();
        }

        string GetPrice(GameDetails gameDetails, BuildingType buildingType) {
            return gameDetails.BuildingPrices[buildingType].ToString();
        }
    }
}
