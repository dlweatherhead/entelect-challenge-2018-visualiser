using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EllipsisText : MonoBehaviour {

	public Text Text;
    public int MaxLength;

	void Update () {
        InvokeRepeating("CheckAndSetText", 0f, 0.1f);
	}

	void CheckAndSetText() {
        if(Text.text.Length > MaxLength) {
            Text.text = Text.text.Substring(0, MaxLength) + "...";
        }
	}
}
