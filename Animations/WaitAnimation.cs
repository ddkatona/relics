using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimation : CustomAnimation {
    
    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        Invoke("EndAnimation", 1f);
    }

}
