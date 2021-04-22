using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : Keyword {

    #region Subscribtions: OnUnitCapture
    void Start() {
        Host.UnitStepped += ResetHaste;
    }

    void OnDisable() {
        Host.UnitStepped -= ResetHaste;
    }
    #endregion

    private void ResetHaste(Move move) {
        Host.hasHaste = true;
    }

}
