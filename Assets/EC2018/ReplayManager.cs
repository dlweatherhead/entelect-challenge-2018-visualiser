using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018;
using System.IO;
using Newtonsoft.Json;

public class ReplayManager : MonoBehaviour {

    const int RoundNameLength = 3;
    const char RoundNamePad = '0';

    List<Round> rounds;

    void Start() {
        rounds = new List<Round>();
        LoadAllRounds();
    }

    public GameState GetGameStateForRound(int round) {
        return rounds[round].statePlayerA;
    }

    void LoadAllRounds() {
        var replayPath = GetReplayPathFromPrefs();
        var numberOfRounds = GetMaxRounds(replayPath);

        for (int roundNumber = 0; roundNumber < numberOfRounds; roundNumber++) {
            var roundName = ConvertRoundToFolderName(roundNumber);
            var statePlayerA = GetGameStateForPlayer(0, replayPath, roundName);
            var round = new Round(statePlayerA, null);
            rounds.Add(round);
        }
    }

    // 0 for Player A, 1 for Player B
    GameState GetGameStateForPlayer(int player, string replayPath, string roundName) {
        var allPlayersDir = Directory.GetDirectories(replayPath + "/" + roundName);
        var firstState = GetFileContents(allPlayersDir[player] + Constants.Paths.MapName);
        return JsonConvert.DeserializeObject<GameState>(firstState);
    }

    string GetFileContents(string filePath) {
        var streamReader = new StreamReader(filePath);
        var contents = streamReader.ReadToEnd();
        streamReader.Close();

        return contents;
    }

    string GetReplayPathFromPrefs() {
        return PlayerPrefs.GetString(Constants.PlayerPrefKeys.SelectedReplay);
    }

    string ConvertRoundToFolderName(int round) {
        return Constants.Paths.RoundFolderNamePrefix + round.ToString().PadLeft(RoundNameLength, RoundNamePad);
    }

    int GetMaxRounds(string replayPath) {
        return Directory.GetDirectories(replayPath).Length;
    }
}
