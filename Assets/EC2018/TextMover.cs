using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMover : MonoBehaviour {

    public Vector3 startPos;
    public float startDelay = 0f;
    public float moveTime = 1f;

    public AudioSource onStopSound;

    RectTransform rectTransform;
    Vector3 targetPos;

    float lerpTime;

    bool stopped;

    void Start() {
        rectTransform = GetComponent<RectTransform>();

        targetPos = rectTransform.position;
        rectTransform.position = startPos;

        lerpTime = moveTime + 1f;
        StartCoroutine(StartMoving());
    }

    void Update() {
        if (lerpTime < moveTime) {
            lerpTime += Time.deltaTime;
            rectTransform.position = Vector3.Lerp(rectTransform.position, targetPos, lerpTime);
        }

        if(Vector3.Distance(rectTransform.position, targetPos) < 50) {
            if(!stopped) {
                onStopSound.Play();
                stopped = true;
            }
        }
    }

    IEnumerator StartMoving() {
        yield return new WaitForSeconds(startDelay);
        lerpTime = 0f;
    }
}
