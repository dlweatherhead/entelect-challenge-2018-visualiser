using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018.Enums;
using DigitalRuby.LightningBolt;

namespace EC2018
{
	public class Instantiator : MonoBehaviour {

		public GameObject groundTilePrefabA;
		public GameObject groundTilePrefabB;
		public Transform groundTileParent;

        public GameObject ironCurtainPlayerA;
        public GameObject ironCurtainPlayerB;

        public GameObject lightningBolt;

        public GameObject playerADestructionAnimation;
        public GameObject playerBDestructionAnimation;

        float timeStep;

        void Awake() {
            timeStep = CommandLineUtil.GetRoundStep();
        }

        public void ClearScene() {
			ClearGameObjectsWithTag (Constants.Tags.Missile);
			ClearGameObjectsWithTag (Constants.Tags.Attack);
			ClearGameObjectsWithTag (Constants.Tags.Defense);
			ClearGameObjectsWithTag (Constants.Tags.Energy);
			ClearGameObjectsWithTag (Constants.Tags.Tesla);
		}

        void ClearGameObjectsWithTag(string objectTag) {
            var taggedGameObjects = GameObject.FindGameObjectsWithTag(objectTag);
            for (int i = 0; i < taggedGameObjects.Length; i++) {
//                taggedGameObjects[i].SetActive(false);
				Destroy (taggedGameObjects[i]);
            }
        }

        public void InstantiateGroundTile(int x, int y, PlayerType playerType) {
			var o = playerType == PlayerType.A ?
				Instantiate (groundTilePrefabA) :
				Instantiate (groundTilePrefabB);
			o.transform.position = new Vector3 (x, o.transform.position.y, y);
			o.transform.SetParent (groundTileParent);
			o.tag = Constants.Tags.GroundTile;
		}

		public void InstantiateMissileAtLocation(List<Missile> missiles, int x, int y, float rate) {
            float interval = 0.15f;

			for (int m = 0; m < missiles.Count; m++) {

                float z = y;
                if(m > 0 && m % 2 == 0) {
                    z += m * interval;
                } else {
                    z += m * interval;
                }

                float x_offset = x;
                if(missiles[m].PlayerType == PlayerType.A) {
                    x_offset -= timeStep;
                } else {
                    x_offset += timeStep;
                }

				PlayerType missilePlayerType = missiles [m].PlayerType;
				int direction = missilePlayerType == PlayerType.A ? 1 : -1;

				var missile = missilePlayerType == PlayerType.A ?
					MissileObjectPool.current.GetForPlayerA () :
					MissileObjectPool.current.GetForPlayerB ();

				missile.SetActive (true);
				missile.transform.position = new Vector3 (x_offset, missile.transform.position.y, z);
				missile.GetComponent<MissileController> ().Setup (missiles[m], direction, rate);
			}
		}

		public void InstantiateBuildingsAtLocation(List<Building> buildings, int x, int y) {
			for (int b = 0; b < buildings.Count; b++) {
				var building = GetPrefabForBuilding (buildings[b], buildings[b].PlayerType);
				if (building != null) {
					building.SetActive (true);
					building.transform.position = new Vector3 (x, building.transform.position.y, y);
					building.GetComponent<BuildingController> ().Setup (buildings [b]);
				}
			}
		}

