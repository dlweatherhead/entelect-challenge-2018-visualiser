using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018.Enums;

namespace EC2018
{
	public class Instantiator : MonoBehaviour {

		public GameObject groundTilePrefabA;
		public GameObject groundTilePrefabB;
		public Transform groundTileParent;

        public GameObject ironCurtainPlayerA;
        public GameObject ironCurtainPlayerB;

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

		public void InstantiateGroundTile(int x, int y, PlayerType playerType) {
			GameObject o = playerType == PlayerType.A ?
				Instantiate (groundTilePrefabA) :
				Instantiate (groundTilePrefabB);
			o.transform.position = new Vector3 (x, o.transform.position.y, y);
			o.transform.SetParent (groundTileParent);
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

				var z = m < missiles.Count / 2 ? y + m / 10 : y - m / 10;
				missile.transform.position = new Vector3 (x, missile.transform.position.y, z);
				missile.GetComponent<MissileController> ().Setup (missiles[m], direction, rate);
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

		public void InstantiateTeslaHit(float targetX, float targetY, PlayerType playerType, float teslaX, float teslaY) {

			var color = playerType == PlayerType.A ? Color.red : Color.blue;

			GameObject lineObject = new GameObject ();
			LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer> ();
			lineObject.transform.position = new Vector3 (teslaX, 0.5f, teslaY);
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			lineRenderer.startWidth = 0.2f;
			lineRenderer.endWidth = 0.3f;

			var positions = new Vector3[2];
			positions[0] = new Vector3 (teslaX, 0.5f, teslaY);
			positions [1] = new Vector3 (targetX, 0.5f, targetY);

			lineRenderer.SetPositions (positions);

			Destroy (lineObject, 1f);
		}

		public void ActivateIronCurtain(PlayerType playerType) {
            var obj = playerType == PlayerType.A ? ironCurtainPlayerA : ironCurtainPlayerB;
            obj.SetActive(true);
		}

        public void DeactivateIronCurtain(PlayerType playerType) {
            var obj = playerType == PlayerType.A ? ironCurtainPlayerA : ironCurtainPlayerB;
            obj.SetActive(false);
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
