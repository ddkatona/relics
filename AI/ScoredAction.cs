using UnityEngine;
using static Move;

public class ScoredAction {

    private readonly Action action;
    private readonly int score;
    private readonly int playerID;
    private readonly int depth;
    private readonly bool invalid;
    private readonly int round;

    public Action Action => action;
    public int Score => score;
    public int PlayerID => playerID;
    public bool Invalid => invalid;
    public int Depth => depth;
    public int Round => round;

    // Invalid
    public ScoredAction() {
        action = new Action();
        score = -10000;
        playerID = -1;
        depth = -1;
        invalid = true;
    }

    // Full
    public ScoredAction(Action action, int score, int playerID, int depth, int round) {
        this.action = action;
        this.score = score;
        this.playerID = playerID;
        this.depth = depth;
        invalid = false;
        this.round = round;
    }

    public ScoredAction GetIfBetter(ScoredAction scoredAction, MoveSuggestor invoker) {
        if (Invalid) return scoredAction;
        if (scoredAction.Invalid) return this;

        if (Score > scoredAction.Score) {
            return this;
        } else {
            // If the Actions are identical
            if (Score == scoredAction.Score) {
                ScoredAction randomChoice = Random.Range(0, 2) == 0 ? this : scoredAction;

                // Priorities, Rules
                if (Action.IsPass())
                    return randomChoice;
                if (Action.Move.GetMoveType(invoker.board) == MoveType.Capture)
                    return this;    // Capture if possible

                return randomChoice;
            }
            return scoredAction;
        }
    }

    public string DetailedString() {
        string player = "PlayerID: " + playerID;
        string actionString = Action.DetailedString();
        string scoreString = "Score: " + Score;
        string depth = "Depth: " + Depth;
        string round = "Round: " + Round;
        return "[" + actionString + "|" + player + "|" + depth + "|" + round + "|" + scoreString + "]";
    }

}
