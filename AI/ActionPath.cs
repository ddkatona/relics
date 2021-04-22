using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Move;

public class ActionPath {

    public List<ScoredAction> actions = new List<ScoredAction>();

    public bool Invalid => actions.Count == 0 || actions[0].Invalid;

    // Single Action initializer
    public ActionPath() {
        actions = new List<ScoredAction> { new ScoredAction() };
    }

    public ActionPath(Action action, int actionScore, int playerID, int depth, int round) {
        actions = new List<ScoredAction> { new ScoredAction(action, actionScore, playerID, depth, round) };
    }

    public ActionPath(ScoredAction action, ActionPath actionPath) {
        actions = new List<ScoredAction> { action };
        actions.AddRange(actionPath.actions);
    }

    public static List<ActionPath> Allocate(int size) {
        List<ActionPath> paths = new List<ActionPath>();
        for (int i = 0; i < size; i++) paths.Add(new ActionPath());
        return paths;
    }

    public static ActionPath SingleInvalidPass {    get =>
        new ActionPath();
    }

    public ActionPath AppendAtFront(ScoredAction scoredAction) {
        List<ScoredAction> tmpActions = actions;
        actions = new List<ScoredAction> { scoredAction };
        actions.AddRange(tmpActions);
        return this;
    }

    public ActionPath GetIfBetter(ActionPath other, int playerID, MoveSuggestor invoker) {
        if (Invalid) return other;
        if (other.Invalid) return this;

        int valueThis = GetValueFor(playerID);
        int valueOther = other.GetValueFor(playerID);
        if (valueThis < valueOther) return other;
        if (valueThis > valueOther) return this;

        // => Equal
        ScoredAction actionThis = actions[0];
        ScoredAction actionOther = other.actions[0];
        ScoredAction result = actionThis.GetIfBetter(actionOther, invoker);
        return result == actionThis ? this : other;
    }

    public int GetValueFor(int playerID) {
        int sum = 0;
        foreach (ScoredAction sa in actions) {
            if (sa.PlayerID == playerID) sum += sa.Score;
            else if (sa.PlayerID != -1) sum -= sa.Score;
        }
        return sum;
    }

    public int CalculateAlpha(int playerID) {
        if (actions[0].Invalid) return 1000;
        int firstScore = actions[0].Score;
        firstScore *= actions[0].PlayerID == playerID ? 1 : -1;
        int totalScore = GetValueFor(playerID);
        return totalScore - firstScore;
    }

    public string DetailedString() {
        string res = "";
        foreach (ScoredAction scoredAction in actions) {
            //if (scoredAction.PlayerID != -1)
                res += scoredAction.DetailedString() + " ==> ";
        }
        return res;
    }

}
