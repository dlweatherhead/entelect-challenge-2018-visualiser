using UnityEngine;
using System.IO;
using EC2018.Enums;
using EC2018;
using TMPro;

public class CountInDetailPopulator : MonoBehaviour {

    public TextMeshProUGUI tmpPlayerA;
    public TextMeshProUGUI tmpPlayerB;

	void Start () {
        tmpPlayerA.text = GetPlayerName (PlayerType.A);
        tmpPlayerB.text = GetPlayerName (PlayerType.B);
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
