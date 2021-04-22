using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStepAnimation : ParametricAnimation {

    // References
    public Unit host;

    // Properties
    public Field origin;
    public Field target;
    public Vector3 way;

    // Audio
    public AudioClip step;
    
    #region Subscribtions
    void OnEnable() {
        host = transform.parent.GetComponentInParent<Unit>();
        host.UnitStepped += QueueAnimation;
    }
    void OnDisable() {
        host.UnitStepped -= QueueAnimation;
    }
    #endregion

    public override void ParametricUpdate(float t) {  
        Vector3 travelled = t * way;
        host.transform.position = origin.transform.position + travelled;
    }

    public override void ParametricEnd() {
        host.transform.position = target.transform.position;
        EndAnimation();
    }

    // Enqueues self to Animate
    public void QueueAnimation(Move move) {
        origin = move.GetStart(host.Board);
        target = move.GetEnd(host.Board);
        way = target.transform.position - origin.transform.position;
        Register();
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        PlayClip(step);
    }

    public override void EndAnimation() {
        base.EndAnimation();
    }

}
