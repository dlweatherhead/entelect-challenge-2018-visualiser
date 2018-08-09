﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018.Enums;

namespace EC2018
{
	[RequireComponent(typeof(PrefabHolder))]
	public class Instantiator : MonoBehaviour {

		private PrefabHolder prefabHolder;

		private GameObject buildingsParent;
		private GameObject missilesParent;
		private GameObject groundTilesParent;

		void Awake() {
			prefabHolder = GetComponent<PrefabHolder> ();
		}

		void Start() {
			buildingsParent = new GameObject (Constants.ParentNames.Buildings);
			missilesParent = new GameObject (Constants.ParentNames.Missiles);
			groundTilesParent = new GameObject (Constants.ParentNames.GroundTiles);
		}

		public void ClearScene() {
			ClearGameObjectsWithTag (Constants.Tags.Missile);
			ClearGameObjectsWithTag (Constants.Tags.Attack);
			ClearGameObjectsWithTag (Constants.Tags.Defense);
			ClearGameObjectsWithTag (Constants.Tags.Energy);
			ClearGameObjectsWithTag (Constants.Tags.Tesla);
		}

		private void ClearGameObjectsWithTag(string tag) {
			GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag (tag);
			for (int i = 0; i < taggedGameObjects.Length; i++) {
				taggedGameObjects[i].SetActive (false);
			}
		}

		public void InstantiateGroundTile(int x, int y) {
			GameObject o = Instantiate (prefabHolder.groundTilePrefab);
			o.transform.position = new Vector3 (x, o.transform.position.y, y);
			o.transform.SetParent (groundTilesParent.transform);
			o.tag = Constants.Tags.GroundTile;
		}

		public void InstantiateMissileAtLocation(List<Missile> missiles, int x, int y, float rate) {
			for (int m = 0; m < missiles.Count; m++) {
				PlayerType missilePlayerType = missiles [m].PlayerType;
				int direction = missilePlayerType == PlayerType.A ? 1 : -1;

				GameObject missile = missilePlayerType == PlayerType.A ?
					MissileObjectPool.current.GetForPlayerA () :
					MissileObjectPool.current.GetForPlayerB ();

				missile.SetActive (true);
				missile.transform.position = new Vector3 (x, missile.transform.position.y, y);
				missile.GetComponent<MissileController> ().Setup (direction, rate, missiles[m].Speed);
			}
		}

		public void InstantiateBuildingsAtLocation(List<Building> buildings, int x, int y) {
			for (int b = 0; b < buildings.Count; b++) {
				GameObject building = GetPrefabForBuilding (buildings[b], buildings[b].PlayerType);
				if (building != null) {
					building.SetActive (true);
					building.transform.position = new Vector3 (x, building.transform.position.y, y);
					building.GetComponent<BuildingController> ().Setup (buildings [b]);
				}
			}
		}

		private GameObject GetPrefabForBuilding(Building building, PlayerType playerType) {
			switch (building.BuildingType) {
				case BuildingType.Attack:
					return playerType == PlayerType.A ? 
						AttackObjectPool.current.GetForPlayerA () : 
						AttackObjectPool.current.GetForPlayerB ();
				case BuildingType.Defense:
					return playerType == PlayerType.A ? 
						DefenseObjectPool.current.GetForPlayerA () : 
						DefenseObjectPool.current.GetForPlayerB ();
				case BuildingType.Energy:
					return playerType == PlayerType.A ? 
						EnergyObjectPool.current.GetForPlayerA () : 
						EnergyObjectPool.current.GetForPlayerB ();
				case BuildingType.Tesla:
					return playerType == PlayerType.A ? 
						TeslaObjectPool.current.GetForPlayerA () : 
						TeslaObjectPool.current.GetForPlayerB ();
				default:
					return null;
			}
		}
	}
}
