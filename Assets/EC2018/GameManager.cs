using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EC2018.Entities;
using EC2018.Enums;
using Newtonsoft.Json;
using EC2018;

[RequireComponent(typeof(Instantiator))]
public class GameManager : MonoBehaviour {
	private string exampleBot = "example-bot";
	private string exampleState = "example-state";

	private Bot bot;
	private GameState gameState;

	private Instantiator instantiator;

	void Awake() {
		instantiator = GetComponent<Instantiator> ();
	}

	void Start () {
		LoadBotFromFile();
		LoadStateFromFile();
		PopulateSceneFromGameMap ();
	}

	void Update () {
		
	}

	private void PopulateSceneFromGameMap() {
		var map = gameState.GameMap;

		for (int outer = 0; outer < map.Length; outer++) {
			for (int inner = 0; inner < map[outer].Length; inner++) {
				var cell = map [outer] [inner];
				var x = cell.X;
				var y = cell.Y;

				instantiator.InstantiateBuildingsAtLocation (cell.Buildings, x, y);
				instantiator.InstantiateMissileAtLocation (cell.Missiles, x, y);

				// Map is orientated vertically, with inner objects having x, y related to that
				//	orientation. We want to orientate horizontally for easier use in Editor.
				//	inner = x
				//	outer = y
				instantiator.InstantiateGroundTile (inner, outer);
			}
		}

	}

	private void LoadBotFromFile() {
		TextAsset botFile = Resources.Load<TextAsset> (exampleBot);
		bot = JsonConvert.DeserializeObject<Bot> (botFile.text);
	}

	private void LoadStateFromFile() {
		TextAsset stateFile = Resources.Load<TextAsset> (exampleState);
		gameState = JsonConvert.DeserializeObject<GameState>(stateFile.text);
	}
}
