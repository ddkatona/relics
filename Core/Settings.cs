using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public static bool showing;

    // Settings
    public static bool rotateBoard;

    public CanvasGroup cg;

    public TMP_Dropdown fps_dropdown;
    public Toggle rotateUnitsToggle;
    public TextMeshPro oppositeNameText;
    public TextMeshPro oppositeGoldText;

    void Start() {
        showing = cg.interactable;
        rotateUnitsToggle.isOn = Game.MAIN.oppositePlayer is HumanPlayer;
        fps_dropdown.value = Application.targetFrameRate == 30 ? 0 : 1;
    }
    
    public void OnFPSChange() {
        int index = fps_dropdown.value;
        if (index == 0) Application.targetFrameRate = 30;
        if (index == 1) Application.targetFrameRate = 60;
        if (index == 2) Application.targetFrameRate = -1;
    }

    public void OnRotateUnitsChanged() {
        /*rotateBoard = rotateUnitsToggle.isOn;
        float y = rotateUnitsToggle.isOn ? 180f : 0f;
        Vector3 rot = new Vector3(0,y,0);
        oppositeNameText.transform.eulerAngles = rot;
        oppositeGoldText.transform.eulerAngles = rot;*/
    }

    public void OnSurrender() {
        showing = false;
        Game.MAIN.PlayerLost(Game.MAIN.regularPlayer);
    }

    public void Toggle() {
        if (showing) Hide();
        else Show();
    }

    private void Show() {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        showing = true;
    }

    private void Hide() {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        showing = false;
    }

}
