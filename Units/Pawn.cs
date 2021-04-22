using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Unit {

    public bool moved;

    void OnEnable() {
        UnitMoved += Moved;
    }

    void OnDisable() {
        UnitMoved -= Moved;
    }

    public void Moved() {
        moved = true;
    }

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();

        // Step
        OptionField singleStep = GetOptionSingle(0, 1, Option.Step);
        if (singleStep != null) {
            options.Add(singleStep);
        }
        OptionField doubleStep = GetOptionSingle(0, 2, Option.Step);
        if (singleStep != null && doubleStep != null) {
            PawnDoubleStepConstraint pdsc = new PawnDoubleStepConstraint(doubleStep, this);
            StepParentConstraint spc = new StepParentConstraint(singleStep);
            MultiConstraint multiConstraint = new MultiConstraint(new List<Constraint> { pdsc, spc });
            doubleStep.AddConstraint(multiConstraint);
            options.Add(doubleStep);
        }

        // Capture
        options.UnionWith(GetOption(-1, 1, MultiOption.Capture));
        options.UnionWith(GetOption(1, 1, MultiOption.Capture));

        return options;
    }

}
