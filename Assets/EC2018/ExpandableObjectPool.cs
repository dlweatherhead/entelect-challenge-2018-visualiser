﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {
	public abstract class ExpandableObjectPool : MonoBehaviour {

		public GameObject poolObjectA;
		public GameObject poolObjectB;
		public Transform parent;
		public int poolSize = 5;

		List<GameObject> poolA;
		List<GameObject> poolB;

		string tag;

		void Start() {
			tag = GetTag ();
			poolA = new List<GameObject> ();
			poolB = new List<GameObject> ();
			InitialisePool (poolA, poolObjectA);
			InitialisePool (poolB, poolObjectB);
		}

		public GameObject GetForPlayerA() {
			return GetPoolObject (poolA, poolObjectA);
		}

		public GameObject GetForPlayerB() {
			return GetPoolObject (poolB, poolObjectB);
		}

		protected abstract string GetTag ();

		private void InitialisePool (List<GameObject> pool, GameObject poolObject) {
			for(int i=0; i < poolSize; i++) {
				GameObject obj = (GameObject)Instantiate (poolObject);
				obj.transform.SetParent (parent);
				obj.tag = tag;
				obj.SetActive (false);
				pool.Add (obj);
			}
		}

		private GameObject GetPoolObject (List<GameObject> pool, GameObject poolObject) {
			for(int i=0; i < pool.Count; i++) {
				if(!pool[i].activeInHierarchy) {
					return pool [i];
				}
			}

			GameObject obj = (GameObject)Instantiate (poolObject);
			obj.transform.SetParent (parent);
			obj.tag = tag;
			pool.Add (obj);
			return obj;
		}
	}
}