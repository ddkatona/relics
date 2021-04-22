using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Move;

public class MoveSuggestor : MonoBehaviour, AIRequester {

    // References
    public Board board;
    public AIRequester requester;

    // Finding best move
    public List<Action> actionsToTry = new List<Action>();
    private ActionPath bestActionPath; // So far
    private List<ActionPath> actionPathLookup = new List<ActionPath>();

    public int alpha; // Every score has to be lower than Alpha, othwerwise the Action can't improve the score
    public bool pruningOn;
    public bool pruneThisRound;

    // State
    public bool started = false;
    public int depth;
    public int maxDepth;
    public int ID;
    public int maxID;
    public int startedSubtasks;
    public static int subtaskLimit;

    // Info
    public int finished;
    public int branches;
    public int tested;
    public int all;

    // Update is called once per frame
    void Update() {
        if (!started) return;

        if (actionsToTry.Count > 0) {
            float fps = 1 / Time.deltaTime;
            subtaskLimit = fps > 70 ? 2 : 1;
            if (startedSubtasks > subtaskLimit) return;

            TryNextMove();
        } else if (startedSubtasks == 0) {
            // All child scenarios + untested(delegated - tested)
            if (requester == null) { Debug.Log("AI Error", gameObject); Debug.Break(); }
            int allScenarios = all + (actionPathLookup.Count - maxID);
            requester.AICallback(bestActionPath, this, tested, allScenarios);
            Destroy(board.gameObject);
        }
    }

    public void TryNextMove() {
        Action action = actionsToTry[0];
        actionsToTry.RemoveAt(0);
        Board createdBoard = Instantiate(board);
        int actionScore = createdBoard.game.currentPlayer.GetScoreIncreaseForRegularPlayer(action);
        actionPathLookup[maxID] = new ActionPath(action, actionScore, GetPlayerID(), depth, board.game.round);

        // Branching
        MoveSuggestor branch = GetMoveSuggestorFor(createdBoard);
        int alphaDown = bestActionPath.CalculateAlpha(createdBoard.game.currentPlayer.ID);
        branch.StartProcessing(this, maxDepth, depth, ID: maxID, alpha: alphaDown);
        maxID++;
        startedSubtasks++;
    }

    // Clones BoardState and removes graphics
    public MoveSuggestor CloneBoard() {
        Board createdBoard = Instantiate(board);
        createdBoard.RemoveGraphics();
        MoveSuggestor moveSuggestor = createdBoard.game.currentPlayer.GetComponent<MoveSuggestor>();
        return moveSuggestor;
    }

    // Initializes and Starts branch
    public void StartProcessing(AIRequester originalRequester, int maxDepth, int depth = -1, int ID = 0, int alpha = 10000) {
        Initialize();

        requester = originalRequester;
        this.maxDepth = maxDepth;
        this.depth = depth + 1;
        this.ID = ID;
        this.alpha = alpha;
        pruneThisRound = originalRequester.GetPlayerID() != GetPlayerID() ? true : false;

        board.name = "Board:" + this.depth + " [" + ID + "]";

        // If maxDepth reached
        if (this.depth == this.maxDepth) {
            requester.AICallback(ActionPath.SingleInvalidPass, this, tested: 1, all: 1);
            Destroy(board.gameObject);
            return;
        }

        // ==> DEPTH IS NOT MAX

        // Get Actions to test
        actionsToTry = GetPreprocessedActions();
        branches = actionsToTry.Count;

        // Set arbitrary Action as initial "best" (with Score: -1000)
        bestActionPath = ActionPath.SingleInvalidPass;
        actionPathLookup = ActionPath.Allocate(size: actionsToTry.Count);
    }

    public void Initialize() {
        // Disable other Suggerstor (it's active due to cloning)
        GetMoveSuggestorFor(board, opposing: true).started = false;

        // Initializig variables (they could not be zero due to cloning)
        started = true;
        startedSubtasks = 0;
        maxID = 0;
        tested = 0;
        all = 0;
    }

    // result: Best Action-Score pair on the called Board
    public void AICallback(ActionPath result, MoveSuggestor ms, int tested, int all) {
        ScoredAction branchBestSoFar = actionPathLookup[ms.ID].actions[0];

        ActionPath actionPathToTest = result.AppendAtFront(branchBestSoFar);
        if (pruningOn && pruneThisRound && !branchBestSoFar.Invalid) {
            if (actionPathToTest.GetValueFor(GetPlayerID()) >= alpha) {
                actionsToTry.Clear();
            }
        }
        bestActionPath = bestActionPath.GetIfBetter(actionPathToTest, GetPlayerID(), this);
        startedSubtasks--;

        // Info
        finished++;
        this.tested += tested;
        this.all += all;
    }

    #region Utilities

    public float GetProgress() {
        return (float)finished / branches;
    }

    private List<Action> GetPreprocessedActions() {
        List<Action> actions = board.GetAllLegalActions();
        HashSet<Field> threats = board.game.GetOtherPlayer(board.game.currentPlayer).SetOfThreatenedFields();
        actions = actions.FindAll(action => 
            action.IsPass() || 
            ( !action.IsPass() && !threats.Contains(action.Move.GetEnd(board)) ) 
        );

        // Captures forward
        List<Action> captures = actions.FindAll(action => action.IsCapture(board));
        if (depth >= 1) captures = captures.FindAll(action => action.Move.GetStart(board).unit.GetValue() <= action.Move.GetEnd(board).unit.GetValue());
        List<Action> steps = actions.FindAll(action => action.IsStep(board));
        if (depth >= 1) steps = steps.FindAll(step => threats.Contains(step.Move.GetStart(board)));
        actions.Clear();
        actions.AddRange(captures);
        actions.AddRange(steps);
        actions.Add(Action.Pass);

        return actions;
    }

    public MoveSuggestor GetMoveSuggestorFor(Board board, bool opposing = false) {
        Player player = board.game.currentPlayer;
        if (opposing) player = board.game.GetOtherPlayer(player);
        return player.GetComponent<MoveSuggestor>();
    }

    public int GetPlayerID() {
        return GetComponent<Player>().ID;
    }

    public bool IsDifferentPlayer(MoveSuggestor ms) {
        return GetPlayerID() != ms.GetPlayerID();
    }
    #endregion

}