using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAnimation : CustomAnimation, IAnimationManager {

    public List<CustomAnimation> animations = new List<CustomAnimation>();
    public int animationsPlaying;

    public override void AnimationUpdate() {
        if (animationsPlaying == 0) EndAnimation();
    }

    public void Build(List<CustomAnimation> animations) {
        this.animations = animations;
        destroyAfter = true;
    }

    public void Build(CustomAnimation a1, CustomAnimation a2) {
        animations = new List<CustomAnimation> { a1, a2 };
        destroyAfter = true;
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        // Start all animations
        animations.ForEach(animation => animation.StartAnimation(this));
        animationsPlaying = animations.Count;
    }

    public void AnimationCallback() {
        animationsPlaying--;
    }

    public override void DestroyIfNeeded() {
        foreach(CustomAnimation ca in animations)
            ca.DestroyIfNeeded();
        if (destroyAfter) Destroy(gameObject);
    }

    public override string ToString() {
        string res = "";
        animations.ForEach(animation => res += animation.ToString() + "    ");
        return res;
    }

}
