using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour {
	public const string BUILDINGS_PARENT = "Buildings";
	public const string GROUNDTILES_PARENT = "GroundTiles";
	public const string MISSILES_PARENT = "Missiles";

	public GameObject attackPrefab;
	public GameObject defensePrefab;
	public GameObject energyPrefab;
	public GameObject missilePrefab;
	public GameObject groundTilePrefab;
}
