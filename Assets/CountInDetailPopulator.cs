using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EC2018.Enums;
using UnityEngine.UI;
using EC2018;

public class CountInDetailPopulator : MonoBehaviour {

	public Text namePlayerA;
	public Text namePlayerB;

	void Start () {
		namePlayerA.text = GetPlayerName (PlayerType.A);
		namePlayerB.text = GetPlayerName (PlayerType.B);
	}

	public string GetPlayerName(PlayerType playerType) {
		var allPlayers = Directory.GetDirectories(GetReplayPathFromPrefs() + "/Round 000");
		if (playerType == PlayerType.A) {
			return new DirectoryInfo(allPlayers[0]).Name;
		}
		return new DirectoryInfo(allPlayers[1]).Name;
	}

	string GetReplayPathFromPrefs() {
		return PlayerPrefs.GetString(Constants.PlayerPrefKeys.SelectedReplay);
	}
}
