using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : Keyword {

    #region Subscribtions: OnUnitStep
    void Start() {
        Host.OnUnitStep += StepBehaviour;
    }

    void OnDisable() {
        Host.OnUnitStep -= StepBehaviour;
    }
    #endregion

    private void StepBehaviour(Move move) {
        Field targetField = move.GetEnd(Host.Board);

        // Change Fields
        Host.field.unit = null;
        targetField.unit = Host;
        Host.field = targetField;
    }

}
