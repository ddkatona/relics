using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearButton : MonoBehaviour {

    public Settings settings;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) settings.Toggle();
    }

    public void OpenSettings() {
        settings.Toggle();
    }
    
    public void Hover() {
        GetComponent<AudioSource>().Play();
    }

}
