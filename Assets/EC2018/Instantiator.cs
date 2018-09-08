using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018.Enums;
using DigitalRuby.LightningBolt;

namespace EC2018
{
    [RequireComponent(typeof(AudioSource))]
	public class Instantiator : MonoBehaviour {

		public GameObject groundTilePrefabA;
		public GameObject groundTilePrefabB;
		public Transform groundTileParent;

        public GameObject ironCurtainPlayerA;
        public GameObject ironCurtainPlayerB;

        public GameObject lightningBoltA;
        public GameObject lightningBoltB;

        public GameObject explosionA;
        public GameObject explosionB;

        public GameObject muzzleFlashPlayerA;
        public GameObject muzzleFlashPlayerB;

        public GameObject playerADestructionAnimation;
        public GameObject playerBDestructionAnimation;

        public AudioSource teslaFiringSourceA;
        public AudioSource teslaFiringSourceB;

        public AudioClip[] firingSoundsPlayerA;
        public AudioClip[] firingSoundsPlayerB;

        List<string> previousMissiles;

        void Start() {
            previousMissiles = new List<string>();
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

				PlayerType missilePlayerType = missiles [m].PlayerType;
				int direction = missilePlayerType == PlayerType.A ? 1 : -1;

				var missile = missilePlayerType == PlayerType.A ?
					MissileObjectPool.current.GetForPlayerA () :
					MissileObjectPool.current.GetForPlayerB ();
                
				missile.SetActive (true);
				missile.transform.position = new Vector3 (x, missile.transform.position.y, z);
                MissileController missileCtrl = missile.GetComponent<MissileController>();
                missileCtrl.Setup (missiles[m], direction, rate);

                if (IsNewMissile(missileCtrl.missile)) {

                    // Play muzzleflash effect
                    var pos = missile.transform.position;

                    var offset = missileCtrl.missile.PlayerType == PlayerType.A ? -0.5f : 0.25f;
                    var muzzleFlash = missileCtrl.missile.PlayerType == PlayerType.A ? muzzleFlashPlayerA : muzzleFlashPlayerB;

                    pos.x -= direction * missileCtrl.missile.Speed + offset;
                    pos.y = 0.5f;
                    var muzzleFlashObj = Instantiate(muzzleFlash, pos, Quaternion.identity);
                    Destroy(muzzleFlashObj, CommandLineUtil.GetRoundStep());

                    AudioClip clip;

                    if(missileCtrl.missile.PlayerType == PlayerType.A) {
                        clip = firingSoundsPlayerA[Random.Range(0, firingSoundsPlayerA.Length)];
                    } else {
                        clip = firingSoundsPlayerB[Random.Range(0, firingSoundsPlayerB.Length)];
                    }

                    missileCtrl.PlaySound(clip);
                }
			}
		}

        bool IsNewMissile(Missile missile) {
            if (previousMissiles.Contains(missile.Id)) {
                return false;
            } else {
                previousMissiles.Add(missile.Id);
                return true;
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

        private void InstantiateLightningHit(PlayerType originPlayer, GameObject start, GameObject end) {
            var lightningToInstantiate = originPlayer == PlayerType.A ? lightningBoltA : lightningBoltB;
            
            var lightningBoltObj = Instantiate(lightningToInstantiate);
            var lightningBoltScript = lightningBoltObj.GetComponent<LightningBoltScript>();
            var lightningBoltLineRenderer = lightningBoltObj.GetComponent<LineRenderer>();

            lightningBoltLineRenderer.startWidth = 0.15f;
            lightningBoltLineRenderer.endWidth = 0.15f;

            lightningBoltScript.StartObject = start;
            lightningBoltScript.EndObject = end;

            Destroy(lightningBoltObj, CommandLineUtil.GetRoundStep());

            var explosionToInstantiate = originPlayer == PlayerType.B ? explosionA : explosionB;
            var expObj = Instantiate(explosionToInstantiate, start.transform.position, Quaternion.identity);
            expObj.transform.localScale = expObj.transform.localScale * 0.5f;
        }

        public void InstantiateTeslaHit(List<HitList> hitList, Player playerA, Player playerB) {
            
            // if list is empty
            if (hitList.Count < 1) return;

            var originIndex = hitList.Count - 1;
            var originTower = hitList[originIndex];
            var originPlayer = originTower.PlayerType;

            if(originPlayer == PlayerType.A) {
                teslaFiringSourceA.Play();
            } else {
                teslaFiringSourceB.Play();
            }

            // IRON CURTAIN HIT
            bool isIronCurtainHit = false;
            if(originTower.PlayerType == PlayerType.A) {
                isIronCurtainHit |= playerB.ActiveIronCurtainLifetime >= 0;
            } else {
                isIronCurtainHit |= playerA.ActiveIronCurtainLifetime >= 0;
            }
            if (isIronCurtainHit) {
                var start = new GameObject();
                var end = new GameObject();
                start.transform.position = new Vector3(originTower.X, 0.5f, originTower.Y);
                end.transform.position = new Vector3(7.5f, 0.5f, originTower.Y);
                InstantiateLightningHit(originPlayer, start, end);
                return;
            }

            // if only 1 hit in list (i.e. iron curtain hit)
            if (hitList.Count < 2) return;

            // FINAL CELL/BASE HIT
            var lastHit = hitList[0];
            var loopIndex = 0;

            if (lastHit.X == 0 && lastHit.Y == 0) {
                var lastTower = hitList[1];
                var targetX = originTower.PlayerType == PlayerType.A ? 16 : -1;

                var start = new GameObject();
                var end = new GameObject();
                start.transform.position = new Vector3(lastTower.X, 0.5f, lastTower.Y);
                end.transform.position = new Vector3(targetX, 0.5f, lastTower.Y);

                InstantiateLightningHit(originPlayer, start, end);

                loopIndex = 1;
            }

            // If only 2 in list (enemy base hit and origin
            if (loopIndex == 1 && hitList.Count < 3) return;

            // HIT CHAIN TILL FINAL
            for(int i=loopIndex; i < hitList.Count-1; i++) {
				var origin = hitList [i];
				var target = hitList [i + 1];

				var start = new GameObject();
				var end = new GameObject();
				start.transform.position = new Vector3(origin.X, 0.5f, origin.Y);
				end.transform.position = new Vector3(target.X, 0.5f, target.Y);

                InstantiateLightningHit(originPlayer, start, end);
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
