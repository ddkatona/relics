using System;
using System.Collections.Generic;

public enum Option { Step, Capture, Support }

public class MultiOption {
    public bool step;
    public bool capture;
    public bool support;

    public MultiOption(bool step, bool capture, bool support = false) {
        this.step = step;
        this.capture = capture;
        this.support = support;
    }

    public static MultiOption StepCapture => new MultiOption(true, true);
    public static MultiOption Step => new MultiOption(true, false);
    public static MultiOption Capture => new MultiOption(false, true);
    public static MultiOption Support => new MultiOption(false, false, true);
}

public class OptionField {

    public Field field;
    public Unit unit;
    public Option option;

    public Constraint constraint;

    public OptionField(Field field, Unit unit, Option option) {
        this.field = field;
        this.unit = unit;
        this.option = option;
    }

    public Vec2 GetOffset() {
        return field.ToVec2().Minus(unit.field.ToVec2());
    }

    public void AddConstraint(Constraint constraint) {
        this.constraint = constraint;
    }

    public bool IsOrigin() {
        return unit.field == field;
    }

    public bool MoveTypeSatisfied() {
        if (StepConditionSatisfied()) return true;
        if (CaptureConditionSatisfied()) return true;
        if (SupportConditionSatisfied()) return true;
        return false;
    }

    public bool StepConditionSatisfied() {
        return option == Option.Step && field.IsEmpty();
    }

    public bool CaptureConditionSatisfied() {
        return option == Option.Capture && !field.IsEmpty() && !field.unit.IsAllyOf(unit);
    }

    public bool SupportConditionSatisfied() {
        return option == Option.Support && !field.IsEmpty() && field.unit.IsAllyOf(unit);
    }

    public bool ConstraintSatisfied() {
        if (constraint == null) return true;
        return constraint.Satisfied();
    }

}