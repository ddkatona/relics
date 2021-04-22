using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInitAnimation : CustomAnimation {

    public Vector3 targetPosition;
    public GameObject body;

    public Vector3 velocity;

    public override void AnimationUpdate() {
        body.transform.position = Vector3.SmoothDamp(body.transform.position, targetPosition, ref velocity, 0.3f);
        if((body.transform.position - targetPosition).sqrMagnitude < 0.0001f) {
            body.transform.position = targetPosition;
            EndAnimation();
        }
    }

}
