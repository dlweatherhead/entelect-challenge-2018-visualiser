using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

	public Text countdownText;
	public string destinationScene;

	void Start () {
		StartCoroutine (StartReplayScene ());
	}
	
	IEnumerator StartReplayScene() {
		yield return new WaitForSeconds (0.5f);
		countdownText.text = "3...";
		yield return new WaitForSeconds (1f);
		countdownText.text = "2...";
		yield return new WaitForSeconds (1f);
		countdownText.text = "1...";
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene(destinationScene, LoadSceneMode.Single);
	}
}
