using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostGraphics : MonoBehaviour {

    public Unit unit;
    public TextMeshPro costTMP;
    public Renderer underlay;

    public TextMeshPro clone;

    public void Update() {
        if(clone != null) {
            clone.transform.position += Vector3.up * Time.deltaTime * 1f;
            clone.transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * 2f;

            Color current = clone.color;
            current.a -= Time.deltaTime * 0.5f;
            clone.color = current;
        }
    }

    public void Initialize(Unit unit) {
        this.unit = unit;

        // Cost circle color
        underlay.material.SetColor("_EmissionColor", unit.owner.playerColor);

        // Cost text color
        Color costLabelColor = unit.owner.playerColor.grayscale > 0.5f ? Color.black : Color.white;
        costTMP.color = costLabelColor;
        costTMP.SetText(unit.Cost.ToString());
    }

    public void Refresh() {
        int oldCost = int.Parse(costTMP.text);
        int newCost = unit.Cost;
        costTMP.text = unit.Cost.ToString();

        int diff = newCost - oldCost;
        GameObject cloneGO = Instantiate(costTMP.gameObject, costTMP.transform.position, costTMP.transform.rotation);
        clone = cloneGO.GetComponent<TextMeshPro>();
        clone.text = diff.ToString();
        clone.color = Mathf.Sign(diff) == 1 ? Color.red : Color.green;
        Invoke("EndAnimation", 1f);
    }

    public void EndAnimation() {
        Destroy(clone.gameObject);
    }

}
