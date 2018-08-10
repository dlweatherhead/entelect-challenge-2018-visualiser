using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHealth : MonoBehaviour {

	public int MaxHealth;

    MeshRenderer meshRenderer;
    float startAlpha;

    void Awake() {
		meshRenderer = GetComponent<MeshRenderer> ();
        startAlpha = meshRenderer.material.color.a;
	}

	public void SetHealth(int health) {
        var color = meshRenderer.material.color;
        var alpha = startAlpha * health / MaxHealth;
        meshRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
	}

}
