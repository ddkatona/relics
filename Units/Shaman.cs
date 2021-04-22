using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : Unit, ISupportUnit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();

        // Capture
        options.UnionWith(GetOptionsInRange(1, 1, Option.Step));
        options.UnionWith(GetOptionsInRange(1, 5, Option.Support));
        return options;
    }

    public void Support(Move move) {
        Unit other = move.GetEnd().unit;
        other.hasHaste = true;
        other.KeywordManager.RevokeKeyword<Poisoned>();

        // Animation
        SupportAnimation supportAnimation = (SupportAnimation)GetAnimation<SupportAnimation>().Copy();
        supportAnimation?.Init(this, other);
        supportAnimation?.Register();

        BuffAnimation buffAnimation = (BuffAnimation)GetAnimation<BuffAnimation>().Copy();
        buffAnimation?.Init(other);
        buffAnimation?.Register();
    }
}
