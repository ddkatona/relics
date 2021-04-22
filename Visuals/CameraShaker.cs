using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

    [Header("Manual")]
    public float duration;

    [Header("Internal")]
    public float t;
    public Vector3 center;
    public Vector3 direction;

    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) StartShake(Random.Range(0.1f,1));

        if(t < 1) {
            transform.position = center + direction * Mathf.Sin(Mathf.PI * t);
            t += Time.deltaTime / duration;
        } else if(t < 2) {
            transform.position = center;
            t = 3;
        }
    }

    public void StartShake(float size) {
        if (t < 3) return;
        direction = Random.insideUnitSphere * size;
        center = transform.position;
        t = 0;
    }
}
