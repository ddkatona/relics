using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAnimation : CustomAnimation {

    public Unit targetUnit;

    // Audio
    public AudioClip healAudio;
    public ParticleSystem healEffect;

    public void Init(Unit target) {
        targetUnit = target;
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        PlayClip(healAudio);
        healEffect.transform.position = targetUnit.transform.position;
        healEffect.Play();
        Invoke("EndAnimation", 1f);
    }

    public override void EndAnimation() {
        base.EndAnimation();
    }

}
