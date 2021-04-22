using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Unit {

    #region Subscriptions
    void Start() {
        owner.Board.game.AfterRoundEnded += RoundStartEffect;
        OnUnitKilled += DeathEffect;
    }

    void OnDisable() {
        owner.Board.game.AfterRoundEnded -= RoundStartEffect;
        OnUnitKilled += DeathEffect;
    }
    #endregion

    public override HashSet<OptionField> GetOptionsByRule() {
        return new HashSet<OptionField>();
    }

    private void RoundStartEffect(int round) {
        if (field == null) return;
        if (GetAdjacentEnemies(field).Count == 0) return;
        Kill(this);
    }

    private void DeathEffect(Unit killer, Field field) {
        List<Unit> adjacentEnemies = GetAdjacentEnemies(field);        

        List<CustomAnimation> animations = new List<CustomAnimation>();
        foreach (Unit unit in adjacentEnemies) {
            unit.KeywordManager.GrantKeyword<Poisoned>();

            // Animation
            SupportAnimation supportAnimation = (SupportAnimation)GetAnimation<SupportAnimation>()?.Copy();
            supportAnimation?.Init(this, unit);
            supportAnimation?.Register(parallel: true);
        }
    }

    public List<Unit> GetAdjacentEnemies(Field field) {
        List<Field> adjacentFieldsWithEnemies = field.GetAdjacentFields().FindAll(f => !f.IsEmpty() && !f.unit.IsAllyOf(this));
        List<Unit> adjacentEnemies = adjacentFieldsWithEnemies.ConvertAll(f => f.unit);
        if (!field.IsEmpty() && !field.unit.IsAllyOf(this)) adjacentEnemies.Add(field.unit);
        return adjacentEnemies;
    }

}
