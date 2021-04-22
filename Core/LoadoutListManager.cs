using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadoutListManager: MonoBehaviour {

    [Header("Manual")]
    public GameLauncher gameLauncher;
    public FileManager fileManager;
    public TMP_Dropdown dropdown;

    public List<Loadout> loadouts = new List<Loadout>();

    public void Start() {
        RefreshOptions();
    }

    public void RefreshOptions() {
        loadouts = fileManager.GetAllLoadoutsFromFiles();
        dropdown.ClearOptions();
        dropdown.AddOptions(loadouts.ConvertAll(loadout => loadout.loadoutName));
    }

    public Loadout GetSelectedLoadout() {
        return loadouts[dropdown.value];
    }

    public Loadout GetAILoadout() {
        return loadouts.Find(loadout => loadout.loadoutName == "Combo");
    }

    public void EditSelectedLoadout() {
        Loadout loadout = GetSelectedLoadout();
        EditorLaunchInfo.loadout = loadout;
        SceneManager.LoadScene("LoadoutEditor");
    }

    public void EditNewLoadout() {
        Loadout newLoadout = new Loadout("New Loadout");
        newLoadout.GenerateEmptyIDs();
        EditorLaunchInfo.loadout = newLoadout;
        SceneManager.LoadScene("LoadoutEditor");
    }

    public void DeleteLoadout() {
        Loadout loadout = GetSelectedLoadout();
        fileManager.DeleteLoadout(loadout);
        RefreshOptions();
    }

}
