using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewSelector : MonoBehaviour {

    [Header("Manual")]
    public RuleBoard ruleBoard;
    public SmartRay ray;

    [Header("State")]
    public UnitViewer unitViewer;
    public float clickTime;
    private Unit unitSelected;

    void Update(){
        CheckUnitView();
    }

    private void CheckUnitView() {
        Field fieldHit;
        if (fieldHit = ray.FieldWithUnitSelector(MouseAction.Down)) {
            unitSelected = fieldHit.unit;
            Invoke("ShowUnitView", 0.0f);
            clickTime = Time.time;
        } else if (Input.GetMouseButtonUp(0)) {
            if (Time.time - clickTime < 0.1f && unitSelected != null)
                ruleBoard.Initialize(unitSelected);
            HideUnitView();
        }
    }

    private void ShowUnitView() {
        if (unitSelected == null) return;
        UnitGraphics unitGraphics = unitSelected.UnitGraphics;
        unitGraphics.unitMarker.Switch(false);
        GameObject graphics = unitGraphics.gameObject;
        unitViewer = Instantiate(
            graphics,
            graphics.transform.position,
            graphics.transform.rotation
        ).GetComponent<UnitViewer>();
        unitViewer.Show(unitSelected);
    }

    private void HideUnitView() {
        unitViewer?.Hide();
        unitViewer = null;
        unitSelected = null;
    }

}
