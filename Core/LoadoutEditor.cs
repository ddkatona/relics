using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadoutEditor : MonoBehaviour {

    public Board board;
    public Board descriptionBoard;
    public FileManager fileManager;
    public EditorUnitList unitList;
    public TMP_InputField nameInput;
    public EditorUnitSelector editorUnitSelector;

    public TextMeshProUGUI unitsCountDisplay;
    public TextMeshProUGUI leaderCountDisplay;

    #region Subscribtions
    void OnEnable() {
        editorUnitSelector.OnUnitsUpdated += UpdateRequirementDisplays;
    }

    void OnDisable() {
        editorUnitSelector.OnUnitsUpdated -= UpdateRequirementDisplays;
    }
    #endregion

    void Start(){
        board.Initialize();
        board.CacheUnits(excludeClassic: false);
        descriptionBoard.CreateFields();
        //descriptionBoard.transform.localScale *= 7f / 8f;
        unitList.DisplayUnitsFrom(0);

        Loadout loadout = GetLoadoutToLoad();
        nameInput.text = loadout.loadoutName;
        board.game.regularPlayer.loadout = loadout;
        board.LoadPlayerLoadout(board.game.regularPlayer, invert: false);

        UpdateRequirementDisplays();
    }

    public void Save() {
        Loadout loadoutToSave = new Loadout(nameInput.text, GetCurrentIDs());
        fileManager.SaveLoadout(loadoutToSave, "Player");
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitWithoutSaving() {
        SceneManager.LoadScene("MainMenu");
    }

    public Loadout GetLoadoutToLoad() {
        Loadout loadout = EditorLaunchInfo.loadout;
        if (loadout == null) loadout = fileManager.GetAllLoadoutsFromFiles()[0];
        return loadout;
    }

    public List<int> GetCurrentIDs() {
        List<int> IDs = new List<int>();
        List<Field> fields = board.fields;
        for (int i = 0; i < 24; i++) {
            int unitID = 0;
            if (!fields[i].IsEmpty()) unitID = fields[i].unit.ID;
            IDs.Add(unitID);
        }
        return IDs;
    }

    private void UpdateRequirementDisplays() {
        unitsCountDisplay.SetText($"Units: {GetUnitCount()}/14");
        if (GetUnitCount() > 14) unitsCountDisplay.color = Color.red;
        else unitsCountDisplay.color = Color.white;

        leaderCountDisplay.SetText($"Leaders: {GetLeaderCount()}");
        if (GetLeaderCount() < 1) leaderCountDisplay.color = Color.red;
        else leaderCountDisplay.color = Color.white;
    }

    public List<Unit> GetUnits() {
        List<Field> fields = board.fields.GetRange(0,24);
        List<Field> nonEmptyFields = fields.FindAll(field => !field.IsEmpty());
        return nonEmptyFields.ConvertAll(field => field.unit);
    }

    public int GetUnitCount() {
        return GetUnits().Count;
    }

    public int GetLeaderCount() {
        return GetUnits().FindAll(unit => unit.HasKeyword<Leader>()).Count;
    }

}
