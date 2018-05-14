using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;

public class BuildingController : MonoBehaviour {

	public Material constructionMaterial;

	private Building building;

	public void Setup (Building building) {
		this.building = building;

		if (building.ConstructionTimeLeft > 0) {
			GetComponent<Renderer> ().material = constructionMaterial;
		}
	}
}
