using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFadeOut : MonoBehaviour {

    public float FadeTime;

    AudioSource audioSource;

    float originalVolume;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;
	}

    public void StartFadeOut() {
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut() {

        while (audioSource.volume > 0f) {
            audioSource.volume -= audioSource.volume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = originalVolume;
    }

}
