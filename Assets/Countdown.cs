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
		yield return new WaitForSeconds (0.75f);
		countdownText.fontSize = 50;
		countdownText.text = "3";
		yield return new WaitForSeconds (1f);
		countdownText.fontSize = 80;
		countdownText.text = "2";
		yield return new WaitForSeconds (1f);
		countdownText.fontSize = 110;
		countdownText.text = "1";
		yield return new WaitForSeconds (1f);
		countdownText.fontSize = 150;
		countdownText.text = "FIGHT!";
		yield return new WaitForSeconds (0.75f);
		SceneManager.LoadScene(destinationScene, LoadSceneMode.Single);
	}
}
