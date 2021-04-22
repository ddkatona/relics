using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : Keyword {

    #region Subscribtions: OnSupport
    void OnEnable() {
        Host.OnSupport += OnSupport;
    }

    void OnDisable() {
        Host.OnSupport -= OnSupport;
    }
    #endregion

    private void OnSupport(Move move) {
        ((ISupportUnit)Host).Support(move);
    }

}
