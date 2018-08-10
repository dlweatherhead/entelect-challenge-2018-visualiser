using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018;

public class BuildingController : MonoBehaviour {

	public int MaxHealth;

	public GameObject model;
	public GameObject constructionModel;

	public HealthBar HealthBar;

	public Building building {
		get;
		set;
	}

	void Awake() {
		if(HealthBar != null) {
			HealthBar.MaxHealth = MaxHealth;
		}
	}

	public void Setup (Building building) {
		this.building = building;

		if (building.ConstructionTimeLeft > 0) {
			model.SetActive (false);
			constructionModel.SetActive (true);
		} else {
			model.SetActive (true);
			constructionModel.SetActive (false);
		}

		if(HealthBar != null) {
			HealthBar.SetHealth (building.Health);	
		}

	}
}
