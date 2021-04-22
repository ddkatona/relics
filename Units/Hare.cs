using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hare : Unit {

    #region Subscription
    private void OnEnable() {
        UnitStepped += SummonHare;
        AfterCapture += SummonHare;
    }

    private void OnDisable() {
        UnitStepped -= SummonHare;
        AfterCapture -= SummonHare;
    }
    #endregion

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetOption(0, 1, MultiOption.Step));
        options.UnionWith(GetOption(0, -1, MultiOption.Step));
        options.UnionWith(GetOption(1, 0, MultiOption.Step));
        options.UnionWith(GetOption(-1, 0, MultiOption.Step));

        HashSet<OptionField> jumps = new HashSet<OptionField>();
        jumps.UnionWith(GetOption(0, 2, MultiOption.StepCapture));
        jumps.UnionWith(GetOption(0, -2, MultiOption.StepCapture));
        jumps.UnionWith(GetOption(2, 0, MultiOption.StepCapture));
        jumps.UnionWith(GetOption(-2, 0, MultiOption.StepCapture));
        foreach(OptionField jump in jumps) {
            UnitHopConstraint ahc = new UnitHopConstraint(jump, this);
            jump.AddConstraint(ahc);
        }
        options.UnionWith(jumps);

        return options;
    }

    private void SummonHare(Move move) {
        if (move.GetOffset().Legnth != 2) return;
        GameObject hareGo = Instantiate(prefab);
        hareGo.transform.parent = transform.parent;
        Hare hare = hareGo.GetComponent<Hare>();
        hare.Initialize(owner, Board, prefab);
        hare.Summon(move.GetStart(Board));
    }

}
