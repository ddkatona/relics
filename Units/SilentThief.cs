using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentThief : Unit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetGlobalOptions(Option.Step));
        foreach(OptionField of in options) {
            AdjacentAllyConstraint aac = new AdjacentAllyConstraint(of, this);
            of.AddConstraint(aac);
        }

        options.UnionWith(GetOptionsInRange(1, 1, Option.Capture));
        return options;
    }
    
}

public class AdjacentAllyConstraint : Constraint {

    public OptionField option;
    public Unit unit;

    public AdjacentAllyConstraint(OptionField option, Unit unit) {
        this.option = option;
        this.unit = unit;
        description = "If there is an adjacent ally";
    }

    public override bool Satisfied() {
        Field target = option.field;
        List<Field> adjacentFields = target.GetAdjacentFields();
        List<Field> adjacentFieldsWithAllies = adjacentFields.FindAll(
            field => !field.IsEmpty() && field.unit != unit && field.unit.IsAllyOf(unit)
        );
        return adjacentFieldsWithAllies.Count > 0;
    }
}
