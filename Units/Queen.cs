using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Unit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetStepCaptureArray(1, 0));
        options.UnionWith(GetStepCaptureArray(0, 1));
        options.UnionWith(GetStepCaptureArray(-1, 0));
        options.UnionWith(GetStepCaptureArray(0, -1));

        options.UnionWith(GetStepCaptureArray(1, 1));
        options.UnionWith(GetStepCaptureArray(1, -1));
        options.UnionWith(GetStepCaptureArray(-1, 1));
        options.UnionWith(GetStepCaptureArray(-1, -1));

        return options;
    }

}
