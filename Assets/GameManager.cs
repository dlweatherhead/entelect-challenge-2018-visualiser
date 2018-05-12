using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EC2018.Entities;
using EC2018.Enums;
using Newtonsoft.Json;
using EC2018;

public class GameManager : MonoBehaviour {
	const string BUILDINGS_PARENT = "Buildings";
	const string GROUNDTILES_PARENT = "GroundTiles";
	const string MISSILES_PARENT = "Missiles";

	public GameObject attackPrefab;
	public GameObject defensePrefab;
	public GameObject energyPrefab;
	public GameObject missilePrefab;
	public GameObject groundTilePrefab;

	private string exampleBot = "example-bot";
	private string exampleState = "example-state";

	private Bot bot;
	private GameState gameState;

	void Start () {
		LoadBotFromFile();
		LoadStateFromFile();
		PopulateSceneFromGameMap ();
	}

	void Update () {
		
	}

	private void PopulateSceneFromGameMap() {
		var map = gameState.GameMap;

		GameObject buildingsParent = Instantiate (new GameObject ());
		buildingsParent.name = BUILDINGS_PARENT;
		GameObject missilesParent = Instantiate (new GameObject ());
		missilesParent.name = MISSILES_PARENT;
		GameObject groundTilesParent = Instantiate (new GameObject ());
		groundTilesParent.name = GROUNDTILES_PARENT;

		for (int i = 0; i < map.Length; i++) {
			for (int j = 0; j < map[i].Length; j++) {
				var cell = map [i] [j];
				var x = cell.X;
				var y = cell.Y;

				InstantiateBuildingsAtLocation (buildingsParent, cell.Buildings, x, y);
				InstantiateMissileAtLocation (missilesParent, cell.Missiles, x, y);

				InstantiateGroundTile (groundTilesParent, j, i); // We need to reverse the orientation
			}
		}

	}

	private void InstantiateGroundTile(GameObject parent, int x, int y) {
		GameObject o = Instantiate (groundTilePrefab);
		o.transform.position = new Vector3 (x, o.transform.position.y, y);
		o.transform.SetParent (parent.transform);
	}

	private void InstantiateMissileAtLocation(GameObject parent, List<Missile> missiles, int x, int y) {
		for (int m = 0; m < missiles.Count; m++) {
			GameObject o = Instantiate (missilePrefab);
			o.transform.position = new Vector3 (x, o.transform.position.y, y);
			o.transform.SetParent (parent.transform);
		}
	}

	private void InstantiateBuildingsAtLocation(GameObject parent, List<Building> buildings, int x, int y) {
		for (int b = 0; b < buildings.Count; b++) {
			GameObject o = GetPrefabForBuilding (buildings[b]);
			if (o != null) {
				o = Instantiate (o);
				o.transform.position = new Vector3 (x, o.transform.position.y, y);
				o.transform.SetParent (parent.transform);
			}
		}
	}

	private GameObject GetPrefabForBuilding(Building building) {
		switch (building.BuildingType) {
			case BuildingType.Attack:
				return attackPrefab;
			case BuildingType.Defense:
				return defensePrefab;
			case BuildingType.Energy:
				return energyPrefab;
			default:
				return null;
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
