using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndAnimation : CustomAnimation {

    Player loser;

    public void Register(Player loser) {
        this.loser = loser;
        Register();
    }

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        Invoke("EndAnimation", 0.5f);
    }

    public override void EndAnimation() {
        loser.game.PlayerLost(loser);
        base.EndAnimation();        
    }

}
