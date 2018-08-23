using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

	public Text countdownText;
	public string destinationScene;
    public float startDelay = 1f;
    public float counterTime = 1f;
    public string[] kickOffPhrases;

	void Start () {
		StartCoroutine (StartReplayScene ());
	}
	
	IEnumerator StartReplayScene() {
        yield return new WaitForSeconds (startDelay);
		countdownText.fontSize = 50;
		countdownText.text = "3";
        yield return new WaitForSeconds (counterTime);
		countdownText.fontSize = 70;
		countdownText.text = "2";
        yield return new WaitForSeconds (counterTime);
		countdownText.fontSize = 100;
		countdownText.text = "1";
        yield return new WaitForSeconds (counterTime);
		countdownText.fontSize = 120;

        int i = Random.Range(0, kickOffPhrases.Length);

        countdownText.text = kickOffPhrases[i];
        yield return new WaitForSeconds (counterTime);
		SceneManager.LoadScene(destinationScene, LoadSceneMode.Single);
	}
}
