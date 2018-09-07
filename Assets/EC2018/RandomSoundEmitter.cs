using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundEmitter : MonoBehaviour {
    
    public AudioClip[] sounds;
    public AudioSource audioSource;

	void Start () {
        var index = Random.Range(0, sounds.Length);
        audioSource.clip = sounds[index];
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
	}
	
}
