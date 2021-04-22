using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private Move move;
    private bool pass;

    public Action(Move move) {
        this.move = move;
        pass = false;
    }

    public Action(Field start, Field end) {
        move = new Move(start, end);
        pass = false;
    }

    public Action() {
        pass = true;
    }

    public static Action Pass => new Action();
    public Move Move => move;

    public bool IsPass() {
        return pass;
    }

    public bool IsCapture(Board board) {
        return !pass && move.GetMoveType(board) == MoveType.Capture;
    }

    public bool IsStep(Board board) {
        return !pass && move.GetMoveType(board) == MoveType.Step;
    }

    public string GetPrettyString() {
        return pass ? "Passed" : move.GetPrettyString();
    }

    public string DetailedString() {
        if (pass) return "Pass";
        else return move.DetailedString();
    }

}
