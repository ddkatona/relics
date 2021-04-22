using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour {

    [Header("Manual")]
    public Player player;
    public BoardInputManager bim;
    public TextMeshPro coinText;
    public List<SpriteRenderer> coins;

    public int gold;
    public int maxGold;

    #region Subscribtions
    /*void OnEnable() {
        player.OnGoldUpdated += GoldUpdated;
        player.OnMaxGoldUpdated += MaxGoldUpdated;

        if (bim != null) {
            bim.OnUnitSelected += HighlightCost;
            bim.OnUnitDeselected += UnhighlightCost;
        }
    }*/

    void OnDisable() {
        if(player != null) {
            player.OnGoldUpdated -= GoldUpdated;
            player.OnMaxGoldUpdated -= MaxGoldUpdated;
        }     

        if (bim != null) {
            bim.OnUnitSelected -= HighlightCost;
            bim.OnUnitDeselected -= UnhighlightCost;
        }
    }
    #endregion

    public void Initialize(Player player) {
        this.player = player;
        bim = player.GetComponent<BoardInputManager>();
        player.OnGoldUpdated += GoldUpdated;
        player.OnMaxGoldUpdated += MaxGoldUpdated;

        if (bim != null) {
            bim.OnUnitSelected += HighlightCost;
            bim.OnUnitDeselected += UnhighlightCost;
        }
    }

    private void GoldUpdated(int gold) {
        coinText.SetText(gold.ToString());
        for(int i = 0; i < coins.Count; i++) {
            if (i < gold) coins[i].color = Color.yellow;
            else coins[i].color = (Color.yellow + Color.black*2) / 3;
        }
        this.gold = gold;
    }

    private void MaxGoldUpdated(int maxGold) {
        for (int i = 0; i < coins.Count; i++) {
            if (i < maxGold) coins[i].enabled = true;
            else coins[i].enabled = false;
        }
        this.maxGold = maxGold;
    }

    private void HighlightCost(Unit unit) {
        for (int i = gold-unit.Cost; i < gold; i++) {
            coins[i].color = (Color.yellow + Color.white * 4)/5;
        }
    }

    private void UnhighlightCost(Unit unit) {
        GoldUpdated(gold);
    }

}
