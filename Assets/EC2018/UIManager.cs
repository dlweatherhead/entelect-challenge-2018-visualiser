using UnityEngine;
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
		public Text HitsA;
		public Text HitsB;
		public Text RoundText;
		public Text AttackPriceText;
		public Text DefensePriceText;
		public Text EnergyPriceText;

		public GameObject FinalGameHolder;
		public Text FinalGameText;

        GameManager gameManager;

        void Awake() {
			gameManager = GameObject.FindGameObjectWithTag (Constants.Tags.GameManager).GetComponent<GameManager> ();
		}

		public void OnPausePlayClick(Button button) {
			var text = button.GetComponentInChildren<Text> ();
			if (text.text == Constants.UI.Pause) {
				text.text = Constants.UI.Play;
			} else {
				text.text = Constants.UI.Pause;
			}

			gameManager.OnPauseInteraction ();
		}

		public void OnReplayMenuButtonClick() {
			gameManager.NavigateToReplayMenu ();
		}

		public void SetPlayerNames(string playerAName, string playerBName) {
			PlayerAName.text = playerAName;
			PlayerBName.text = playerBName;
		}

		public void UpdateUI(GameDetails gameDetails, Player playerA, Player playerB) {
			HealthA.text = GetHealth(playerA);
			HealthB.text = GetHealth(playerB);

			EnergyA.text = GetEnergy (playerA);
			EnergyB.text = GetEnergy (playerB);

			ScoreA.text = GetScore (playerA);
			ScoreB.text = GetScore (playerB);

			HitsA.text = GetHits(playerA);
			HitsB.text = GetHits(playerB);

			AttackPriceText.text = GetPrice (gameDetails, BuildingType.Attack);
			DefensePriceText.text = GetPrice (gameDetails, BuildingType.Defense);
			EnergyPriceText.text = GetPrice (gameDetails, BuildingType.Energy);

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

        string GetHits(Player player) {
            return player.HitsTaken.ToString();
        }

        string GetPrice(GameDetails gameDetails, BuildingType buildingType) {
            return gameDetails.BuildingPrices[buildingType].ToString();
        }
    }
}
