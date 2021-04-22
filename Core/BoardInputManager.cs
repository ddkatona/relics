using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardInputManager : MonoBehaviour {

    [Header("Manual")]
    public Board board;

    // References
    public SmartRay ray;
    public DragHighlighter drag;

    // State
    public Unit unitSelected;
    public Validity unitValidity;
    public Player player;

    // Draging
    public List<Unit> unitsWithMoves = new List<Unit>();

    public delegate void UnitDelegate(Unit unit);
    public UnitDelegate OnUnitSelected;
    public UnitDelegate OnUnitDeselected;

    #region Subscribtions
    void OnEnable() {
        board.game.TurnStarted += OnTurnStart;
        board.game.TurnEnded += OnTurnEnd;
    }

    void OnDisable() {
        board.game.TurnStarted -= OnTurnStart;
        board.game.TurnEnded -= OnTurnEnd;
    }
    #endregion

    private void Start() {
        ray = Camera.main.GetComponent<SmartRay>();
    }

    // Update is called once per frame
    void Update() {
        // Turn Actions
        if (!player.HasAction() || board.game.animationManager.InProgress()) return;      
        UnitOptionsFlow();
        PassOptions();
    }

    #region TurnOptions
    private void UnitOptionsFlow() {
        Field fieldHit;

        // 1) Selecting
        if (fieldHit = ray.FieldWithUnitSelector(MouseAction.Down)) {  // Button Down
            unitSelected = fieldHit.unit;
            unitValidity = CheckUnitValidity(unitSelected);
            if (unitValidity.Valid) {
                HighlightUnits(player, false);
                Invoke("HighlightLegalMoves", 0.0f);
                drag.SetArrowStart(fieldHit);
                OnUnitSelected?.Invoke(unitSelected);
            }
            return;
        }

        // 2) Holding -> Only for Drag animation
        if (fieldHit = ray.Selector<Field>(MouseAction.Held)) {
            if (unitSelected == null) return;
            if (!unitValidity.Valid) return;
            drag.SetArrowTarget(fieldHit);
            return;
        } else if(Input.GetMouseButton(0)) {
            drag.Pause();
            return;
        }

        // 3) Releasing
        if (Input.GetMouseButtonUp(0) && unitSelected != null) {
            UnhighlightLegalMoves();
        }
        if (fieldHit = ray.Selector<Field>(MouseAction.Up)) {
            if (fieldHit != null && unitSelected != null) {
                Action action = new Action(unitSelected.field, fieldHit);
                if (unitValidity.Valid && IsMoveLegal(unitSelected, fieldHit)) {
                    // Making the actual Move
                    player.MakeAction(action);
                } else if(fieldHit != unitSelected.field) {
                    // Error message printing
                    Validity worseValidity = unitValidity.GetIfWorse(action.Move.CheckValidity(board));
                    if (worseValidity.PrintIfInvalid())
                        InfoText.Show(unitSelected.unitName + " can't move like that");
                    HighlightUnits(player, true);
                } else {
                    HighlightUnits(player, true);
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            drag.Stop();
            OnUnitDeselected?.Invoke(unitSelected);
            unitSelected = null;
            unitValidity = null;
            if(fieldHit == null) HighlightUnits(player, true);
        }
    }

    public Validity CheckUnitValidity(Unit unit) {
        if (!unit.OwnedBy(player)) {
            return new Validity($"{unit.unitName} is an enemy");
        } else if (!unit.CheapEnoughToMove()) {
            return new Validity($"{unit.unitName} costs {unit.Cost} gold");
        } else if (!player.HasAction()) {
            return new Validity($"Not your turn");
        } else if (!unit.hasHaste) {
            return new Validity($"{unit.unitName} has no Haste");
        } else if (unit.GetMoves().Count == 0) {
            return new Validity($"{unit.unitName} has no possible moves");
        }
        return new Validity();
    }

    private void PassOptions() {
        // Pass button is selected via the Buttons
        if (Input.GetKeyDown(KeyCode.Space)) {
            player.MakeAction(new Action());
        }

        // Pass button is selected via the Cursor
        PassStone passStone;
        if (passStone = ray.Selector<PassStone>(MouseAction.Down)) {
            if (board.game.currentPlayer == player && !GetComponentInParent<AnimationManager>().InProgress())
                player.MakeAction(new Action());
        }
    }
    #endregion

    #region Move Highlight
    private void HighlightLegalMoves() {
        if (unitSelected == null) return;
        List<Move> moves = unitSelected.GetMoves();
        moves.ForEach(item => 
            item.GetEnd().HighlightMove(item)
        );
    }

    private void UnhighlightLegalMoves() {
        if (unitSelected == null) return;
        List<Move> moves = unitSelected.GetMoves();
        moves.ForEach(item => 
            item.GetEnd().Unhighlight()
        );
    }

    private bool IsMoveLegal(Unit unit, Field field) {
        List<Move> moves = unit.GetMoves();
        List<Field> legalFields = moves.ConvertAll(item =>
            item.GetEnd()
        );
        if (!legalFields.Contains(field)) return false;
        return true;
    }
    #endregion

    #region Unit Highlighter
    private void OnTurnStart(Player player) {
        HighlightUnits(player, on: true);
    }

    private void OnTurnEnd(Player player) {
        HighlightUnits(player, on: false);
    }

    private List<Unit> GetUnitsWithMoves(Player player) {
        if (player != this.player) return new List<Unit>();
        List<Unit> units = board.GetAlliesOf(player);
        return units.FindAll(unit => unit.GetMoves().Count > 0);
    }

    private void HighlightUnits(Player player, bool on) {
        List<Unit> units = GetUnitsWithMoves(player);
        units.ForEach(unit => unit.UnitGraphics.unitMarker.Switch(on, 0));
    }
    #endregion

}
