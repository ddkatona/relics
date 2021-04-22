using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndAnimation : CustomAnimation {

    public Game game;

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        int round = game.round + 1;
        MainText.Show("Round " + round, game.GetPlayerForRound(round).playerName, 2f);
        Invoke("EndAnimation", 2f);
    }

}
