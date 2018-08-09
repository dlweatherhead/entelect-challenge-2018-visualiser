using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class MissileObjectPool : MonoBehaviour {

		public static MissileObjectPool current;
		public GameObject missilePlayerA;
		public GameObject missilePlayerB;

		public int poolAmount = 5;

		public List<GameObject> playerAMissiles;
		List<GameObject> playerBMissiles;

		void Awake() {
			current = this;
		}

		void Start () {
			InitialisePlayerAMissiles ();
			InitialisePlayerBMissiles ();
		}

		private void InitialisePlayerAMissiles() {
			playerAMissiles = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject ma = (GameObject)Instantiate (missilePlayerA);
				ma.SetActive (false);
				playerAMissiles.Add (ma);
			}
		}

		private void InitialisePlayerBMissiles() {
			playerBMissiles = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject mb = (GameObject)Instantiate (missilePlayerB);
				mb.SetActive (false);
				playerBMissiles.Add (mb);
			}
		}

		public GameObject GetPlayerAMissile() {
			for(int i=0; i < playerAMissiles.Count; i++) {
				if(!playerAMissiles[i].activeInHierarchy) {
					return playerAMissiles [i];
				}
			}
				
			GameObject ma = (GameObject)Instantiate (missilePlayerA);
			playerAMissiles.Add (ma);
			return ma;
		}

		public GameObject GetPlayerBMissile() {
			for(int i=0; i < playerBMissiles.Count; i++) {
				if(!playerBMissiles[i].activeInHierarchy) {
					return playerBMissiles [i];
				}
			}

			GameObject mb = (GameObject)Instantiate (missilePlayerB);
			playerBMissiles.Add (mb);
			return mb;
		}
	}
}
