using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Unit {

    #region Subscription
    private void OnEnable() {
        OnUnitKilled += DeathEffect;
        
    }

    private void OnDisable() {
        OnUnitKilled -= DeathEffect;
    }
    #endregion

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        // Step
        options.UnionWith(GetOption(0, -1, MultiOption.Step));
        // Capture
        options.UnionWith(GetOption(1, 1, MultiOption.Capture));
        options.UnionWith(GetOption(0, 1, MultiOption.StepCapture));
        options.UnionWith(GetOption(-1, 1, MultiOption.Capture));        
        return options;
    }

    private void DeathEffect(Unit killer, Field field) {
        killer.KeywordManager.GrantKeyword<Poisoned>();

        // Animation
        BuffAnimation buffAnimation = (BuffAnimation)GetAnimation<BuffAnimation>().Copy();
        buffAnimation?.Init(killer);
        buffAnimation?.Register();
    }

}
