using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tireless : Keyword {

    #region Subscribtions: OnTurnEnd
    void Start() {
        Host.Game.TurnEnded += ResetHaste;
    }

    void OnDisable() {
        Host.Game.TurnEnded -= ResetHaste;
    }
    #endregion

    private void ResetHaste(Player player) {
        Host.hasHaste = true;
    }

}
