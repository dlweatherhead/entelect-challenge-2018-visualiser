using UnityEngine;
using System.IO;
using EC2018.Enums;
using EC2018;
using TMPro;

public class CountInDetailPopulator : MonoBehaviour {

    public TextMeshProUGUI tmpPlayerA;
    public TextMeshProUGUI tmpPlayerB;
    public int MaxLength = 31;

	void Start () {
        tmpPlayerA.text = GetPlayerName (PlayerType.A);
        tmpPlayerB.text = GetPlayerName (PlayerType.B);
	}

	public string GetPlayerName(PlayerType playerType) {
		var allPlayers = Directory.GetDirectories(GetReplayPathFromPrefs() + "/Round 000");
        string playerName;
		if (playerType == PlayerType.A) {
			playerName = new DirectoryInfo(allPlayers[0]).Name;
        } else {
            playerName = new DirectoryInfo(allPlayers[1]).Name;   
        }

        if(playerName.Length > MaxLength) {
            playerName = playerName.Substring(0, MaxLength) + "...";
        }

        return playerName;
	}

	string GetReplayPathFromPrefs() {
		return PlayerPrefs.GetString(Constants.PlayerPrefKeys.SelectedReplay);
	}
}
