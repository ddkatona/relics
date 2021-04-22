using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalObject : MonoBehaviour {

    public T GetAnimation<T>() {
        Animations a = GetComponentInChildren<Animations>();
        if (a == null) {
            return default;
        }
        T ret = a.GetComponentInChildren<T>();
        if (ret == null && !IsCopied()) Debug.LogWarning(typeof(T) + " is missing from " + this);
        return ret;
    }

    public void MuteAnimations() {
        Transform animations = transform.Find("Animations");
        foreach(Transform animation in animations) {
            animation.GetComponent<CustomAnimation>().Mute();
        }
        Destroy(animations.GetComponent<AudioSource>());
    }

    public void RemoveAnimations() {
        Animations a = GetComponentInChildren<Animations>();
        if (a == null) return;
        foreach (Transform animaitonTransform in a.transform) {
            Destroy(animaitonTransform.gameObject);
        }
    }

    public virtual bool IsCopied() {
        return false;
    }

}
