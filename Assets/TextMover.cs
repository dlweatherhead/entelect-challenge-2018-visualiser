using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMover : MonoBehaviour {

	public Vector3 startPos;

	RectTransform rectTransform;
	Vector3 targetPos;

	float lerpTime;

	void Start () {
		rectTransform = GetComponent<RectTransform> ();

		targetPos = rectTransform.position;
		rectTransform.position = startPos;
	}

	void Update () {
		if(lerpTime < 2.5f) {
			lerpTime += lerpTime + Time.deltaTime;
			rectTransform.position = Vector3.Lerp (rectTransform.position, targetPos, lerpTime);
		}
	}
}
