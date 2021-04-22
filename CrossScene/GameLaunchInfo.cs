using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLaunchInfo {

    public static bool aiGame;

    public static Loadout loadout_0;
    public static Loadout loadout_1;

    public static Loadout GetLoadoutForID(int ID) {
        if (ID == 0) return loadout_0;
        if (ID == 1) return loadout_1;
        return null;
    }

    public static bool TestGame => loadout_0 == null || loadout_1 == null;

}
