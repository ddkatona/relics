using UnityEngine;

public class Move {

    private int startID;
    private int endID;

    public Move(Field start, Field end) {
        startID = start.ID;
        endID = end.ID;
    }  

    public Field GetStart() {
        return GetStart(Board.MAIN);
    }

    public Field GetStart(Board board) {
        return board.fields[startID];
    }

    public Field GetEnd() {
        return GetEnd(Board.MAIN);
    }

    public Field GetEnd(Board board) {
        return board.fields[endID];
    }

    public Vec2 GetOffset() {
        return GetEnd().ToVec2().Minus(GetStart().ToVec2());
    }

    public override string ToString() {
        return "[" + startID + "] --> [" + endID + "]";
    }

    public string GetPrettyString() {
        Field start = GetStart();
        Field end = GetEnd();
        return $"{start.unit.unitName} {start.GetName()} to {end.GetName()}";
    }

    public string DetailedString() {
        Field end = GetEnd();
        string st = GetStart().unit.unitName;
        string en = end.IsEmpty() ? "Empty" : end.unit.unitName;
        return st + "(" + startID + ") --> " + en + "(" + endID + ")";
    }

    public MoveType GetMoveType(Board board) {
        Field starField = GetStart(board);
        Field endField = GetEnd(board);
        if (endField.IsEmpty()) return MoveType.Step;
        if (!starField.IsEmpty()) {
            if(starField.unit.owner != endField.unit.owner) return MoveType.Capture;
            else return MoveType.Support;
        } else {
            return MoveType.Invalid;
        }
    }

    public Color GetMoveTypeColor(MoveType moveType) {
        switch (moveType) {
            case MoveType.Step:     return Color.yellow;
            case MoveType.Capture:  return Color.green;
            case MoveType.Support:  return Color.cyan;
            case MoveType.Invalid:  return Color.red;
            default:                return Color.blue;
        }
    }

    public Color GetColor() {
        return GetColor(Board.MAIN);
    }

    public Color GetColor(Board board) {
        MoveType moveType = GetMoveType(board);
        return GetMoveTypeColor(moveType);
    }

    public Validity CheckValidity(Board board) {
        MoveType type = GetMoveType(board);
        if (type == MoveType.Step) return new Validity();
        if (type == MoveType.Invalid) return new Validity();
        if (type == MoveType.Support) {
            Unit supportUnit = GetStart(board).unit;
            Unit supportedUnit = GetEnd(board).unit;
            if (supportedUnit.IsAllyOf(supportUnit)) return new Validity();
            else new Validity($"{supportedUnit.unitName} is an enemy, you can't support it");
        }
        // == Capture ==
        Unit attackingUnit = GetStart(board).unit;
        Unit attackedUnit = GetEnd(board).unit;

        // ANY -> SUPERIOR ==> invalid
        if (attackedUnit.HasKeyword<Superior>()) {
            int moreExpensive = attackedUnit.Cost - attackingUnit.Cost;
            if (moreExpensive > 0)
                return new Validity($"{attackedUnit.unitName} is Superior, costs {moreExpensive} more");
        }
        // NON-SUPPORT -> ALLY ==> invalid
        if (attackedUnit.owner == attackingUnit.owner)
            return new Validity($"{attackingUnit.unitName} is not Support");
        // ANY -> INVINCIBLE ==> invalid
        if (attackedUnit.HasKeyword<Invincible>())
            return new Validity($"{attackedUnit.unitName} is Invincible");
        // !(FLYING or RANGED) -> FLYING ==> invalid
        if(attackedUnit.HasKeyword<Flying>())
        if (!(attackingUnit.HasKeyword<Flying>() || attackingUnit.HasKeyword<Ranged>()) 
            && attackedUnit.HasKeyword<Flying>())
            return new Validity($"{attackedUnit.unitName} is Flying");

        return new Validity();
    }

}

public enum MoveType { Step, Capture, Support, Invalid };

