using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Unit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetMirrored(2, 1, MultiOption.StepCapture, true, true));
        options.UnionWith(GetMirrored(1, 2, MultiOption.StepCapture, true, true));
        return options;
    }

}
