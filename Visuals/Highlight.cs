using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Move;

public class Highlight : MonoBehaviour {

    [Header("Manual")]
    public Renderer R;

    public Color stepColor;
    public Color captureColor;
    public Color bothColor;
    public Color defaultColor;

    // Update is called once per frame
    void Update() {
        
    }

    public void Activate(Move move) {
        Color c = Color.yellow;
        switch (move.GetMoveType(Board.MAIN)) {
            case MoveType.Capture:
                c = captureColor;
                break;
            case MoveType.Step:
                c = stepColor;
                break;
            default:
                c = stepColor;
                break;
        }
        R.enabled = true;
        R.material.SetColor("_EmissionColor", c);
    }

    public void Activate(OptionField optionField) {
        if (!R.enabled) {
            R.enabled = true;
            R.material.SetColor("_EmissionColor", Color.gray/3);
        }        
    }

    public void Activate() {
        R.enabled = true;
        R.material.SetColor("_EmissionColor", defaultColor);
    }

    public void Deactivate() {
        R.enabled = false;
    }

}
