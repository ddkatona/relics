using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Loadout {

    public string loadoutName;

    // From Field(0) to Field(23)
    public List<int> unitIDs = new List<int>();

    // Empty with name
    public Loadout(string name) {
        loadoutName = name;
    }

    public Loadout(string name, List<int> IDs) {
        loadoutName = name;
        unitIDs = IDs;
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public void GenerateEmptyIDs() {
        for(int i = 0; i < 24; i++) {
            unitIDs.Add(0);
        }
    }

}
