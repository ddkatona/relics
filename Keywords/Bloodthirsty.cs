using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodthirsty : Keyword {

    #region Subscribtions: OnUnitCapture
    void Start() {
        Host.UnitCapture += ResetHaste;
    }

    void OnDisable() {
        Host.UnitCapture -= ResetHaste;
    }
    #endregion

    private void ResetHaste(Move move) {
        Host.hasHaste = true;
    }

}