		public void InstantiateTeslaHit(List<HitList> hitList) {
			for(int i=0; i < hitList.Count-1; i++) {
				var origin = hitList [i];
				var target = hitList [i + 1];

				var start = new GameObject();
				var end = new GameObject();
				start.transform.position = new Vector3(origin.X, 0.5f, origin.Y);
				end.transform.position = new Vector3(target.X, 0.5f, target.Y);

				var lightningBoltObj = Instantiate(lightningBolt);
				var lightningBoltScript = lightningBoltObj.GetComponent<LightningBoltScript>();
				var lightningBoltLineRenderer = lightningBoltObj.GetComponent<LineRenderer>();

				lightningBoltLineRenderer.startWidth = 0.2f;
				lightningBoltLineRenderer.endWidth = 0.2f;

				lightningBoltScript.StartObject = start;
				lightningBoltScript.EndObject = end;

				Destroy (lightningBoltObj, CommandLineUtil.GetRoundStep ());
			}

            if(hitList[0].EnemyBaseHit) {
                var origin = hitList[0];
                var target = new HitList {
                    X = origin.PlayerType == PlayerType.A ? -1 : 16,
                    Y = origin.Y,
                    PlayerType = origin.PlayerType
                };

                var start = new GameObject();
                var end = new GameObject();
                start.transform.position = new Vector3(origin.X, 0.5f, origin.Y);
                end.transform.position = new Vector3(target.X, 0.5f, target.Y);

                var lightningBoltObj = Instantiate(lightningBolt);
                var lightningBoltScript = lightningBoltObj.GetComponent<LightningBoltScript>();
                var lightningBoltLineRenderer = lightningBoltObj.GetComponent<LineRenderer>();

                lightningBoltLineRenderer.startWidth = 0.2f;
                lightningBoltLineRenderer.endWidth = 0.2f;

                lightningBoltScript.StartObject = start;
                lightningBoltScript.EndObject = end;

                Destroy(lightningBoltObj, CommandLineUtil.GetRoundStep());
            }

            if (hitList[0].enemyIronCurtainHit) {
                var origin = hitList[0];
                var target = new HitList {
                    X = origin.X,
                    Y = origin.Y,
                    PlayerType = origin.PlayerType
                };

                var start = new GameObject();
                var end = new GameObject();
                start.transform.position = new Vector3(origin.X, 0.5f, origin.Y);
                end.transform.position = new Vector3(7.5f, 0.5f, target.Y);

                var lightningBoltObj = Instantiate(lightningBolt);
                var lightningBoltScript = lightningBoltObj.GetComponent<LightningBoltScript>();
                var lightningBoltLineRenderer = lightningBoltObj.GetComponent<LineRenderer>();

                lightningBoltLineRenderer.startWidth = 0.2f;
                lightningBoltLineRenderer.endWidth = 0.2f;

                lightningBoltScript.StartObject = start;
                lightningBoltScript.EndObject = end;

                Destroy(lightningBoltObj, CommandLineUtil.GetRoundStep());
            }
		}

		public void ActivateIronCurtain(PlayerType playerType) {
            var obj = playerType == PlayerType.A ? ironCurtainPlayerA : ironCurtainPlayerB;
            obj.SetActive(true);
		}

        public void DeactivateIronCurtain(PlayerType playerType) {
            var obj = playerType == PlayerType.A ? ironCurtainPlayerA : ironCurtainPlayerB;
            obj.SetActive(false);
        }

        public void InstantiateEndGameAnimations(PlayerType playerType) {
            if(playerType == PlayerType.A) {
                playerADestructionAnimation.SetActive(true);
            } else {
                playerBDestructionAnimation.SetActive(true);
            }
            
        }

        GameObject GetPrefabForBuilding(Building building, PlayerType playerType) {
            switch (building.BuildingType) {
                case BuildingType.Attack:
                    return playerType == PlayerType.A ?
                        AttackObjectPool.current.GetForPlayerA() :
                        AttackObjectPool.current.GetForPlayerB();
                case BuildingType.Defense:
                    return playerType == PlayerType.A ?
                        DefenseObjectPool.current.GetForPlayerA() :
                        DefenseObjectPool.current.GetForPlayerB();
                case BuildingType.Energy:
                    return playerType == PlayerType.A ?
                        EnergyObjectPool.current.GetForPlayerA() :
                        EnergyObjectPool.current.GetForPlayerB();
                case BuildingType.Tesla:
                    return playerType == PlayerType.A ?
                        TeslaObjectPool.current.GetForPlayerA() :
                        TeslaObjectPool.current.GetForPlayerB();
                default:
                    return null;
            }
        }
    }
}
