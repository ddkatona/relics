using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisoned : Keyword {

    #region Subscribtions: OnRoundEnd
    void Start() {
        Host.Game.RoundEnded += Die;
    }

    void OnDisable() {
        Host.Game.RoundEnded -= Die;
    }
    #endregion

    private void Die(int round) {
        Host.Kill(Host);
    }

}
