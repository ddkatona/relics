using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimation : MonoBehaviour {

    // Properties
    public string animationName;
    public IAnimationManager animationManager;
    public bool destroyAfter;

    // State
    public bool started;

    private void Update() {
        if (started) AnimationUpdate();
    }

    public virtual void AnimationUpdate() {}

    public AudioSource Audio => GetComponent<AudioSource>();

    public CustomAnimation Copy() {
        GameObject animationGO = Instantiate(gameObject);
        CustomAnimation customAnimation = animationGO.GetComponent<CustomAnimation>();
        customAnimation.destroyAfter = true;
        return customAnimation;
    }

    public virtual void DestroyIfNeeded() {
        if(destroyAfter) Destroy(gameObject);
    }

    public virtual void StartAnimation(IAnimationManager animationManager) {
        this.animationManager = animationManager;
        started = true;
    }

    public virtual void EndAnimation() {
        started = false;
        animationManager.AnimationCallback();
    }

    public void Register(bool parallel = false) {
        if(parallel)
            Game.MAIN.animationManager.AddToLastInParallel(this);
        else
            Game.MAIN.animationManager.Add(this);
    }

    public void PlayClip(AudioClip audioClip, float volume = 1f) {
        Audio.volume = 0.2f;
        Audio.clip = audioClip;
        Audio.Play();
    }

    public void Mute() {
        if(Audio != null && !Audio.isPlaying)
            Audio.enabled = false;
    }

    public override string ToString() {
        return animationName;
    }

}