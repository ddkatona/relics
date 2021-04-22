using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildGrizzly : Unit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetStepCaptureArray(1, 0, maxSteps: 2));
        options.UnionWith(GetStepCaptureArray(-1, 0, maxSteps: 2));
        options.UnionWith(GetStepCaptureArray(0, 1, maxSteps: 2));
        options.UnionWith(GetStepCaptureArray(0, -1, maxSteps: 2));

        options.UnionWith(GetOption(1, 1, MultiOption.StepCapture));
        options.UnionWith(GetOption(1, -1, MultiOption.StepCapture));
        options.UnionWith(GetOption(-1, 1, MultiOption.StepCapture));
        options.UnionWith(GetOption(-1, -1, MultiOption.StepCapture));
        return options;
    }

}
