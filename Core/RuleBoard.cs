using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBoard : MonoBehaviour {

    public CanvasGroup cg;
    public Board main;
    public Board description;
    public ConstraintLegendManager constraintLegendManager;
    public EditorUnitList editorUnitList;
    public List<string> chars = new List<string> {"A", "B", "C", "D"};

    public void Initialize(Unit unit) {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        Unit placedUnit = PlaceUnit(unit.prefab);  
        HashSet<OptionField> options = placedUnit.GetOptionsByRule();
        constraintLegendManager.DisplayConstraintInfos(GetUniqueConstraints(options));
        foreach (OptionField optionField in options)
            optionField.field.HighlightOption(optionField);
        editorUnitList?.Hide();
        SwapInDesriptionBoard();
    }

    public void Exit() {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        Unit displayedUnit = description.GetField(3, 3).unit;
        if (displayedUnit != null) {
            HashSet<OptionField> options = displayedUnit.GetOptionsByRule();
            foreach (OptionField optionField in options)
                optionField.field.Unhighlight();
            Destroy(displayedUnit.gameObject);
        }
        editorUnitList?.Show();
        SwapInMainBoard();
    }

    public void SwapInDesriptionBoard() {
        main.RelocateFieldsAndUnits(new Vector3(0f, 0f, 8f));
        description.RelocateFieldsAndUnits(new Vector3(0f, 0f, 0f));
    }

    public void SwapInMainBoard() {
        main.RelocateFieldsAndUnits(new Vector3(0f, 0f, 0f));
        description.RelocateFieldsAndUnits(new Vector3(0f, 0f, 8f));
    }

    public Unit PlaceUnit(GameObject unitPrefab) {
        Field middleField = description.GetField(3, 3);
        description.InstantiateUnit(unitPrefab, middleField, description.game.regularPlayer);
        return middleField.unit;
    }

    public List<Constraint> GetUniqueConstraints(HashSet<OptionField> optionFields) {
        List<OptionField> optionList = new List<OptionField>(optionFields);
        optionList = optionList.FindAll(option => option.constraint != null);
        List<Constraint> constraints = optionList.ConvertAll(option => option.constraint);

        Dictionary<Type, string> types = new Dictionary<Type, string>();
        List<Constraint> ret = new List<Constraint>();
        foreach (Constraint constraint in constraints) {
            Type constraintType = constraint.GetType();
            if (!types.ContainsKey(constraintType)) {
                types.Add(constraintType, chars[types.Count]);
                ret.Add(constraint);
            }
            constraint.letter = types[constraintType];
        }
        return ret;
    }

}
