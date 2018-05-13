using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EC2018.Entities;

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
}
