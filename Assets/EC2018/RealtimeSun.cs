using System.Collections;
using System.Collections.Generic;
using System.IO;
using EC2018;
using UnityEngine;

public class RealtimeSun : MonoBehaviour {

    public float RotationDistance;
    public Color startColor;
    public Color endColor;

    Light sunlight;
    int maxRounds = 1;
    float rotationSpeed;

    float t;

	void Start () {

        sunlight = GetComponent<Light>();

        maxRounds = GetMaxRounds();
        rotationSpeed = RotationDistance / maxRounds;
	}

    void Update() {

        sunlight.color = Color.Lerp(startColor, endColor, t);
        if (t < 1) { // while t below the end limit...
                     // increment it at the desired rate every update:
            t += Time.deltaTime / maxRounds;
        }

        //sunlight.color = Color.Lerp(sunlight.color, endColor, maxRounds);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                 transform.localEulerAngles.y + rotationSpeed * Time.deltaTime,
                                                 transform.localEulerAngles.z);
    }

    int GetMaxRounds() {
        var replayPath = PlayerPrefs.GetString(Constants.PlayerPrefKeys.SelectedReplay);
        return Directory.GetDirectories(replayPath).Length - 1; // Index offset
    }
}
