using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {

	public int StartX;
	public int TargetX;

	public float speed = 1f;

	public void Setup(int distance, float rate) {
		speed = distance / rate;
	}

	void Update() {
		transform.Translate (Vector3.right * speed * Time.deltaTime);
	}

	public void Halt() {
		speed = 0;
	}
}
