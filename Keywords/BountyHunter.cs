using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunter : Keyword {

    #region Subscribtions: BeforeCapture
    void Start() {
        Host.BeforeCapture += GainCoins;
    }

    void OnDisable() {
        Host.BeforeCapture -= GainCoins;
    }
    #endregion

    void GainCoins(Move captureMove) {
        Player owner = Host.owner;
        Unit capturedUnit = captureMove.GetEnd(owner.Board).unit;
        int plunder = capturedUnit.Cost;
        owner.Coins = Mathf.Min(owner.Coins + plunder, owner.MaxCoins);
        //Debug.Log("Plunder: " + plunder);
    }

}
