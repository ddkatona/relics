using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    // Singleton
    public static Game MAIN;

    [Header("Manual")]
    public Player regularPlayer;
    public Player oppositePlayer;
    public Player startingPlayer;
    public Player currentPlayer;
    public Board board;
    public AnimationManager animationManager;
    public PassStone passStone;

    // State
    public int round;
    public int consecutivePasses;

    #region Events
    public delegate void PlayerDelegate(Player player);
    public PlayerDelegate TurnStarted;
    public PlayerDelegate TurnEnded;

    public delegate void IntegerDelegate(int round);
    public IntegerDelegate RoundEnded;
    public IntegerDelegate AfterRoundEnded;
    public IntegerDelegate RoundStarted;
    #endregion

    #region Subscribtions
    void OnEnable() {
        animationManager.AnimationsCompleted += OnAnimationsFinished;
        animationManager.OnAnimationEnded += UpdateHasteGraphics;
    }

    void OnDisable() {
        animationManager.AnimationsCompleted -= OnAnimationsFinished;
        animationManager.OnAnimationEnded -= UpdateHasteGraphics;
    }
    #endregion

    public void Initialize() {
        MAIN = this;
        board.Initialize();
        regularPlayer.Initialize();
        oppositePlayer.Initialize();
        board.PopulateBoard(regularPlayer, oppositePlayer, random: regularPlayer.loadout == null);
    }

    public void RemoveGraphics() {
        // Remove Pass Stone
        PassStone passStone = GetComponentInChildren<PassStone>();
        Destroy(passStone.gameObject);

        // Remove Player graphics
        regularPlayer.RemoveGraphics();
        oppositePlayer.RemoveGraphics();
    }

    #region Turn & Round progression
    // Called at the end of every Turn EXCEPT when it's and end of a Round
    public void EndTurn() {
        TurnEnded?.Invoke(currentPlayer);
        if (board.isDummy) {
            NextTurn(); // Skip animations if Dummy
            return;
        }

        RegisterEndAnimation(roundEnd: false);
        animationManager.StartAnimations();        
    }

    public void NextTurn() {
        currentPlayer = GetOtherPlayer(currentPlayer);
        TurnStarted?.Invoke(currentPlayer);
    }

    public void EndRound() {
        Player oldPlayer = currentPlayer;
        currentPlayer = null;

        TurnEnded?.Invoke(oldPlayer);
        RoundEnded?.Invoke(round);

        if (board.isDummy) {
            NextRound(); // Skip animations if dummy
            return;
        }

        RegisterEndAnimation(roundEnd: true);
        AfterRoundEnded?.Invoke(round); // Use for Round Start Effects
        animationManager.StartAnimations();
    }

    public void NextRound() {
        round++;
        currentPlayer = GetPlayerForRound(round);
        consecutivePasses = 0;

        RoundStarted?.Invoke(round);
        TurnStarted?.Invoke(currentPlayer);
    }

    public void OnAnimationsFinished() {
        if (consecutivePasses >= 2) {
            NextRound();
        } else {
            NextTurn();
        }
    }

    public void RegisterEndAnimation(bool roundEnd, float startSpeed = 0, bool parallel = true) {
        PassTurnStartAnimation pass = passStone.GetAnimation<PassTurnStartAnimation>();
        if (roundEnd) {
            pass.Init(GetPlayerForRound(round + 1));
            animationManager.Add(pass);

            if(parallel)
                animationManager.AddToLastInParallel(board.GetAnimation<RoundEndAnimation>());
            else
                animationManager.Add(board.GetAnimation<RoundEndAnimation>());
        } else {
            pass.Init(GetOtherPlayer(currentPlayer), startSpeed);
            animationManager.Add(pass);
        }

        Player nextPlayer = roundEnd ? GetPlayerForRound(round + 1) : GetOtherPlayer(currentPlayer);
        RotateBoardAnimation rba = board.GetAnimation<RotateBoardAnimation>();
        rba.Init(nextPlayer);
        animationManager.AddToLastInParallel(rba);
    }
    #endregion

    #region Players
    public Player GetRandomPlayer() {
        if (UnityEngine.Random.Range(0, 2) > 0) return regularPlayer;
        else return oppositePlayer;
    }

    public Player GetPlayerByID(int ID) {
        if (regularPlayer.ID == ID) return regularPlayer;
        if (oppositePlayer.ID == ID) return oppositePlayer;
        return null;
    }

    public Player GetOtherPlayer(Player player) {
        if (player == null) return startingPlayer;
        return GetPlayerByID((player.ID + 1) % 2);
    }

    public Player GetPlayerForRound(int round) {
        return round % 2 == 0 ? GetOtherPlayer(startingPlayer) : startingPlayer;
    }

    public Player SetRandomStartingPlayer(int seed) {
        UnityEngine.Random.InitState(seed);
        return startingPlayer = GetRandomPlayer();
    }
    #endregion

    private void UpdateHasteGraphics() {
        if(round > 0)
            board.GetUnits().ForEach(unit => unit.UnitGraphics.RefreshGraphics());
    }

    #region Game End
    public void PlayerWon(Player player) {
        if (board.isDummy) return;

        // Go to EndGame Screen
        EndGameInfo.winnerName = player.playerName;
        SceneManager.LoadScene("EndGame");
    }

    public void PlayerLost(Player player) {
        PlayerWon(GetOtherPlayer(player));
    }
    #endregion

}
