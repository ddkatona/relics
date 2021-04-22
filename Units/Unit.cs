using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Move;

public abstract class Unit : GraphicalObject {

    [Header("Manual")]
    public int ID;
    public string unitName;
    public Texture image;
    public int cost;
    [TextArea(5, 5)]
    public string cardText;
    public bool classic;

    [Header("References")]
    public Field field;
    public Player owner;
    public Board board;
    public GameObject prefab;

    [Header("State")]
    public bool hasHaste;

    public Board Board => board;
    public Game Game => Board.game;
    public KeywordManager KeywordManager => GetComponentInChildren<KeywordManager>();
    public int Cost {
        get => cost;
        set {
            cost = value;
            if (cost < 0) cost = 0;
        }
    }
    public UnitGraphics UnitGraphics => GetComponentInChildren<UnitGraphics>();

    #region Events
    public delegate void MoveDelegate(Move move);
    public MoveDelegate BeforeCapture;
    public MoveDelegate UnitCapture;
    public MoveDelegate AfterCapture;
    public MoveDelegate OnUnitStep;
    public MoveDelegate UnitStepped;
    public MoveDelegate OnSupport;
    public delegate void EmptyDelegate();
    public EmptyDelegate UnitMoved;
    public EmptyDelegate UnitSummoned;
    public delegate void UnitDelegate(Unit unit, Field field);
    public UnitDelegate OnUnitKilled;
    #endregion

    #region Subscribtions
    void OnDisable() {
        Game.RoundStarted -= RoundStart;
    }
    #endregion

    #region Initialize
    public void Initialize(Player owner, Board board, GameObject prefab) {
        this.owner = owner;
        this.board = board;
        this.prefab = prefab;
        Game.RoundStarted += RoundStart;

        if (!board.IsCopied()) {
            // Instantiate Graphics
            GameObject unitGraphicsPrefab = Resources.Load<GameObject>("UnitGraphics");
            GameObject unitGraphicsGO = Instantiate(unitGraphicsPrefab, transform);
            unitGraphicsGO.GetComponent<UnitGraphics>().Initialize(this);
        } else {
            RemoveAnimations();
        }

        KeywordManager.RegisterDefaultKeywords();
    }
    #endregion

    public override bool IsCopied() {
        return Board != Board.MAIN;
    }

    public bool HasKeyword<T>() {
        return KeywordManager.HasKeyword<T>();
    }

    public bool OwnedBy(Player player) {
        return player == owner;
    }

    public bool IsAllyOf(Unit other) {
        return other.owner == owner;
    }

    #region Basic behaviors
    // Linking an unlinked Unit to a Field
    public bool Summon(Field f, bool direct = true) {
        if (field != null) {
            Debug.Log("Unable to spawn Unit: Field isn't empty!");
            return false;
        }

        if(!f.IsEmpty()) {
            if (!direct) return false;
            List<Field> adjacentFields = f.GetAdjacentFields();
            foreach(Field adjField in adjacentFields) {
                bool success = Summon(adjField, direct: false);
                if (success) return true;
            }
            return false;
        }

        // Set Field
        field = f;
        f.unit = this;

        // Update position
        transform.position = field.transform.position;

        UnitSummoned?.Invoke();
        return true;
    }

    // Changing the Field of the Unit
    public virtual void Move(Move move) {
        Field targetField = move.GetEnd(Board);
        MoveType moveType = move.GetMoveType(Board);

        hasHaste = false;
        owner.Coins -= Cost;

        switch (moveType) {
            case MoveType.Step:
                //Step(move);
                OnUnitStep?.Invoke(move);
                UnitStepped?.Invoke(move);
                break;
            case MoveType.Capture:
                BeforeCapture?.Invoke(move);
                UnitCapture?.Invoke(move);
                AfterCapture?.Invoke(move);
                break;
            case MoveType.Support:
                OnSupport?.Invoke(move);
                break;
            case MoveType.Invalid:
                Debug.Log("Wrong call to Move Unit. Summoning...");
                Summon(targetField);
                break;
            default: break;
        }
        UnitMoved?.Invoke();
    }

    // Removing unit from the board
    public void Kill(Unit killer) {
        Field tmp = field;
        if (field.unit == this)
            field.unit = null;
        field = null;
        
        owner.Board.AddToGraveyard(this);
        GetAnimation<DeathAnimation>()?.Register();
        OnUnitKilled?.Invoke(killer, tmp);
        owner.OnAllyKilled?.Invoke(this);
    }

    public void RoundStart(int round) {
        hasHaste = true;
    }
    #endregion

    #region Moves
    // Functions to describe possible moves

    public bool CheapEnoughToMove() {
        return cost <= owner.Coins;
    }

