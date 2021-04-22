using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAnimation : ParametricAnimation {

    public GameObject trailPrefab;

    public Unit unitSupporting;
    public Unit unitToSupport;

    public TrailRenderer trail;

    // Audio
    public AudioClip healAudio;    

    public void Init(Unit unitSupporting, Unit unitToSupport) {
        this.unitSupporting = unitSupporting;
        this.unitToSupport = unitToSupport;
    }

    public override void ParametricUpdate(float t) {
        Vector3 way = unitToSupport.transform.position - unitSupporting.transform.position;
        float distance = way.magnitude;
        float travelled = t * distance;
        float height = Mathf.Sin(t * Mathf.PI) / 2f;
        trail.transform.position = unitSupporting.transform.position + way.normalized * travelled + Vector3.up * height;
    }

    public override void ParametricEnd() {
        EndAnimation();
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        trail = Instantiate(trailPrefab, transform.parent).GetComponent<TrailRenderer>();
        PlayClip(healAudio);
    }

    public override void EndAnimation() {
        base.EndAnimation();
    }
}
