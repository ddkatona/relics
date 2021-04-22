using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Unit, ISupportUnit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetOption(1, 0, MultiOption.StepCapture));
        options.UnionWith(GetOption(1, 1, MultiOption.StepCapture));
        options.UnionWith(GetOption(1, -1, MultiOption.StepCapture));

        options.UnionWith(GetOption(0, 1, MultiOption.StepCapture));
        options.UnionWith(GetOption(0, -1, MultiOption.StepCapture));

        options.UnionWith(GetOption(-1, 0, MultiOption.StepCapture));
        options.UnionWith(GetOption(-1, 1, MultiOption.StepCapture));
        options.UnionWith(GetOption(-1, -1, MultiOption.StepCapture));

        options.UnionWith(GetOption(3, 0, MultiOption.Support));
        options.UnionWith(GetOption(-4, 0, MultiOption.Support));

        return options;
    }

    public void Support(Move move) {
        Field end = move.GetEnd(Board);
        Board.SwapUnits(this, end.unit);
    }
}
