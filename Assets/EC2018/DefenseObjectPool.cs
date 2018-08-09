using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class DefenseObjectPool : MonoBehaviour {

		public static DefenseObjectPool current;
		public GameObject defensePlayerA;
		public GameObject defensePlayerB;

		public int poolAmount = 5;

		public List<GameObject> playerADefenses;
		List<GameObject> playerBDefenses;

		void Awake() {
			current = this;
		}

		void Start () {
			InitialisePlayerADefenses ();
			InitialisePlayerBDefenses ();
		}

		private void InitialisePlayerADefenses() {
			playerADefenses = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject da = (GameObject)Instantiate (defensePlayerA);
				da.SetActive (false);
				playerADefenses.Add (da);
			}
		}

		private void InitialisePlayerBDefenses() {
			playerBDefenses = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject db = (GameObject)Instantiate (defensePlayerB);
				db.SetActive (false);
				playerBDefenses.Add (db);
			}
		}

		public GameObject GetPlayerADefense() {
			for(int i=0; i < playerADefenses.Count; i++) {
				if(!playerADefenses[i].activeInHierarchy) {
					return playerADefenses [i];
				}
			}

			GameObject da = (GameObject)Instantiate (defensePlayerA);
			playerADefenses.Add (da);
			return da;
		}

		public GameObject GetPlayerBDefense() {
			for(int i=0; i < playerBDefenses.Count; i++) {
				if(!playerBDefenses[i].activeInHierarchy) {
					return playerBDefenses [i];
				}
			}

			GameObject db = (GameObject)Instantiate (defensePlayerB);
			playerBDefenses.Add (db);
			return db;
		}
	}
}