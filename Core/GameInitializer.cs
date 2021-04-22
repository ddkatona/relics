using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : GraphicalObject {

    public Game game;
    public Board descriptionBoard;

    public GameObject oppositePlayerGO;

    // Start is called before the first frame update
    void Start() {
        if(GameLaunchInfo.aiGame) ModifyToAI();

        game.Initialize();
        descriptionBoard.CreateFields();
        //descriptionBoard.transform.localScale *= 7f/8f;
        Player startingPlayer = game.SetRandomStartingPlayer(5);

        // Pre game animations
        AnimationManager am = game.animationManager;

        // Coin Flip
        CoinFlipAnimation cfa = GetAnimation<CoinFlipAnimation>();
        am.Add(GetAnimation<CoinFlipAnimation>());
        game.RegisterEndAnimation(roundEnd: true, startSpeed: cfa.maxAV, parallel: false);
        //am.AddToLastInParallel(GetAnimation<CameraInitAnimation>());

        am.StartAnimations();
    }

    public override bool IsCopied() {
        return false;
    }

    public void ModifyToAI() {
        HumanPlayer humanPlayer = oppositePlayerGO.GetComponent<HumanPlayer>();
        AIPlayer aiPlayer = oppositePlayerGO.AddComponent<AIPlayer>();
        aiPlayer.playerName = "AI";
        aiPlayer.ID = 1;
        aiPlayer.game = humanPlayer.game;
        aiPlayer.nameText = humanPlayer.nameText;
        aiPlayer.actionText = humanPlayer.actionText;
        aiPlayer.actionTokenText = humanPlayer.actionTokenText;

        Destroy(oppositePlayerGO.GetComponent<BoardInputManager>());
        Destroy(humanPlayer);

        game.oppositePlayer = aiPlayer;
        oppositePlayerGO.GetComponentInChildren<GoldManager>().player = aiPlayer;
    }

}
