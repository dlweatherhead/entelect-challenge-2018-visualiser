using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour {

	public float delay = 2f;

	void Start () {
		Destroy (gameObject, delay);
	}
}
