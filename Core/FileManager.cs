using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour {

    public string RootPath => $"{Application.persistentDataPath}/Loadouts";
    public string FolderPath(string folder) => $"{RootPath}/{folder}";

    void Start(){
        if (!File.Exists(FolderPath("Player"))) Directory.CreateDirectory(FolderPath("Player"));
        if (!File.Exists(FolderPath("Default"))) Directory.CreateDirectory(FolderPath("Default"));

        List<string> defaults = new List<string>(Directory.GetFiles(FolderPath("Default")));
        if (defaults.Count < 1) {
            SaveLoadout(new Loadout("Combo", new List<int> {
                17, 16, 16, 14, 16, 16, 17,
                8, 9, 10, 10, 10, 9, 8,
                0, 0, 0, 0, 0, 0, 0
            }), "Default");
            /*SaveLoadout(new Loadout("Classic", new List<int> {
                3, 1, 5, 4, 6, 5, 1, 3,
                2, 2, 2, 2, 2, 2, 2, 2,
                0, 0, 0, 0, 0, 0, 0, 0
            }), "Default");*/
        }
    }

    public bool LoadoutWithNameExistsInFolder(string loadoutName, string folder) {
        return File.Exists($"{FolderPath(folder)}/{loadoutName}.json");
    }

    public bool LoadoutWithNameExists(string loadoutName) {
        return LoadoutWithNameExistsInFolder(loadoutName, "Player") || 
            LoadoutWithNameExistsInFolder(loadoutName, "Default");
    }

    public void SaveLoadout(Loadout loadout, string folder) {
        string json = loadout.ToJson();
        string filePath = $"{FolderPath(folder)}/{loadout.loadoutName}.json";
        File.WriteAllText(filePath, json);
        Debug.Log($"Load saved: {filePath}"); 
    }

    public void DeleteLoadout(Loadout loadout) {
        string path = $"{FolderPath("Player")}/{loadout.loadoutName}.json";
        if(File.Exists(path))
            File.Delete(path);
    }

    public List<Loadout> GetLoadoutsFromFiles(string folder) {
        List<Loadout> loadouts = new List<Loadout>();
        //string path = $"{Application.persistentDataPath}/{folder}";
        foreach (string file in Directory.GetFiles(FolderPath(folder))) {
            string json = File.ReadAllText(file);
            Loadout loadout = JsonUtility.FromJson<Loadout>(json);
            loadouts.Add(loadout);
        }
        return loadouts;
    }

    public List<Loadout> GetAllLoadoutsFromFiles() {
        List<Loadout> loadouts = GetLoadoutsFromFiles("Player");
        List<Loadout> defaultLoadouts = GetLoadoutsFromFiles("Default");
        loadouts.AddRange(defaultLoadouts);
        return loadouts;
    }

}
