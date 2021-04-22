using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderCountUpdater : MonoBehaviour {

    public Game game;
    public TextMeshProUGUI regularTMP;
    public TextMeshProUGUI oppositeTMP;

    #region Subscribtions
    private void OnEnable() {
        game.TurnEnded += UpdateLeaderCounts;
        game.TurnStarted += UpdateLeaderCounts;
    }

    private void OnDisable() {
        game.TurnEnded -= UpdateLeaderCounts;
        game.TurnStarted -= UpdateLeaderCounts;
    }
    #endregion

    private void UpdateLeaderCounts(Player player) {
        Board board = game.board;
        List<Unit> regularLeaders = board.GetLeadersOf(game.regularPlayer);
        List<Unit> oppositeLeaders = board.GetLeadersOf(game.oppositePlayer);
        regularTMP.SetText(regularLeaders.Count.ToString());
        oppositeTMP.SetText(oppositeLeaders.Count.ToString());
    }

}
