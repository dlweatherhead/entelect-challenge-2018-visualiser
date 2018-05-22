using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EC2018 {
	public class HealthBar : MonoBehaviour {

		public Slider Slider;

		public int MaxHealth;

		void Awake() {
			Slider.maxValue = MaxHealth;
		}
	
		public void SetHealth (int health) {
			Slider.value = Mathf.Clamp(health, 0f, MaxHealth);
		}
	}
}
