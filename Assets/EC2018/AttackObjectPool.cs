using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public class AttackObjectPool : MonoBehaviour {

		public static AttackObjectPool current;
		public GameObject attackPlayerA;
		public GameObject attackPlayerB;

		public int poolAmount = 5;

		public List<GameObject> attackPoolPlayerA;
		List<GameObject> attackPoolPlayerB;

		void Awake() {
			current = this;
		}

		void Start () {
			InitialisePoolPlayerA ();
			InitialisePoolPlayerB ();
		}

		private void InitialisePoolPlayerA() {
			attackPoolPlayerA = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject aa = (GameObject)Instantiate (attackPlayerA);
				aa.SetActive (false);
				attackPoolPlayerA.Add (aa);
			}
		}

		private void InitialisePoolPlayerB() {
			attackPoolPlayerB = new List<GameObject> ();
			for(int i=0; i < poolAmount; i++) {
				GameObject ab = (GameObject)Instantiate (attackPlayerB);
				ab.SetActive (false);
				attackPoolPlayerB.Add (ab);
			}
		}

		public GameObject GetAttackPlayerA() {
			for(int i=0; i < attackPoolPlayerA.Count; i++) {
				if(!attackPoolPlayerA[i].activeInHierarchy) {
					return attackPoolPlayerA [i];
				}
			}

			GameObject aa = (GameObject)Instantiate (attackPlayerA);
			attackPoolPlayerA.Add (aa);
			return aa;
		}

		public GameObject GetAttackPlayerB() {
			for(int i=0; i < attackPoolPlayerB.Count; i++) {
				if(!attackPoolPlayerB[i].activeInHierarchy) {
					return attackPoolPlayerB [i];
				}
			}

			GameObject ab = (GameObject)Instantiate (attackPlayerB);
			attackPoolPlayerB.Add (ab);
			return ab;
		}
	}

}
