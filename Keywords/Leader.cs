using System.Collections.Generic;
using UnityEngine;

public class Leader : Keyword {

    void OnEnable() {
        Host.OnUnitKilled += Effect;
    }

    void OnDisable() {
        Host.OnUnitKilled -= Effect;
    }

    private void Effect(Unit killer, Field field) {
        List<Unit> allies = Host.Board.GetAlliesOf(Host.owner);
        List<Unit> allyLeaders = allies.FindAll(ally => ally.KeywordManager.HasKeyword<Leader>());
        int numberOfAllyLeaders = allyLeaders.Count;

        //if (!Host.Board.isDummy) Debug.Log(Host.owner.playerName + " Leaders left: " + numberOfAllyLeaders);
        if (numberOfAllyLeaders == 0) {
            // If all allied Leaders are dead
            GameEndAnimation gea = Host.Board.GetAnimation<GameEndAnimation>();
            gea?.Register(loser: Host.owner);
        }
    }

}
