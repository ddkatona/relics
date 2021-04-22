using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : Keyword {

    #region Subscribtions: OnRoundEnd
    void Start() {
        Host.Game.RoundEnded += RemoveKeyword;
    }

    void OnDisable() {
        Host.Game.RoundEnded -= RemoveKeyword;
    }
    #endregion

    private void RemoveKeyword(int round) {
        Host.KeywordManager.RemoveKeyword(this);
    }

}
