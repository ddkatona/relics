using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStepHopAnimation : ParametricAnimation {

    public Field origin;
    public Field target;
    public Unit host;

    // Audio
    public AudioClip knock;

    #region Subscribtions
    void OnEnable() {
        host = transform.parent.GetComponentInParent<Unit>();
        host.UnitStepped += Register;
    }
    void OnDisable() {
        host.UnitStepped -= Register;
    }
    #endregion

    public override void ParametricUpdate(float t) {
        Vector3 way = target.transform.position - origin.transform.position;
        float distance = way.magnitude;
        float travelled = t * distance;
        float height = Mathf.Sin(t * Mathf.PI) / 2f;
        host.transform.position = origin.transform.position + way.normalized * travelled + Vector3.up * height;
    }

    public override void ParametricEnd() {
        host.transform.position = target.transform.position;
        EndAnimation();
    }

    public void Register(Move move) {
        origin = move.GetStart(host.Board);
        target = move.GetEnd(host.Board);
        t = 0;
        Game.MAIN.animationManager.Add(this);
    }

    public override void EndAnimation() {
        PlayClip(knock);
        base.EndAnimation();
    }
}
