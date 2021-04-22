using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHighlighter : MonoBehaviour {

    [Header("Manual")]
    public GameObject arrow;

    // State
    public Field source;
    public Field target;
    public Vector3 smoothTarget;
    public Vector3 smoothTargetVelocity = Vector3.zero;

    public static DragHighlighter MAIN;

    void Start(){
        MAIN = this;
    }

    void Update(){
        if(source != null && target != null) {
            smoothTarget = Vector3.SmoothDamp(smoothTarget, target.transform.position, ref smoothTargetVelocity, 0.05f);
            PlaceArrow(source.transform.position, smoothTarget);
        }
    }

    public void SetArrowStart(Field start) {
        source = start;
    }

    public void SetArrowTarget(Field target) {
        if (target == null || target == source) { this.target = null; arrow.SetActive(false); return; }
        if (this.target == target) return;
        
        arrow.SetActive(true);
        if (this.target == null) {
            smoothTarget = source.transform.position + (target.transform.position - source.transform.position)*0.001f;
            PlaceArrow(source.transform.position, smoothTarget);
            smoothTargetVelocity = Vector3.zero;
        }
        this.target = target;

        GetComponent<AudioSource>().Play();
    }

    public void PlaceArrow(Vector3 a, Vector3 b) {
        arrow.transform.position = a;
        arrow.transform.LookAt(b, Vector3.up);
        float distance = (b-a).magnitude;
        arrow.transform.localScale = new Vector3(1, distance, distance);
    }

    public void Pause() {
        arrow.SetActive(false);
    }

    public void Stop() {
        source = null;
        target = null;
        arrow.SetActive(false);
    }

}
