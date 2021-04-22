using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTurnStartAnimation : CustomAnimation {

    // References
    public Board board;
    public PassStone passStone;

    // Properties
    public float targetAngle;
    float way;

    public float velocity;

    public override void AnimationUpdate() {
        SetEulerY(Mathf.SmoothDamp(passStone.transform.eulerAngles.y, targetAngle, ref velocity, 0.2f));

        if (Mathf.Abs(passStone.transform.eulerAngles.y - targetAngle) < 0.1f) {
            SetEulerY(targetAngle);
            EndAnimation();
        }
    }

    public void Init(Player nextPlayer, float startVelocity = 0) {
        float originAngle = passStone.transform.eulerAngles.y;
        velocity = startVelocity;
        targetAngle = nextPlayer.regularFacing ?
            (Mathf.Abs(originAngle) < 1f ? 0f : 360f) :
            180f;
        way = targetAngle - originAngle;
    }

    public void SetEulerY(float y) {
        Vector3 tmp = passStone.transform.eulerAngles;
        tmp.y = y;
        passStone.transform.eulerAngles = tmp;
    }

}
