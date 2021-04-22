using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour, IAnimationManager {

    public GameObject multiAnimationPrefab;

    public List<CustomAnimation> animations = new List<CustomAnimation>();

    public delegate void Template();
    public Template OnAnimationEnded;
    public Template AnimationsCompleted;
    public bool verbose;

    #region Setup phase
    public void Add(CustomAnimation customAnimation) {
        animations.Add(customAnimation);
    }

    public void AddMulti(CustomAnimation a1, CustomAnimation a2) {
        GameObject multiAnimationGO = Instantiate(multiAnimationPrefab, transform);
        MultiAnimation multiAnimation = multiAnimationGO.GetComponent<MultiAnimation>();
        multiAnimation.Build(a1, a2);
        animations.Add(multiAnimation);
    }

    public void AddMulti(List<CustomAnimation> animations_) {
        if (animations_.Count == 0) return;
        GameObject multiAnimationGO = Instantiate(multiAnimationPrefab, transform);
        MultiAnimation multiAnimation = multiAnimationGO.GetComponent<MultiAnimation>();
        multiAnimation.Build(animations_);
        animations.Add(multiAnimation);
    }

    public void AddToLastInParallel(CustomAnimation a) {
        if(animations.Count == 0) {
            Add(a);
            return;
        }
        // Detaching currently Last animation
        CustomAnimation lastItem = animations[animations.Count - 1];
        animations.Remove(lastItem);

        // Constructing list of next Last parallel animations
        List<CustomAnimation> lastAnimations;
        if (lastItem is MultiAnimation) {
            lastAnimations = new List<CustomAnimation>(((MultiAnimation)lastItem).animations);
            Destroy(lastItem.gameObject);
        } else {
            lastAnimations = new List<CustomAnimation>() { lastItem };
        }
        lastAnimations.Add(a);

        // Instantiating MultiAnimation
        GameObject multiAnimationGO = Instantiate(multiAnimationPrefab, transform);
        MultiAnimation multiAnimation = multiAnimationGO.GetComponent<MultiAnimation>();
        multiAnimation.Build(lastAnimations);

        animations.Add(multiAnimation);
    }
    #endregion

    // Outside -> Start playing queued animations
    public void StartAnimations() {
        // If no more animations -> finish
        if (animations.Count == 0) {
            FinishAnimations();
        } else {
            // Play next animation
            if(verbose) Debug.Log("Animation playing: " + animations[0].ToString());
            animations[0].StartAnimation(this);
        }        
    }

    public void AnimationCallback() {
        CustomAnimation completedAnimation = animations[0];
        animations.RemoveAt(0);
        if (completedAnimation.destroyAfter) completedAnimation.DestroyIfNeeded();
        OnAnimationEnded?.Invoke();
        StartAnimations();
    }

    public void FinishAnimations() {
        animations.Clear();
        AnimationsCompleted?.Invoke();
    }

    public bool InProgress() {
        return animations.Count > 0;
    }
}
