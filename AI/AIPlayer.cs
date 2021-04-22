using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIPlayer : Player {

    public override void Update() {
        base.Update();
    }

    public override void AICallback(ActionPath result, MoveSuggestor ms, int tested, int all) {
        base.AICallback(result, ms, tested, all);
        MakeAction(result.actions[0].Action);
    }

    public override void TurnStart(Player commingPlayer) {
        base.TurnStart(commingPlayer);
        if(!Board.IsCopied() && this == commingPlayer) StartAI();
    }

}
