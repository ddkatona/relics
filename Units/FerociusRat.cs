using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerociusRat : Unit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetMirrored(1, 1, MultiOption.StepCapture, true, true));
        options.UnionWith(GetMirrored(2, 2, MultiOption.StepCapture, true, false));
        return options;
    }

}
