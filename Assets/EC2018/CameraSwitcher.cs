using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EC2018 {

	public class CameraSwitcher : MonoBehaviour {

		public GameObject SideViewCamera;
		public GameObject TopViewCamera;

		private GameObject activeCamera;

		void Start() {
			SideViewCamera.SetActive(true);
			TopViewCamera.SetActive(false);
			activeCamera = SideViewCamera;
		}

		void Update () {
			if (Input.GetKeyDown(KeyCode.C)) {
				Debug.Log ("Switching Camera");
				OnCameraSwithClicked ();
			}
		}

		public void OnCameraSwithClicked() {
			if(activeCamera == SideViewCamera) {
				Debug.Log ("Top Camera Active");
				TopViewCamera.SetActive(true);
				SideViewCamera.SetActive(false);
				activeCamera = TopViewCamera;
			} else if (activeCamera == TopViewCamera) {
				Debug.Log ("Side Camera Active");
				SideViewCamera.SetActive(true);
				TopViewCamera.SetActive(false);
				activeCamera = SideViewCamera;
			}
		}
	}
}
