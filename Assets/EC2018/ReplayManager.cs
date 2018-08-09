using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018;
using System.IO;
using Newtonsoft.Json;

public class ReplayManager : MonoBehaviour {

	private const int RoundNameLength = 3;
	private const char RoundNamePad = '0';

	List<Round> rounds;

	void Start() {
		rounds = new List<Round> ();
		LoadAllRounds ();
	}

	public GameState GetGameStateForRound(int round) {
		return rounds [round].statePlayerA;
	}

	private void LoadAllRounds() {
		string replayPath = GetReplayPathFromPrefs ();
		int numberOfRounds = GetMaxRounds (replayPath);

		for(int roundNumber=0; roundNumber<numberOfRounds; roundNumber++) {
			string roundName = ConvertRoundToFolderName (roundNumber);
			GameState statePlayerA = GetGameStateForPlayer (0, replayPath, roundName);
			GameState statePlayerB = GetGameStateForPlayer (1, replayPath, roundName);
			Round round = new Round (statePlayerA, statePlayerB);
			rounds.Add (round);
		}
	}
		
	// 0 for Player A, 1 for Player B
	private GameState GetGameStateForPlayer(int player, string replayPath, string roundName) {
		string[] allPlayersDir = Directory.GetDirectories (replayPath + "/" + roundName);
		string firstState = GetFileContents (allPlayersDir [0] + Constants.Paths.MapName);
		return JsonConvert.DeserializeObject<GameState>(firstState);
	}

	private string GetFileContents(string path) {
		string filePath = path;
		var streamReader = new StreamReader (filePath);
		var contents = streamReader.ReadToEnd ();
		streamReader.Close ();

		return contents;
	}

	private string GetReplayPathFromPrefs() {
		return PlayerPrefs.GetString (Constants.PlayerPrefKeys.SelectedReplay);
	}

	private string ConvertRoundToFolderName(int round) {
		return Constants.Paths.RoundFolderNamePrefix + round.ToString ().PadLeft (RoundNameLength, RoundNamePad);
	}

	private int GetMaxRounds(string replayPath) {
		return Directory.GetDirectories (replayPath).Length;
	}
}
