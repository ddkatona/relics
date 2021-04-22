using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeywordBar : MonoBehaviour {

    [Header("Manual")]
    public KeywordPivot specialPivot;
    public KeywordPivot basicPivot;
    public SpriteRenderer crownSprite;

    // References
    public Unit unit;

    #region Subscribtions
    void OnDisable() {
        if(unit != null)
            unit.KeywordManager.OnKeywordsChange -= ChangeKeywords;
    }
    #endregion

    public void Initialize(Unit unit) {
        this.unit = unit;
        unit.KeywordManager.OnKeywordsChange += ChangeKeywords;
    }

    private void ChangeKeywords(List<Keyword> keywords) {
        List<Keyword> basicKeywords = keywords.FindAll(keyword => keyword.basic && !(keyword is Leader));
        List<Keyword> specialKeywords = keywords.FindAll(keyword => !keyword.basic && !(keyword is Leader));
        basicPivot.KeywordsChanged(basicKeywords);
        specialPivot.KeywordsChanged(specialKeywords);

        bool hasLeader = keywords.Exists(keyword => keyword is Leader);
        crownSprite.enabled = hasLeader;
    }

}

public class KeywordIcon {
    public Transform transf;
    public Keyword keyword;

    public KeywordIcon(Transform t, Keyword keyword) {
        transf = t;
        this.keyword = keyword;
    }
}