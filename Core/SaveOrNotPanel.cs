using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveOrNotPanel : MonoBehaviour {

    public TMP_InputField loadoutNameInput;
    public TextMeshProUGUI prompt;
    public FileManager fileManager;
    public LoadoutEditor loadoutEditor;
    public CanvasGroup cg;

    public void TrySave() {
        if (loadoutEditor.GetUnitCount() > 14) {
            InfoText.Show("Loadouts may only have 16 Units");
            return;
        }
        if (loadoutEditor.GetLeaderCount() < 1) {
            InfoText.Show("Loadouts must contain at least 1 Leader");
            return;
        }
        string loadoutName = loadoutNameInput.text;
        if (fileManager.LoadoutWithNameExistsInFolder(loadoutName, "DefaultLoadouts")) {
            InfoText.Show($"Can't use {loadoutName} as a name");
            return;
        }

        if (fileManager.LoadoutWithNameExistsInFolder(loadoutName, "Loadouts")) {
            Show();
        } else {
            loadoutEditor.Save();
        }
    }

    public void Show() {
        prompt.SetText($"Loadout {loadoutNameInput.text} already exists. Do you want to override it?");
        cg.alpha = 1;
        cg.blocksRaycasts = true;
    }
    
    public void Hide() {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
    }

    public void Yes() {
        loadoutEditor.Save();
    }

    public void No() {
        Hide();
    }

}
