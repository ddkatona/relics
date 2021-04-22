using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorUnitSelector : MonoBehaviour {

    public Unit unitHeld;
    public SmartRay ray;
    public Camera cam;
    public CanvasGroup deleteZone;

    public delegate void EmptyDelegate();
    public EmptyDelegate OnUnitsUpdated;

    void Update(){
        UnitUnderlay unitUnderlay;
        Field field;

        // Mouse Down
        if ((unitUnderlay = ray.Selector<UnitUnderlay>(MouseAction.Down)) != null) {
            Unit unitPrototype = unitUnderlay.transform.GetComponentInChildren<Unit>();
            GameObject unitGO = Instantiate(unitPrototype.gameObject);
            unitHeld = unitGO.GetComponent<Unit>();
            unitHeld.transform.localScale = new Vector3(1,1,1);
        }        
        if ((field = ray.Selector<Field>(MouseAction.Down)) != null && !field.IsEmpty()) {
            unitHeld = field.unit;
            deleteZone.alpha = 1;
        }

        // Mouse Held
        if(Input.GetMouseButton(0) && unitHeld != null) {
            RaycastHit hit = ray.GetRaycastHit();
            Vector3 dir = (cam.transform.position - hit.point).normalized;
            unitHeld.transform.position = hit.point + dir * 5f;
        }

        // Mouse Up
        if((field = ray.Selector<Field>(MouseAction.Up)) && unitHeld) {
            if (field.ID < 21) {
                if (CreatingUnit(unitHeld)) {
                    CreateUnitOn(field);
                } else {
                    MoveUnitTo(field);
                }
            } else {
                DestroyHeldUnit();
            }
            unitHeld = null;
            deleteZone.alpha = 0;
        } else if(Input.GetMouseButtonUp(0)) {
            if (unitHeld != null) {
                if (CreatingUnit(unitHeld))
                    DestroyHeldUnit();
                else
                    unitHeld.transform.position = unitHeld.field.transform.position;
                unitHeld = null;
                deleteZone.alpha = 0;
            }            
        }
    }

    private void MoveUnitTo(Field field) {
        if(field.IsEmpty()) {
            // Move
            Field oldField = unitHeld.field;
            unitHeld.field = null;
            unitHeld.Summon(field);
            oldField.unit = null;
        } else {
            // Switch
            Unit otherUnit = field.unit;
            Field tmpField = unitHeld.field;

            unitHeld.field = otherUnit.field;
            unitHeld.transform.position = otherUnit.transform.position;
            otherUnit.field.unit = unitHeld;

            otherUnit.field = tmpField;
            otherUnit.transform.position = tmpField.transform.position;
            tmpField.unit = otherUnit;
        }
        OnUnitsUpdated?.Invoke();
    }

    private void CreateUnitOn(Field field) {
        if(field.IsEmpty()) {
            // Create
            unitHeld.Summon(field);
        } else {
            // Destroy
            Destroy(unitHeld.gameObject);
        }
        OnUnitsUpdated?.Invoke();
    }

    private bool CreatingUnit(Unit unit) {
        return unit.field == null;
    }

    private void DestroyHeldUnit() {
        if (unitHeld == null) return;
        if (unitHeld.field != null) unitHeld.field.unit = null;
        Destroy(unitHeld.gameObject);
        OnUnitsUpdated?.Invoke();
    }

}
