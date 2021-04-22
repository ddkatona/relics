using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HumanPlayer : Player {

    public override void Update() {
        base.Update();
        if(Input.GetKeyDown(KeyCode.A)) {
            if (game.currentPlayer == this)
                StartAI();
            else
                Debug.Log("It's not your turn!");
        }
    }

    public override void AICallback(ActionPath result, MoveSuggestor ms, int tested, int all) {
        base.AICallback(result, ms, tested, all);         
    }
}
