using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SText: MonoBehaviour {

    public static Dictionary<string, Color> colorDict = new Dictionary<string, Color>();
    //public List<ColorBatch> miscBatches = new List<ColorBatch>();

    private List<string> generalWords = new List<string>() {
        "grant",
        "kill",
        "haste",
        "summon",
        "death",
        "die"
    };

    private List<string> actionWords = new List<string>() {
        
    };

    public void OnEnable() {
        if (colorDict.Count > 0) return;
        // Add Keywords
        List<GameObject> keywordGOs = new List<GameObject>(Resources.LoadAll<GameObject>("Keywords"));
        List<Keyword> keywords = keywordGOs.ConvertAll(go => go.GetComponent<Keyword>());
        foreach(Keyword keyword in keywords) {
            colorDict.Add(keyword.keywordName.ToLower(), keyword.color);
        }

        // Add Yellow
        foreach(string word in generalWords) {
            colorDict.Add(word, new Color(255f/255f, 217f/255f, 0f, 1f));
        }

        // Add Blue
        foreach (string word in actionWords) {
            colorDict.Add(word, new Color(0 / 255f, 187/255f, 1f, 1f));
        }

        // Double for all cases
        Dictionary<string, Color> tmp = new Dictionary<string, Color>();
        foreach(KeyValuePair<string, Color> pair in colorDict) {
            tmp.Add(char.ToUpper(pair.Key[0]) + pair.Key.Substring(1), pair.Value);
        }
        foreach (KeyValuePair<string, Color> pair in tmp) {
            colorDict.Add(pair.Key, pair.Value);
        }
    }

    public static string Icon(string name) {
        return $"<sprite=\"{name}\" index=0> {name}";
    }

    public static string Format(string text) {
        foreach (KeyValuePair<string, Color> pair in colorDict) {
            string colorHex = "#" + ColorUtility.ToHtmlStringRGB(pair.Value);
            text = text.Replace(pair.Key, $"<color={colorHex}>{pair.Key}</color>");
        }
        return text;
    }

}
