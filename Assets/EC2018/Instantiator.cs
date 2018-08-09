using System.Collections;
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
			ClearGameObjectsWithTag (GetTagForBuildingType(BuildingType.Attack));
			ClearGameObjectsWithTag (GetTagForBuildingType(BuildingType.Defense));
			ClearGameObjectsWithTag (GetTagForBuildingType(BuildingType.Energy));
		}

		private void ClearGameObjectsWithTag(string tag) {
			if (tag == null) {
				return;
			}

			GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag (tag);
			for (int i = 0; i < taggedGameObjects.Length; i++) {
				taggedGameObjects[i].SetActive (false);
			}
		}

		public void InstantiateGroundTile(int x, int y) {
			GameObject o = Instantiate (prefabHolder.groundTilePrefab);
			o.transform.position = new Vector3 (x, o.transform.position.y, y);
			o.transform.SetParent (buildingsParent.transform);
		}

		public void InstantiateMissileAtLocation(List<Missile> missiles, int x, int y, float rate) {
			for (int m = 0; m < missiles.Count; m++) {
				GameObject missile;
				PlayerType missilePlayerType = missiles [m].PlayerType;
				int direction = missilePlayerType == PlayerType.A ? 1 : -1;

				if(missilePlayerType == PlayerType.A) {
					missile = MissileObjectPool.current.GetPlayerAMissile ();
				} else {
					missile = MissileObjectPool.current.GetPlayerBMissile ();
				}

				missile.transform.position = new Vector3 (x, missile.transform.position.y, y);
				missile.transform.SetParent (missilesParent.transform);
				missile.SetActive (true);
				missile.GetComponent<MissileController> ().Setup (direction, rate, missiles[m].Speed);
			}
		}

		public void InstantiateBuildingsAtLocation(List<Building> buildings, int x, int y) {
			for (int b = 0; b < buildings.Count; b++) {
				GameObject o = GetPrefabForBuilding (buildings[b], buildings[b].PlayerType);
				if (o != null) {
					o.transform.position = new Vector3 (x, o.transform.position.y, y);
					o.transform.SetParent (groundTilesParent.transform);
					o.SetActive (true);
					o.tag = GetTagForBuildingType(buildings [b].BuildingType);
					o.GetComponent<BuildingController> ().Setup (buildings [b]);
				}
			}
		}

		private GameObject GetPrefabForBuilding(Building building, PlayerType playerType) {
			switch (building.BuildingType) {
				case BuildingType.Attack:
					return playerType == PlayerType.A ? 
						AttackObjectPool.current.GetAttackPlayerA () : 
						AttackObjectPool.current.GetAttackPlayerB ();
				case BuildingType.Defense:
					return playerType == PlayerType.A ? 
						DefenseObjectPool.current.GetPlayerADefense () : 
						DefenseObjectPool.current.GetPlayerBDefense ();
				case BuildingType.Energy:
					return playerType == PlayerType.A ? 
						EnergyObjectPool.current.GetEnergyPlayerA () : 
						EnergyObjectPool.current.GetEnergyPlayerB ();
				default:
					return null;
			}
		}

		private string GetTagForBuildingType(BuildingType buildingType) {
			switch (buildingType) {
				case BuildingType.Attack:
					return Constants.Tags.Attack;
				case BuildingType.Defense:
					return Constants.Tags.Defense;
				case BuildingType.Energy:
					return Constants.Tags.Energy;
				default:
					return null;
			}
		}
	}
}
