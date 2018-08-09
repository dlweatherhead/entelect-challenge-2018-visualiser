using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class EnergyObjectPool : MonoBehaviour {

		public static EnergyObjectPool current;
		public GameObject energyPlayerA;
		public GameObject energyPlayerB;

		public int poolAmount = 5;

		public List<GameObject> energyPoolPlayerA;
		List<GameObject> energyPoolPlayerB;

		void Awake() {
			current = this;
		}

		void Start () {
			InitialisePoolPlayerA ();
			InitialisePoolPlayerB ();
		}

		private void InitialisePoolPlayerA() {
			energyPoolPlayerA = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject ea = (GameObject)Instantiate (energyPlayerA);
				ea.SetActive (false);
				energyPoolPlayerA.Add (ea);
			}
		}

		private void InitialisePoolPlayerB() {
			energyPoolPlayerB = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject eb = (GameObject)Instantiate (energyPlayerB);
				eb.SetActive (false);
				energyPoolPlayerB.Add (eb);
			}
		}

		public GameObject GetEnergyPlayerA() {
			for(int i=0; i < energyPoolPlayerA.Count; i++) {
				if(!energyPoolPlayerA[i].activeInHierarchy) {
					return energyPoolPlayerA [i];
				}
			}

			GameObject ea = (GameObject)Instantiate (energyPlayerA);
			energyPoolPlayerA.Add (ea);
			return ea;
		}

		public GameObject GetEnergyPlayerB() {
			for(int i=0; i < energyPoolPlayerB.Count; i++) {
				if(!energyPoolPlayerB[i].activeInHierarchy) {
					return energyPoolPlayerB [i];
				}
			}

			GameObject eb = (GameObject)Instantiate (energyPlayerB);
			energyPoolPlayerB.Add (eb);
			return eb;
		}
	}

}
