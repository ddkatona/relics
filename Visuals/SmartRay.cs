using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartRay : MonoBehaviour {

    public Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    public RaycastHit GetRaycastHit() {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (Settings.showing || hit.transform == null) {
            return default;
        }
        return hit;
    }

    public Field FieldWithUnitSelector(MouseAction mouseAction) {
        Field field = Selector<Field>(mouseAction);
        if (field == null) return null;
        if (field.IsEmpty()) return null;
        return field;
    }

    public T Selector<T>(MouseAction mouseAction) {
        if (!IsMouseActionPresent(mouseAction)) return default;
        RaycastHit? hit = GetRaycastHit();
        Transform transform = hit?.transform;
        if (transform == null) return default;
        T objectOfType = transform.GetComponentInParent<T>();
        if (objectOfType != null) return objectOfType;
        return default;
    }


    public bool IsMouseActionPresent(MouseAction mouseAction) {
        if (Input.GetMouseButtonDown(0) && mouseAction == MouseAction.Down) return true;
        if (Input.GetMouseButton(0) && mouseAction == MouseAction.Held) return true;
        if (Input.GetMouseButtonUp(0) && mouseAction == MouseAction.Up) return true;
        return false;
    }
}

public enum MouseAction { Down, Held, Up }