    public List<Move> GetMoves(bool current = true) {
        if (current && (!CheapEnoughToMove() || !hasHaste)) return new List<Move>();
        List<Field> fields = new List<Field>(GetFields());
        List<Move> moves = fields.ConvertAll(f => new Move(field, f));

        // Filter illegal Captures
        moves = moves.FindAll(move => move.CheckValidity(Board).Valid);
        return moves;
    }

    public HashSet<Field> GetThreatenedFields() {
        HashSet<Field> fields = GetFields();
        fields.RemoveWhere(field => !field.IsEmpty());
        return fields;
    }

    public HashSet<Field> GetFields() {
        return FilterOptions(GetOptionsByRule());
    }

    public abstract HashSet<OptionField> GetOptionsByRule();

    public HashSet<OptionField> GetMirrored(int x, int y, MultiOption mOption, bool H = false, bool V = false) {
        HashSet<OptionField> ret = new HashSet<OptionField>();
        ret.UnionWith(GetOption(x, y, mOption));
        if (H) ret.UnionWith(GetOption(-x, y, mOption));
        if (V) ret.UnionWith(GetOption(x, -y, mOption));
        if(H & V) ret.UnionWith(GetOption(-x, -y, mOption));
        return ret;
    }

    public HashSet<OptionField> GetOption(int x, int y, MultiOption mOption) {
        HashSet<OptionField> ret = new HashSet<OptionField>();
        OptionField optionField = null;
        if(mOption.step) {
            optionField = GetOptionSingle(x, y, Option.Step);
            if (optionField != null) ret.Add(optionField);
        }
        if (mOption.capture) {
            optionField = GetOptionSingle(x, y, Option.Capture);
            if (optionField != null) ret.Add(optionField);
        }
        if (mOption.support) {
            optionField = GetOptionSingle(x, y, Option.Support);
            if (optionField != null) ret.Add(optionField);
        }
        return ret;
    }

    public OptionField GetOptionSingle(int x, int y, Option option = Option.Step) {
        Field f = field.GetOffsetField(owner, x, y);
        return f == null ? null : new OptionField(f, this, option);
    }

    public HashSet<OptionField> GetStepCaptureArray(int x, int y, int maxSteps = 8) {
        HashSet<OptionField> ret = new HashSet<OptionField>();
        Field runningField = field;
        OptionField optionStep = null;
        OptionField optionCapture = null;
        while (ret.Count < maxSteps*2) {
            runningField = runningField.GetOffsetField(owner, x, y);
            if (runningField != null) {
                StepParentConstraint spc = new StepParentConstraint(optionStep);
                optionStep = new OptionField(runningField, this, Option.Step);
                optionCapture = new OptionField(runningField, this, Option.Capture);
                if (spc.parent != null) {                    
                    optionStep.AddConstraint(spc);
                    optionCapture.AddConstraint(spc);
                }               
                ret.Add(optionStep);
                ret.Add(optionCapture);
            } else {
                break;
            }
        }
        return ret;
    }

    public HashSet<Field> FilterOptions(HashSet<OptionField> options) {
        HashSet<Field> fields = new HashSet<Field>();
        foreach (OptionField option in options) {
            if (option.MoveTypeSatisfied() && option.ConstraintSatisfied()) {
                fields.Add(option.field);
            }
        }
        return fields;
    }

    public HashSet<OptionField> GetOptionsInRange(int lowest, int highest, Option option) {
        HashSet<OptionField> options = new HashSet<OptionField>();
        for (int x = -4; x <= 4; x++) {
            for (int y = -4; y <= 4; y++) {
                int rangeToCheck = Mathf.Abs(x) + Mathf.Abs(y);
                if (lowest <= rangeToCheck && rangeToCheck <= highest) {
                    OptionField optionField = GetOptionSingle(x, y, option);
                    if (optionField != null) options.Add(optionField);
                }
            }
        }
        return options;
    }

    public HashSet<OptionField> GetOptionsInRange(int lowest, int highest, MultiOption mOption) {
        HashSet<OptionField> options = new HashSet<OptionField>();
        if (mOption.step) {
            options.UnionWith(GetOptionsInRange(lowest, highest, Option.Step));
        }
        if (mOption.capture) {
            options.UnionWith(GetOptionsInRange(lowest, highest, Option.Capture));
        }
        if (mOption.support) {
            options.UnionWith(GetOptionsInRange(lowest, highest, Option.Support));
        }
        return options;
    }

    public HashSet<OptionField> GetGlobalOptions(Option option) {
        List<Field> fields = Board.fields.FindAll(field => field != this.field);
        List<OptionField> options = fields.ConvertAll(field => new OptionField(field, this, option));
        return new HashSet<OptionField>(options);
    }
    #endregion

    #region AI help
    public int GetValue() {
        int value = Cost;
        if (KeywordManager.HasKeyword<Leader>()) value += 5;
        return value;
    }

    public void RemoveGraphics() {
        UnitGraphics unitGraphics = GetComponentInChildren<UnitGraphics>();
        Destroy(unitGraphics.gameObject);
    }
    #endregion

}