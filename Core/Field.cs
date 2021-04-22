using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour {

    [Header("Manual")]
    public TextMeshPro infoText;
    public Highlight highlight;
    public GameObject arrowPrefab;
    public OptionGraphics optionGraphics;

    public List<GameObject> createdStuff;

    public int ID;
    public Board board;
    public Unit unit;

    public Highlight Highlight => GetComponentInChildren<Highlight>();

    public void Initialize(Board board, int ID) {
        this.board = board;
        this.ID = ID;
        infoText?.SetText(ID.ToString());
    }

    public bool IsEmpty() {
        return unit == null;
    }

    public Field GetOffsetField(Player player, int x, int y) {
        return board.GetFieldWithOffset(this, player, new Vec2(x,y));
    }

    public int X => ID % board.width;
    public int Y => ID / board.height;

    public Vec2 ToVec2() {
        return new Vec2(X, Y);
    }

    public string GetName() {
        char letter = 'A';
        char number = '1';
        letter += (char)X;
        number += (char)Y;
        return letter.ToString() + number.ToString();
    }

    public List<Field> GetAdjacentFields() {
        List<Field> adjacentFields = new List<Field>() {
            GetOffsetField(board.game.regularPlayer, 0,1),
            GetOffsetField(board.game.regularPlayer,0,-1),
            GetOffsetField(board.game.regularPlayer,1,0),
            GetOffsetField(board.game.regularPlayer,-1,0)
        };
        return adjacentFields.FindAll(field => field != null);
    }

    #region Graphics
    public void HighlightMove(Move move) {
        if (IsEmpty()) {
            Highlight.Activate(move);
            Unit steppingUnit = move.GetStart().unit;
            optionGraphics.ShowToDisplay(Option.Step, steppingUnit);
        } else {
            if (move.GetMoveType(Board.MAIN) == MoveType.Capture) {
                Unit steppingUnit = move.GetStart().unit;
                bool melee = steppingUnit.KeywordManager.HasKeyword<Melee>();
                unit.UnitGraphics.unitMarker.Switch(true, melee ? 1 : 2);
            }
            if (move.GetMoveType(Board.MAIN) == MoveType.Support) {
                unit.UnitGraphics.unitMarker.Switch(true, 3);
            }            
        }            
    }

    public void HighlightOption(OptionField optionField) {
        Highlight.Activate(optionField);
        if (optionField.constraint != null) {
            // Arrow 
            if (optionField.constraint is StepParentConstraint) {
                GameObject arrowGO = Instantiate(arrowPrefab, transform);
                createdStuff.Add(arrowGO);
                Vector3 from = ((StepParentConstraint)optionField.constraint).parent.field.transform.position;
                Vector3 to = transform.position;
                Vector3 midPoint = (from + to) / 2;
                arrowGO.transform.position = midPoint;
                arrowGO.transform.LookAt(to, Vector3.up);
                arrowGO.transform.position += Vector3.Cross((to - from).normalized, Vector3.up) * 0.25f;
                arrowGO.transform.position += Vector3.up * 0.1f;

                arrowGO.transform.localScale /= 3f;
            } else {
                infoText.gameObject.SetActive(true);
                infoText.SetText(optionField.constraint.letter);
            }
        }
        optionGraphics.AddToDisplay(optionField);
    }

    public void Unhighlight() {
        if (IsEmpty())
            Highlight.Deactivate();
        else
            unit.UnitGraphics.unitMarker.Switch(false);

        infoText.gameObject.SetActive(false);
        createdStuff.ForEach(stuff => Destroy(stuff));
        createdStuff.Clear();
        optionGraphics.Destroy();
    }

    public void RemoveGraphics() {
        foreach (Transform tmp in transform) {
            Destroy(tmp.gameObject);
        }
    }
    #endregion

}
