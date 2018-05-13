using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EC2018.Entities;
using EC2018.Enums;

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

	private GameManager gameManager;

	void Awake() {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}

	public void OnPausePlayClick(Button button) {
		Text text = button.GetComponentInChildren<Text> ();
		if (text.text == "Pause") {
			text.text = "Play";
		} else {
			text.text = "Pause";
		}

		gameManager.OnPauseInteraction ();
	}

	public void UpdateUI(GameDetails gameDetails, Player playerA, Player playerB) {
		PlayerAName.text = playerA.PlayerType.ToString ();
		PlayerBName.text = playerB.PlayerType.ToString ();

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

	private string GetPlayerType(Player player) {
		return player.PlayerType.ToString ();
	}

	private string GetHealth(Player player) {
		return player.Health.ToString ();
	}

	private string GetEnergy(Player player) {
		return player.Energy.ToString ();
	}

	private string GetScore(Player player) {
		return player.Score.ToString ();
	}

	private string GetHits(Player player) {
		return player.HitsTaken.ToString ();
	}

	private string GetPrice(GameDetails gameDetails, BuildingType buildingType) {
		return gameDetails.BuildingPrices[buildingType].ToString();
	}
}
