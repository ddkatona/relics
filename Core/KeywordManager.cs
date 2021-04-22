using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordManager : MonoBehaviour {

    // References
    public List<Keyword> keywords = new List<Keyword>();

    // Prefabs
    public static List<GameObject> keywordPrefabs = new List<GameObject>();

    // Events
    public delegate void KeywordsDelegate(List<Keyword> ks);
    public KeywordsDelegate OnKeywordsChange;

    public void RegisterDefaultKeywords() {
        // Load Keyword Prefabs
        if(keywordPrefabs.Count == 0)
            keywordPrefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Keywords"));

        List<Keyword> kwds = new List<Keyword>(GetComponentsInChildren<Keyword>());
        kwds.ForEach(keyword => AddKeyword(keyword));
    }

    public void AddKeyword(Keyword keyword) {
        keywords.Add(keyword);
        OnKeywordsChange?.Invoke(keywords);
    }

    public void RemoveKeyword(Keyword keyword) {
        keywords.Remove(keyword);
        Destroy(keyword.gameObject);
        OnKeywordsChange?.Invoke(keywords);
    }

    public void GrantKeyword<T>() {
        if (HasKeyword<T>()) return;
        GameObject keywordPrefab = keywordPrefabs.Find(prefab => prefab.GetComponent<T>() != null);
        GameObject keywordGO = Instantiate(keywordPrefab, transform);
        AddKeyword(keywordGO.GetComponent<Keyword>());
    }

    public void RevokeKeyword<T>() {
        if (!HasKeyword<T>()) return;
        Keyword keyword = keywords.Find(k => k is T);
        RemoveKeyword(keyword);
    }

    public bool HasKeyword<T>() {
        return keywords.Exists(keyword => keyword is T);
    }

}
