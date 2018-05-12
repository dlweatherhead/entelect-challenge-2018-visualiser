using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EC2018.Entities;
using Newtonsoft.Json;
using EC2018;

public class GameManager : MonoBehaviour {

	private string exampleBot = "example-bot";
	private string exampleState = "example-state";

	void Start () {
		LoadBotFromFile();
		LoadStateFromFile();
	}

	void Update () {
		
	}

	private void LoadBotFromFile() {
		TextAsset botFile = Resources.Load<TextAsset> (exampleBot);
		var bot = JsonConvert.DeserializeObject<Bot> (botFile.text);

		Debug.Log (bot.nickName);
	}

	private void LoadStateFromFile() {
		TextAsset stateFile = Resources.Load<TextAsset> (exampleState);

		var gameState = JsonConvert.DeserializeObject<GameState>(stateFile.text);

		Debug.Log (gameState.Players[0].PlayerType);
	}
}
