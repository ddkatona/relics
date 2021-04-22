using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorUnitList : MonoBehaviour {

    [Header("Manual")]
    public Board board;
    public GameObject unitUnderPrefab;
    public RectTransform unitListBackground;
    public RectTransform filterPanel;

    public int start;
    public float scale;
    public int unitNumber;
    public List<Unit> unitsDisplayed = new List<Unit>();

    public void DisplayUnitsFrom(int start) {
        Clear();
        List<GameObject> unitPrefabs = board.unitPrefabs.GetRange(start, Mathf.Min(unitNumber, board.unitPrefabs.Count-start));
        unitPrefabs.ForEach(unitPrefab => DisplayUnit(unitPrefab));
    }

    public void DisplayUnit(GameObject unitPrefab) {
        GameObject goUnder = Instantiate(unitUnderPrefab, transform);
        goUnder.transform.position += Vector3.right * unitsDisplayed.Count * scale;

        GameObject go = Instantiate(unitPrefab);
        go.transform.position = goUnder.transform.position;
        Unit unit = go.GetComponent<Unit>();
        unit.Initialize(board.game.regularPlayer, board, unitPrefab);
        go.transform.localScale = new Vector3(1f, 1f, 1f) * 1.8f;
        go.transform.parent = goUnder.transform;
        unitsDisplayed.Add(unit);        
    }

    public void Clear() {
        foreach (Unit unit in unitsDisplayed) {
            Destroy(unit.transform.parent.gameObject);
        }
        unitsDisplayed.Clear();
    }

    public void ScrollLeft() {
        start = Mathf.Max(start - unitNumber, 0);
        DisplayUnitsFrom(start);
    }

    public void ScrollRight() {
        if (start + unitNumber > board.unitPrefabs.Count) return;
        start = Mathf.Min(start + unitNumber, board.unitPrefabs.Count - 1);
        DisplayUnitsFrom(start);
    }

    public void Show() {
        transform.position += Vector3.up;
        unitListBackground.position += Vector3.up;
        filterPanel.position += Vector3.up;
    }

    public void Hide() {
        transform.position -= Vector3.up;
        unitListBackground.position -= Vector3.up;
        filterPanel.position -= Vector3.up;
    }

}
