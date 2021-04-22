using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostChangeAnimation : CustomAnimation {

    public Unit targetUnit;
    public AudioClip chime;

    public void Init(Unit targetUnit) {
        this.targetUnit = targetUnit;
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        targetUnit.UnitGraphics.costGraphics.Refresh();
        PlayClip(chime);
        Invoke("EndAnimation", 1f);
    }

    public override void EndAnimation() {
        base.EndAnimation();
    }

}
