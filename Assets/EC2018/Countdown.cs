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

    public AudioClip[] announcerCountdown; // Must be 3 clips

    public AudioClip[] announcerPhrases;
    public AudioSource announcerAudioSource;

	void Start () {
		StartCoroutine (StartReplayScene ());
	}
	
	IEnumerator StartReplayScene() {
        yield return new WaitForSeconds (startDelay);
		countdownText.fontSize = 40;
		countdownText.text = "3";
        announcerAudioSource.clip = announcerCountdown[2];
        announcerAudioSource.Play();
        yield return new WaitForSeconds (counterTime);
		countdownText.fontSize = 60;
		countdownText.text = "2";
        announcerAudioSource.clip = announcerCountdown[1];
        announcerAudioSource.Play();
        yield return new WaitForSeconds (counterTime);
		countdownText.fontSize = 80;
		countdownText.text = "1";
        announcerAudioSource.clip = announcerCountdown[0];
        announcerAudioSource.Play();
        yield return new WaitForSeconds (counterTime);
		countdownText.fontSize = 100;

        int i = Random.Range(0, kickOffPhrases.Length);

        announcerAudioSource.clip = announcerPhrases[i];
        announcerAudioSource.Play();

        countdownText.text = kickOffPhrases[i];
        yield return new WaitForSeconds (counterTime);
		SceneManager.LoadScene(destinationScene, LoadSceneMode.Single);
	}
}
