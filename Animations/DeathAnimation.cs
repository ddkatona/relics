using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : CustomAnimation {

    public Unit host;
    public ParticleSystem puff;

    // Audio
    public AudioClip death;

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        host = transform.parent.GetComponentInParent<Unit>();
        puff.transform.position += Vector3.up * 0.2f;
        puff.Play();
        PlayClip(death);
        Invoke("RemoveVisuals", 0.2f);
        Invoke("EndAnimation", 1.5f);
    }

    public void RemoveVisuals() {
        host.RemoveGraphics();
    }

    public override void EndAnimation() {
        base.EndAnimation();
        host.transform.position -= Vector3.up;
        host.MuteAnimations();
        host.RemoveAnimations();
        host.gameObject.SetActive(false);
    }    

}
