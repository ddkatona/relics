using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewer : MonoBehaviour {

    [Header("Manual")]
    public Transform main;
    public Transform info;
    public Vector3 showPosition;
    public Vector3 showSize;

    [Header("State")]
    public bool show;
    public bool showAdditionalInfo;

    [Header("Refereneces")]
    public Unit unit;
    public KeywordInfoPanel keywordInfoPanel;
    //public Board descriptionBoard;
    //public ConstraintLegendManager constraintLegendManager;
    //public EditorUnitList editorUnitList;
    //public CanvasGroup scoreBoard;

    // For Zoom movement
    private Vector3 positionVelocity = Vector3.zero;
    private Vector3 scaleVelocity = Vector3.zero;

    // For Opening movement
    private Vector3 topVelocity = Vector3.zero;
    private Vector3 infoVelocity = Vector3.zero;

    void Start() {
        keywordInfoPanel = GameObject.Find("Canvas/KeywordPanel").GetComponent<KeywordInfoPanel>();
        //descriptionBoard = GameObject.Find("DescriptionBoard").GetComponent<Board>();
        //constraintLegendManager = GameObject.Find("Canvas/UnitAdditionalInfo/MovesLegend").GetComponent<ConstraintLegendManager>();
        //editorUnitList = GameObject.Find("UnitList")?.GetComponent<EditorUnitList>();
        //scoreBoard = GameObject.Find("Canvas/LeaderCountPanel")?.GetComponent<CanvasGroup>();
    }

    void Update() {
        // Zoom & Opening animation
        if (show) {
            // Zoom movement
            transform.position = Vector3.SmoothDamp(transform.position, showPosition, ref positionVelocity, 0.1f);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, showSize, ref scaleVelocity, 0.1f);

            // Open movement
            main.transform.position = Vector3.SmoothDamp(main.transform.position, transform.position + Vector3.forward * 1.75f, ref topVelocity, 0.2f);
            info.transform.position = Vector3.SmoothDamp(info.transform.position, transform.position + Vector3.forward * -0.45f, ref infoVelocity, 0.2f);

            Vector3 mousePosition = Input.mousePosition;
            //Debug.Log(mousePosition);
            if(mousePosition.x > 1225) {
                ShowAdditional();
            } else {
                HideAdditional();
            }
        }
    }

    public void Show(Unit unit) {
        this.unit = unit;
        //GetAudioSource().PlayScheduled(0.15f);
        show = true;        
    }

    public void Hide() {
        HideAdditional();
        //GetAudioSource().Stop();
        Destroy(gameObject);
    }

    public AudioSource GetAudioSource() {
        return unit.GetComponentInChildren<Animations>().GetComponent<AudioSource>();
    }

    public void ShowAdditional() {
        if (showAdditionalInfo) return;
        showAdditionalInfo = true;
        keywordInfoPanel.SetVisibility(true);
        keywordInfoPanel.DisplayKeywords(unit.KeywordManager.keywords);
        /*
        keywordInfoPanel.SetVisibility(true);
        keywordInfoPanel.DisplayKeywords(unit.KeywordManager.keywords);
        showAdditionalInfo = true;

        // Place Unit
        Field middleField = descriptionBoard.GetField(3, 3);
        descriptionBoard.InstantiateUnit(unit.prefab, middleField, descriptionBoard.game.regularPlayer);
        Unit displayedUnit = middleField.unit;
        
        HashSet<OptionField> options = displayedUnit.GetOptionsByRule();
        // Legend Update
        constraintLegendManager.DisplayConstraintInfos(GetUniqueConstraints(options));

        // Highlight Fields
        foreach (OptionField optionField in options)
            optionField.field.HighlightOption(optionField);
        SwapInDesriptionBoard();
        editorUnitList?.Clear();

        if (scoreBoard != null) scoreBoard.alpha = 0;*/
    }

    public void HideAdditional() {
        if (!showAdditionalInfo) return;
        showAdditionalInfo = false;
        keywordInfoPanel.SetVisibility(false);
        /*
        keywordInfoPanel.SetVisibility(false);
        showAdditionalInfo = false;

        // Destroy Field Highlight
        Unit displayedUnit = descriptionBoard.GetField(3, 3).unit;
        if (displayedUnit != null) {
            HashSet<OptionField> options = displayedUnit.GetOptionsByRule();
            foreach (OptionField optionField in options)
                optionField.field.Unhighlight();
            Destroy(displayedUnit.gameObject);
        }
        SwapInMainBoard();
        editorUnitList?.DisplayUnitsFrom(editorUnitList.start);

        if(scoreBoard != null) scoreBoard.alpha = 1;*/
    }

    /*
    public void SwapInDesriptionBoard() {
        Board mainBoard = unit.Board;
        mainBoard.RelocateFieldsAndUnits(new Vector3(0f,0f,8f));
        descriptionBoard.RelocateFieldsAndUnits(new Vector3(-1.6f, 0.1f, 0.5f));
    }

    public void SwapInMainBoard() {
        Board mainBoard = unit.Board;
        mainBoard.RelocateFieldsAndUnits(new Vector3(0f,0f,0f));
        descriptionBoard.RelocateFieldsAndUnits(new Vector3(0f, 0f, 8f));
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
                types.Add(constraintType, types.Count == 0 ? "A" : "B");
                ret.Add(constraint);
            }
            constraint.letter = types[constraintType];
        }
        return ret;
    }
    */

}
