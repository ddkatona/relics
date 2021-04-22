using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Keyword {

    void OnEnable() {
        host = transform.parent.GetComponentInParent<Unit>();
        host.UnitCapture += CaptureBehaviour;
    }

    void OnDisable() {
        host.UnitCapture -= CaptureBehaviour;
    }

    private void CaptureBehaviour(Move move) {
        Field targetField = move.GetEnd(host.Board);
        Unit targetUnit = targetField.unit;

        // Animation
        RangedAnimation ra = host.GetAnimation<RangedAnimation>();
        if (Host.board.IsCopied() && ra != null) Debug.Break();
        ra?.Register(move);

        targetUnit.Kill(Host);
    }

}
