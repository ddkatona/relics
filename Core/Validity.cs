using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Validity {
    readonly bool valid;
    readonly string error;
    
    public bool Valid => valid;
    public string Error => error;

    public Validity(string error) {
        valid = false;
        this.error = error;
    }

    public Validity() {
        valid = true;
        error = "";
    }

    public bool PrintIfInvalid() {
        if (!valid) InfoText.Show(error);
        return valid;
    } 

    // Returns other if this is Valid
    public Validity GetIfWorse(Validity other) {
        if (!valid) return this;
        return other;
    }

}
