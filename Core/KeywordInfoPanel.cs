using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeywordInfoPanel : MonoBehaviour {

    public List<RectTransform> abilityCards = new List<RectTransform>();
    public List<RectTransform> keywordCards = new List<RectTransform>();

    [Header("Manual")]
    public RectTransform abilityInfoCardPrefab;
    public RectTransform keywordInfoCardPrefab;    

    private void GenerateKeywordCard(Keyword keyword) {
        RectTransform prototype = keyword.basic ? abilityInfoCardPrefab : keywordInfoCardPrefab;
        List<RectTransform> list = keyword.basic ? ref abilityCards : ref keywordCards;

        RectTransform keywordCard = Instantiate(prototype, transform);
        keywordCard.transform.position = prototype.transform.position;
        keywordCard.transform.position -= Vector3.up * (keywordCard.rect.height + 15f) * list.Count;
        keywordCard.gameObject.SetActive(true);
        list.Add(keywordCard);

        // Set Sprite
        Image image = keywordCard.transform.Find("KeywordIcon").GetComponent<Image>();
        image.sprite = keyword.GetSprite();

        // Set Text
        TextMeshProUGUI keywordText = keywordCard.transform.Find("KeywordText").GetComponent<TextMeshProUGUI>();
        keywordText.text = SText.Format($"<b>{keyword.keywordName}:</b> {keyword.keywordText}");
    }

    public void DisplayKeywords(List<Keyword> keywords) {
        // Delete
        foreach (RectTransform rt in abilityCards) {
            Destroy(rt.gameObject);
        }
        foreach (RectTransform rt in keywordCards) {
            Destroy(rt.gameObject);
        }
        abilityCards.Clear();
        keywordCards.Clear();

        // Repopulate
        keywords.Sort((a, b) => a.importance < b.importance ? 1 : -1);
        foreach (Keyword keyword in keywords) {
            GenerateKeywordCard(keyword);
        }
    }

    public void SetVisibility(bool vis) {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = vis ? 1 : 0;
    }

}
