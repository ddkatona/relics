using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousPlant : Unit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetOptionsInRange(1, 2, Option.Capture));
        return options;
    }

}
