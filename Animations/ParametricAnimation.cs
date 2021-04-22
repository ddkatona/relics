using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametricAnimation : CustomAnimation {

    public float duration;
    public float t = 0;

    public override void AnimationUpdate() {
        base.AnimationUpdate();
        if (t < 1) {
            ParametricUpdate(t);
            t += Time.deltaTime / duration;
        } else if (t < 2) {
            t = 3;
            ParametricEnd();
        }
    }

    public virtual void ParametricUpdate(float t_) {}

    public virtual void ParametricEnd() {}

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        t = 0;
    }

}
