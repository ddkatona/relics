using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBoardAnimation : ParametricAnimation {

    // References
    public Board board;
    public List<Unit> units = new List<Unit>();

    // Properties
    public float originAngle;
    public float targetAngle;
    float way;

    public override void ParametricUpdate(float t) {
        foreach(Unit unit in units) {
            float travelled = t * way;
            SetEulerZ(unit.transform, originAngle + travelled);
        }
    }

    public override void ParametricEnd() {
        foreach (Unit unit in units) {
            SetEulerZ(unit.transform, targetAngle);
        }
        EndAnimation();
    }

    public void Init(Player nextPlayer) {
        units = board.GetUnits();
        originAngle = units[0].transform.eulerAngles.z;
        float regularZ = Mathf.Abs(originAngle) < 1f ? 0f : 360f;
        float oppositeZ = 180f;
        targetAngle = nextPlayer.regularFacing ? regularZ : oppositeZ;
        if (!Settings.rotateBoard) targetAngle = regularZ;
        
        way = targetAngle - originAngle;
    }

    public void SetEulerZ(Transform tr, float z) {
        Vector3 tmp = tr.eulerAngles;
        tmp.z = z;
        tr.eulerAngles = tmp;
    }

}
