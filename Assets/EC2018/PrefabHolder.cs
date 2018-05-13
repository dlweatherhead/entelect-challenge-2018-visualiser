using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour {
	public const string BUILDINGS_PARENT = "Buildings";
	public const string GROUNDTILES_PARENT = "GroundTiles";
	public const string MISSILES_PARENT = "Missiles";

	public GameObject attackPrefab_A;
	public GameObject defensePrefab_A;
	public GameObject energyPrefab_A;
	public GameObject missilePrefab_A;

	public GameObject attackPrefab_B;
	public GameObject defensePrefab_B;
	public GameObject energyPrefab_B;
	public GameObject missilePrefab_B;

	public GameObject groundTilePrefab;
}
