using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {

	public int StartX;
	public int TargetX;

	public float speed = 1f;

	private float nextStatePosition;

	public void Setup(int distance, float rate, int missileSpeed) {
		transform.GetChild (0).gameObject.SetActive (true);
		speed = missileSpeed * distance / rate;
		nextStatePosition = transform.position.x + distance * missileSpeed;
	}

	void Update() {
		transform.Translate (Vector3.right * speed * Time.deltaTime);

		if(speed > 0) {
			if(transform.position.x > nextStatePosition) {
				HaltAndSetToNextStatePosition ();
			}
		} else {
			if(transform.position.x < nextStatePosition) {
				HaltAndSetToNextStatePosition ();
			}
		}
	}

	public void HaltAndSetToNextStatePosition() {
		Halt ();
		transform.position = new Vector3 (nextStatePosition, transform.position.y, transform.position.z);
	}

	public void Halt() {
		speed = 0;
	}
}
