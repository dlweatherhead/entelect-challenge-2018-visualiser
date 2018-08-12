using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosionSpawner : MonoBehaviour {

    public GameObject explosion;
    public float width;
    public float height;
    public float rate;
    public float amount;

    private float timer;

    private void Update() {
        if (timer < 0) {
            var x = transform.position.x + Random.Range(0f, width);
            var z = transform.position.z + Random.Range(0f, height);

            Instantiate(explosion, new Vector3(x, 0.5f, z), Quaternion.identity);

            timer = rate;
        } else {
            timer -= Time.deltaTime;
        }
        
    }
}
