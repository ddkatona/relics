using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Keyword {

    #region Subscribtions: OnCapture
    void OnEnable() {
        Host.UnitCapture += CaptureBehaviour;
    }

    void OnDisable() {
        Host.UnitCapture -= CaptureBehaviour;
    }
    #endregion

    private void CaptureBehaviour(Move move) {
        Field targetField = move.GetEnd(Host.Board);
        Unit targetUnit = targetField.unit;

        // Change Fields
        Host.field.unit = null;
        targetField.unit = Host;
        Host.field = targetField;

        UnitCaptureAnimation uca = Host.GetAnimation<UnitCaptureAnimation>();
        if (Host.board.IsCopied() && uca != null) Debug.Break(); 
        uca?.Init(move, targetUnit);
        uca?.Register();

        targetUnit.Kill(Host);
    }

}
