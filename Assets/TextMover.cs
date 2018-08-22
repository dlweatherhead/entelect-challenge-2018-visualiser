using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMover : MonoBehaviour {

    public Vector3 startPos;
    public float startDelay = 0f;
    public float moveTime = 1f;

    RectTransform rectTransform;
    Vector3 targetPos;

    float lerpTime;

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
    }

    IEnumerator StartMoving() {
        yield return new WaitForSeconds(startDelay);
        lerpTime = 0f;
    }
}
