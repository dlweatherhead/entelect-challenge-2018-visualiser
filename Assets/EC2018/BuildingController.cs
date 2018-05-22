using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018;

public class BuildingController : MonoBehaviour {

	public int MaxHealth;

	public Material constructionMaterial;

	public HealthBar HealthBar;

	private Building building;

	void Awake() {
		if(HealthBar != null) {
			HealthBar.MaxHealth = MaxHealth;
		}
	}

	public void Setup (Building building) {
		this.building = building;

		if (building.ConstructionTimeLeft > 0) {
			GetComponent<Renderer> ().material = constructionMaterial;
		}

		if(HealthBar != null) {
			HealthBar.SetHealth (building.Health);	
		}

	}
}
