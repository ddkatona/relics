using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit {

    #region Subscriptions
    void Start() {
        owner.OnAllyKilled += Effect;
    }

    void OnDisable() {
        owner.OnAllyKilled -= Effect;
    }
    #endregion

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        // Steps
        options.UnionWith(GetOption(1, 0, MultiOption.Step));
        options.UnionWith(GetOption(1, 1, MultiOption.Step));
        options.UnionWith(GetOption(1, -1, MultiOption.Step));

        options.UnionWith(GetOption(0, 1, MultiOption.Step));
        options.UnionWith(GetOption(0, -1, MultiOption.Step));

        options.UnionWith(GetOption(-1, 0, MultiOption.Step));
        options.UnionWith(GetOption(-1, 1, MultiOption.Step));
        options.UnionWith(GetOption(-1, -1, MultiOption.Step));

        // Capture
        options.UnionWith(GetOptionsInRange(2, 3, Option.Capture));

        return options;
    }

    private void Effect(Unit allyKilled) {
        if (allyKilled == this) return;
        Unit mostExpensiveAlly = owner.MostExpensiveAlly(new List<Unit> { this });
        if (mostExpensiveAlly == null) return;
        mostExpensiveAlly.Cost = mostExpensiveAlly.Cost - 1;

        // Animation
        SupportAnimation supportAnimation = (SupportAnimation)GetAnimation<SupportAnimation>()?.Copy();
        supportAnimation?.Init(this, mostExpensiveAlly);
        supportAnimation?.Register();

        CostChangeAnimation costChangeAnimation = (CostChangeAnimation)GetAnimation<CostChangeAnimation>()?.Copy();
        costChangeAnimation?.Init(mostExpensiveAlly);
        costChangeAnimation?.Register();
    }

}
