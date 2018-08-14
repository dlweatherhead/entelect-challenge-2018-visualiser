using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMover : MonoBehaviour {

	public Vector3 startPos;

	RectTransform rectTransform;
	Vector3 targetPos;

	void Start () {
		rectTransform = GetComponent<RectTransform> ();

		targetPos = rectTransform.position;
		rectTransform.position = startPos;
	}

	void Update () {
		var lerp = Vector3.Lerp (rectTransform.position, targetPos, 0.5f);
		rectTransform.position = lerp;
	}
}
