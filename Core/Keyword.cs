using System.Collections.Generic;
using UnityEngine;

public class Keyword : MonoBehaviour {

    [Header("Manual")]
    //public GameObject icon;
    public Sprite iconSprite;
    public string keywordName;
    [TextArea(5, 5)]
    public string keywordText;
    public bool basic;
    public int importance;
    public Color color;

    public Unit host;
    public Unit Host => host ?? transform.parent.parent.GetComponent<Unit>();

    public Sprite GetSprite() {
        return iconSprite;
    }

}