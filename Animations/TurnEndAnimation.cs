using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndAnimation : CustomAnimation {

    public Board board;

    public override void StartAnimation(IAnimationManager animationManager) {
        base.StartAnimation(animationManager);
        //MainText.Show(board.GetOtherPlayer(board.currentPlayer).playerName + "'s turn");
        Invoke("EndAnimation", 0.1f);
    }

}
