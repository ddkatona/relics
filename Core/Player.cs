using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, AIRequester {

    [Header("Manual")]
    public int ID;
    public string playerName;
    public bool regularFacing;
    public Color playerColor;
    public Game game;
    public TextMeshPro nameText;
    public TextMeshPro actionText;
    public TextMeshPro actionTokenText;

    public Loadout loadout;
    public MoveSuggestor ai;   

    public Board Board => game.board;

    public int coins;
    public int Coins {
        get => coins;
        set {
            OnGoldUpdated?.Invoke(value);
            coins = value;
        }
    }

    public int maxCoins;
    public int MaxCoins {
        get => maxCoins;
        set {
            OnMaxGoldUpdated?.Invoke(value);
            maxCoins = value;
        }
    }

    public delegate void IntegerDelegate(int integer);
    public IntegerDelegate OnGoldUpdated;
    public IntegerDelegate OnMaxGoldUpdated;

    public delegate void UnitDelegate(Unit unit);
    public UnitDelegate OnAllyKilled;

    public virtual void Update() {
        if(ai != null)
            AIProgressGraphics();
    }

    #region Subscribtions
    void Start() {
        game.TurnStarted += TurnStart;
        game.TurnEnded += TurnEnd;
        game.RoundStarted += RoundStart;
        game.RoundEnded += RoundEnd;
    }

    void OnDisable() {
        game.TurnStarted -= TurnStart;
        game.TurnEnded -= TurnEnd;
        game.RoundStarted -= RoundStart;
        game.RoundEnded -= RoundEnd;
    }
    #endregion

    public void Initialize() {
        loadout = GameLaunchInfo.GetLoadoutForID(ID);

        // Graphics
        Transform playerNameObject = transform.Find("PlayerName");
        // Background Panel
        Renderer panel = playerNameObject.GetComponentInChildren<Renderer>();
        Color playerColorWithAlpha = playerColor;
        playerColorWithAlpha.a = 0.5f;
        panel.material.color = playerColorWithAlpha;

        // GoldManager
        GoldManager goldManager = GetComponentInChildren<GoldManager>();
        goldManager.Initialize(this);

        nameText.SetText(playerName);
    }

    public int GetPlayerID() {
        return ID;
    }

    public bool HasAction() {
        return game.currentPlayer == this;
    }

    public Unit MostExpensiveAlly(List<Unit> excluding = null) {
        List<Unit> allies = Board.GetAlliesOf(this);
        if(excluding != null) allies.RemoveAll(unit => excluding.Contains(unit));
        allies.Sort((a, b) => a.Cost < b.Cost ? 1 : -1);
        if (allies.Count == 0) return null;
        return allies[0];
    }

    #region Game Logic
    public void MakeAction(Action action) {
        if (!Board.IsCopied())
            actionText.SetText(action.GetPrettyString());
        Board.ExecuteAction(action);
        if (!Board.IsCopied() && action.IsPass()) {
            game.passStone.Press();
        }        
    }

    public virtual void TurnStart(Player commingPlayer) {
        if (!Board.IsCopied()) {
            if(Board.game.currentPlayer == this) {
                actionText.SetText("Move or Pass");
                ColorName(Color.green);
            } else {
                ColorName(Color.white);
            }            
        }
    }

    private void TurnEnd(Player oldPlayer) {
        if (!Board.IsCopied() && this == oldPlayer) ColorName(Color.white);
    }

    public virtual void RoundStart(int round) {
        if (MaxCoins < 8) MaxCoins++;
        Coins = MaxCoins;

        if (!Board.IsCopied())
            actionTokenText.SetText(this == Board.game.currentPlayer ? "T" : "");
    }

    public virtual void RoundEnd(int round) {

    }
    #endregion

    #region AI
    // Debug
    public float aiStartTime;

    public void StartAI() {
        MoveSuggestor moveSuggestor = GetComponent<MoveSuggestor>();
        MoveSuggestor clonedSuggestor = moveSuggestor.CloneBoard();
        clonedSuggestor.StartProcessing(this, 4);
        aiStartTime = Time.time;
        ai = clonedSuggestor;
    }

    public virtual void AICallback(ActionPath result, MoveSuggestor ms, int tested, int all) {
        float duration = Time.time - aiStartTime;
        float scenariosPerSec = tested / duration;
        string performace = "[Tested " + tested + "/" + all + " scenarios in " + duration + "s (" + scenariosPerSec + "sc/s)]";
        //Debug.Log("TOTAL SCORE: " + result.GetValueFor(ID) + " || ACTION: " + result.actions[0].DetailedString());
        //Debug.Log("ActionPath: " + result.DetailedString());
        //Debug.Log(performace);
        ai = null;
        actionText.SetText(result.actions[0].Action.GetPrettyString());
    }

    public int GetScoreIncreaseForRegularPlayer(Action action) {
        int scoreBefore = Board.GetValueAdvantage();
        MakeAction(action);
        int scoreAfter = Board.GetValueAdvantage();
        int deltaScore = scoreAfter - scoreBefore;
        if (regularFacing) return deltaScore;
        else return -deltaScore;
    }

    public string AIResponseString(ScoredAction result, int tested, int all) {
        string moveInfo = "[PlayerID: " + result.PlayerID + "]";
        if (result.Action.IsPass()) {
            moveInfo += " [Pass]";
        } else {
            moveInfo += " [" + result.Action.DetailedString() + "]";
        }
        return moveInfo + " [Score: " + result.Score + "]";
    }

    public int GetActionListValue(int playerID, List<ScoredAction> sActions) {
        int sum = 0;
        foreach (ScoredAction sa in sActions) {
            if (sa.PlayerID == playerID) sum += sa.Score;
            else if(sa.PlayerID != -1) sum -= sa.Score;
        }
        return sum;
    }

    public HashSet<Field> SetOfThreatenedFields() {
        List<Unit> units = Board.GetAlliesOf(this);
        HashSet<Field> fields = new HashSet<Field>();
        units.ForEach(unit => fields.UnionWith(unit.GetThreatenedFields()));
        return fields;
    }
    #endregion

    #region Graphics
    public void ColorName(Color color) {
        nameText.color = color;
    }

    public void AIProgressGraphics() {
        actionText.SetText($"Thinking ({Mathf.RoundToInt(100 * ai.GetProgress())}%)");
    }

    public void RemoveGraphics() {
        foreach(Transform tmp in transform) {
            //if(tmp.name != "GoldManager")
                Destroy(tmp.gameObject);
        }
        // Disable BoardInputManager
        BoardInputManager bim = GetComponent<BoardInputManager>();
        if (bim != null) bim.enabled = false;
    }
    #endregion

}
