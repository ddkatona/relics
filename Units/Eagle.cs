using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Unit, ISupportUnit {

    public override HashSet<OptionField> GetOptionsByRule() {
        HashSet<OptionField> options = new HashSet<OptionField>();
        options.UnionWith(GetOptionsInRange(4, 4, Option.Step));
        options.UnionWith(GetGlobalOptions(Option.Support));
        return options;
    }

    public void Support(Move move) {
        Field end = move.GetEnd(Board);
        Board.SwapUnits(this, end.unit);
    }
}
