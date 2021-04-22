using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFlipAnimation : CustomAnimation {

    public Game game;
    public float y = 0;
    public float angularVelocity;
    public float angularAcceleration;

    public float maxAV;
    public bool skip;

    public override void AnimationUpdate() {
        angularVelocity = Mathf.SmoothDamp(angularVelocity, maxAV, ref angularAcceleration, 0.5f);
        y += angularVelocity * Time.deltaTime;
        SetEulerY(y);

        if (angularVelocity > maxAV - 1f) EndAnimation();
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        y = game.passStone.transform.eulerAngles.y;
        if (skip) Invoke("EndAnimation", 0.1f);
    }

    public override void EndAnimation() {
        base.EndAnimation();
    }

    public void SetEulerY(float y) {
        Vector3 tmp = game.passStone.transform.eulerAngles;
        tmp.y = y;
        game.passStone.transform.eulerAngles = tmp;
    }

}
