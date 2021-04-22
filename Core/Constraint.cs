using System.Collections.Generic;

public abstract class Constraint {

    protected string description;
    public string letter;

    public abstract bool Satisfied();

    public virtual string GetDescription() {
        return description;
    }

}

public class MultiConstraint : Constraint {

    public List<Constraint> constraints = new List<Constraint>();

    public MultiConstraint(List<Constraint> constraints) {
        this.constraints = constraints;
    }

    public override bool Satisfied() {
        foreach(Constraint constraint in constraints)
            if (!constraint.Satisfied()) return false;
        return true;
    }

    public override string GetDescription() {
        string appended = "";
        foreach (Constraint constraint in constraints)
            appended += constraint.GetDescription() + "\n";
        return appended;
    }
}

public class StepParentConstraint : Constraint {

    public OptionField parent;

    public StepParentConstraint(OptionField parent) {
        this.parent = parent;
        description = "If unit can step on the field before";
    }

    public override bool Satisfied() {
        if (parent.IsOrigin()) return true;
        return parent.StepConditionSatisfied() && parent.ConstraintSatisfied();
    }

}

public class UnitHopConstraint : Constraint {

    public OptionField option;
    public Unit unit;

    public UnitHopConstraint(OptionField option, Unit unit) {
        this.option = option;
        this.unit = unit;
        description = "If there is a unit in between";
    }

    public override bool Satisfied() {
        Field source = unit.field;
        Field target = option.field;
        Vec2 way = target.ToVec2().Minus(source.ToVec2());
        if (!way.DivisibleBy(2)) return false;
        way.DivideBy(2);
        Field midField = source.board.GetField(source.ToVec2().Plus(way));
        if (midField.IsEmpty()) return false;
        //if (!midField.unit.IsAllyOf(unit)) return false;
        return true;
    }
}

public class PawnDoubleStepConstraint : Constraint {

    public OptionField option;
    public Pawn pawn;

    public PawnDoubleStepConstraint(OptionField option, Pawn pawn) {
        this.option = option;
        this.pawn = pawn;
        description = "If Pawn hasn't moved yet";
    }

    public override bool Satisfied() {
        return !pawn.moved;
    }
}