using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collateral : Keyword {

    #region Subscribtions: AfterCapture
    void OnEnable() {
        Host.AfterCapture += AfterCaptureBehaviour;
    }

    void OnDisable() {
        Host.AfterCapture -= AfterCaptureBehaviour;
    }
    #endregion

    private void AfterCaptureBehaviour(Move move) {
        Field targetField = move.GetEnd(Host.Board);
        List<Field> adjacentFields = targetField.GetAdjacentFields();
        List<Field> adjacentFieldsWithUnits = adjacentFields.FindAll(field => !field.IsEmpty());
        List<Unit> adjacentUnits = adjacentFieldsWithUnits.ConvertAll(field => field.unit);
        List<Unit> adjacentEneimes = adjacentUnits.FindAll(unit => !unit.IsAllyOf(Host));
        adjacentEneimes.ForEach(unit => unit.Kill(Host));
    }

}
