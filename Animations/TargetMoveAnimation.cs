using UnityEngine;

public class TargetMoveAnimation : CustomAnimation {

    public Vector3 targetPosition;
    public Unit host;

    public Vector3 velocity = Vector3.zero;

    // Audio
    public AudioClip whoosh;

    public override void AnimationUpdate() {
        host.transform.position = Vector3.SmoothDamp(host.transform.position, targetPosition, ref velocity, 0.2f);
        if ((host.transform.position - targetPosition).magnitude < 0.001f) {
            host.transform.position = targetPosition;
            EndAnimation();
        }
    }

    public void Init(Unit host, Vector3 target) {
        this.host = host;
        targetPosition = target;
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        PlayClip(whoosh, 0.2f);
    }

}

