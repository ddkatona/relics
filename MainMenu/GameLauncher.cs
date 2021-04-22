using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher: MonoBehaviour {

    [Header("Manual")]
    public LoadoutListManager loadoutListManager;

    public void LoadPvP() {
        GameLaunchInfo.loadout_0 = loadoutListManager.GetSelectedLoadout();
        GameLaunchInfo.loadout_1 = loadoutListManager.GetAILoadout();
        GameLaunchInfo.aiGame = false;
        SceneManager.LoadScene("PvPScene");
    }

    public void LoadPvE() {
        GameLaunchInfo.loadout_0 = loadoutListManager.GetSelectedLoadout();
        GameLaunchInfo.loadout_1 = loadoutListManager.GetAILoadout();
        GameLaunchInfo.aiGame = true;
        SceneManager.LoadScene("PvPScene");
    }

}
