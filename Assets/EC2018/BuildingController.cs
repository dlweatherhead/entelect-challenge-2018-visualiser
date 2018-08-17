using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018;
using EC2018.Enums;

namespace EC2018 {
    
    public class BuildingController : MonoBehaviour {
        
        public int MaxHealth;
        
        public GameObject model;
        public GameObject constructionModel;
        
        public HealthBar HealthBar;

		public GameObject constructionAnimation;
        
		float lerpTime;

		float startScale;
		float endScale;

		float gameSpeed;

        public Building building {
            get;
            set;
        }
        
        void Awake() {

			gameSpeed = CommandLineUtil.GetRoundStep ();

			constructionModel.SetActive (false);
			model.SetActive (true);

            if(HealthBar != null) {
                HealthBar.MaxHealth = MaxHealth;
            }
        }
        
		void Update() {

			if(building != null && building.ConstructionTimeLeft >= 0) {
				if(lerpTime < gameSpeed) {
					lerpTime += Time.deltaTime;
					float value = Mathf.Lerp (startScale, endScale, lerpTime);
					transform.localScale = new Vector3(value, value, value);
					Color c = model.GetComponent<MeshRenderer> ().material.color;
					Color nc = new Color (value/2f, value/2f, value/2f);
					model.GetComponent<MeshRenderer> ().material.color = nc;
				}
			}
		}

        public void Setup (Building building) {
            this.building = building;
            
			int timeLeft = building.ConstructionTimeLeft;

			if (timeLeft >= 0) {
				if(building.BuildingType == BuildingType.Tesla) {
					startScale = (10f - timeLeft - 1f) / 10f;
					endScale = (10f - timeLeft) / 10f;
					transform.localScale = new Vector3(startScale, startScale, startScale);
					Color c = model.GetComponent<MeshRenderer> ().material.color;
					Color nc = new Color (startScale/2f, startScale/2f, startScale/2f);
					model.GetComponent<MeshRenderer> ().material.color = nc;
				} else {
					startScale = 0f;
					endScale = gameSpeed;
					transform.localScale = Vector3.zero;
				}
            }


			if(building.BuildingType == BuildingType.Tesla) {
				if(timeLeft + 1 == 10) {
					GameObject obj = Instantiate (constructionAnimation);
					obj.transform.position = transform.position + obj.transform.position;
					Destroy (obj, 10f * gameSpeed);
				}
			} else {
				if(timeLeft == 0) {
					GameObject obj = Instantiate (constructionAnimation, transform);
					Destroy (obj, gameSpeed);
				}
			}


            
            if(HealthBar != null) {
                HealthBar.SetHealth (building.Health);  
            }
            
        }
    }
    
}
