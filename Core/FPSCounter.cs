using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour {

    public float t;

    void Start() {
        //Screen.SetResolution(1920, 1080, true);

        if (Application.targetFrameRate == -1) {
            if (Application.platform == RuntimePlatform.Android) {
                Application.targetFrameRate = 30;
            } else {
                Application.targetFrameRate = 60;
            }
        }        
    }

    void Update() {
        if (t > 1) {
            float fps = 1 / Time.deltaTime;
            int wholeFPS = Mathf.RoundToInt(fps);
            GetComponent<TextMeshProUGUI>().SetText(wholeFPS.ToString());
            t = 0;
        } else {
            t += Time.deltaTime * 5f;
        }
    }
    
}
