using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018;
using EC2018.Entities;

public class MissileController : MonoBehaviour {

	public int StartX;
	public int TargetX;
	public float speed = 1f;
	public GameObject explosion;

	private Missile missile;
	private float nextStatePosition;

	public void Setup(Missile missile, int distance, float rate) {
		this.missile = missile;
		transform.GetChild (0).gameObject.SetActive (true);
		speed = missile.Speed * distance / rate;
		nextStatePosition = transform.position.x + distance * missile.Speed;
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

	void OnTriggerEnter(Collider other) {
		switch(other.tag) {
			case Constants.Tags.MissileCollider:
				var buildingCtrl = other.gameObject.GetComponentInParent<BuildingController> ();
				if (missile.PlayerType != buildingCtrl.building.PlayerType) {
					Instantiate (explosion, transform.position, Quaternion.identity);
					gameObject.SetActive (false);
				}
				break;
			case Constants.Tags.Barrier:
				Instantiate (explosion, transform.position, Quaternion.identity);
				gameObject.SetActive (false);
				break;
		}
	}
}
