using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCaptureAnimation : ParametricAnimation {

    // References
    public Unit host;

    // Properties
    public Field origin;
    public Field target;
    public Unit targetUnit;
    public Vector3 way;

    // Audio
    public AudioClip sword;

    void OnEnable() {
        host = transform.parent.GetComponentInParent<Unit>();
    }

    public override void ParametricUpdate(float t) {
        Vector3 travelled = t * way;
        float height = Mathf.Sin(t * Mathf.PI) / 2f;
        host.transform.position = origin.transform.position + travelled + Vector3.up * height;
    }

    public override void ParametricEnd() {
        targetUnit.transform.position -= Vector3.up * 0.01f;
        host.transform.position = target.transform.position;
        EndAnimation();
    }

    public void Init(Move move, Unit targetUnit) {
        origin = move.GetStart(host.Board);
        target = move.GetEnd(host.Board);
        this.targetUnit = targetUnit;
        way = target.transform.position - origin.transform.position;
    }

    public override void EndAnimation() {
        PlayClip(sword);
        Camera.main.GetComponent<CameraShaker>().StartShake(0.25f);
        base.EndAnimation();
    }

}
