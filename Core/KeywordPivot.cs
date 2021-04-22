using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordPivot : MonoBehaviour {

    [Header("Manual")]
    public GameObject spritePrefab;

    private List<KeywordIcon> keywordIcons = new List<KeywordIcon>();

    public void KeywordsChanged(List<Keyword> newKeywords) {
        keywordIcons.ForEach(icon => Destroy(icon.transf.gameObject));
        keywordIcons = newKeywords.ConvertAll(keyword => CreateIcon(keyword));
        keywordIcons.Sort((a, b) => (int)Mathf.Sign(b.keyword.importance - a.keyword.importance));
        RefreshIcons();
    }

    private void RefreshIcons() {
        int keywordCount = keywordIcons.Count;
        if (keywordCount == 0) return;
        float stepSize = 0.14f;
        float width = (keywordCount - 1) * stepSize;

        float offestIterator = 0;
        for (int i = 0; i < keywordIcons.Count; i++) {
            keywordIcons[i].transf.position = transform.position + transform.right * offestIterator;
            offestIterator += stepSize;
        }
    }

    private KeywordIcon CreateIcon(Keyword keyword) {
        GameObject iconGO = Instantiate(spritePrefab);
        SpriteRenderer R = iconGO.GetComponent<SpriteRenderer>();
        if(keyword.iconSprite != null) R.sprite = keyword.iconSprite;
        KeywordIcon icon = new KeywordIcon(iconGO.transform, keyword);
        iconGO.transform.SetParent(transform);
        return icon;
    }

}
