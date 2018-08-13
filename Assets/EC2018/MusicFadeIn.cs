using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeIn : MonoBehaviour {

    public float FadeTime;

    AudioSource audioSource;

    float targetVolume;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        targetVolume = audioSource.volume;
        audioSource.volume = 0f;

        StartCoroutine("FadeIn");
	}

    //private void Update() {
    //    while(counter < FadeTime) {

    //        audioSource.volume += targetVolume * Time.deltaTime / FadeTime;

    //        counter += Time.delta
    //    }
    //}

    IEnumerator FadeIn() {
        while (audioSource.volume < targetVolume) {
            audioSource.volume += targetVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

}
