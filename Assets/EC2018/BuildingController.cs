using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC2018.Entities;
using EC2018;
using EC2018.Enums;

namespace EC2018 {

    [RequireComponent(typeof(AudioSource))]
    public class BuildingController : MonoBehaviour {
        
        public int MaxHealth;
        
        public GameObject model;
        public GameObject constructionModel;
        
        public HealthBar HealthBar;

		public GameObject constructionAnimation;

		public bool isUnderConstruction;

        public AudioClip attackConstruction;
        public AudioClip energyConstruction;
        public AudioClip defenseConstruction;
        public AudioClip teslaConstruction;
        AudioSource audioSource;

		float lerpTime;

		float startScale;
		float endScale;

		float gameSpeed;

        public Building building {
            get;
            set;
        }
        
        void Awake() {
            audioSource = GetComponent<AudioSource> ();

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
				isUnderConstruction = true;
				if(building.BuildingType == BuildingType.Tesla) {
					startScale = (9f - timeLeft) /10f;
					endScale = (10f - timeLeft) / 10f;
				} else if(building.BuildingType == BuildingType.Defense) {
					startScale = (2f - timeLeft) / 3f;
					endScale = (3f - timeLeft) / 3f;
				} else {
					startScale = (0f - timeLeft) / 1f;
					endScale = (1f - timeLeft) / 1f;
				}
				SetConstructionParameters (startScale);
			}

            if(building.BuildingType == BuildingType.Tesla || building.BuildingType == BuildingType.Defense) {
                if(timeLeft < 0) {
                    isUnderConstruction = false;    
                }
            } else {
                if(timeLeft <= 0) {
                    isUnderConstruction = false;
                }
            }

			if(building.BuildingType == BuildingType.Tesla) {
				if(timeLeft == 9) {
					GameObject obj = Instantiate (constructionAnimation);
					obj.transform.position = transform.position + obj.transform.position;
					Destroy (obj, 9f * gameSpeed);

                    audioSource.clip = teslaConstruction;
                    audioSource.pitch = Random.Range(0.7f, 1.3f);
                    audioSource.Play();
				}
			} else if (building.BuildingType == BuildingType.Defense) {
				if(timeLeft == 2) {
					GameObject obj = Instantiate (constructionAnimation);
					obj.transform.position = transform.position + obj.transform.position;
					Destroy (obj, 2f * gameSpeed);

                    audioSource.clip = defenseConstruction;
                    audioSource.pitch = Random.Range(0.7f, 1.3f);
                    audioSource.Play();
				}
			} else {
				if(timeLeft == 0) {
					GameObject obj = Instantiate (constructionAnimation);
					obj.transform.position = transform.position + obj.transform.position;
					Destroy (obj, gameSpeed);

                    if(building.BuildingType == BuildingType.Attack) {
                        audioSource.clip = attackConstruction;
                    } else {
                        audioSource.clip = energyConstruction;
                        audioSource.volume = 0.3f;
                    }
                    audioSource.pitch = Random.Range(0.7f, 1.3f);
                    audioSource.Play();
				}
			}
				
            if(HealthBar != null) {
                HealthBar.SetHealth (building.Health);  
            }
            
        }

		private void SetConstructionParameters(float start) {
			transform.localScale = new Vector3(start, start, start);
			Color c = model.GetComponent<MeshRenderer> ().material.color;
			Color nc = new Color (start/2f, start/2f, start/2f);
			model.GetComponent<MeshRenderer> ().material.color = nc;
		}
    }
    
}
