using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionGraphics : MonoBehaviour {

    public GameObject prototype;
    public GameObject fieldMesh;
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public Sprite melee;
    public Sprite ranged;

    public Sprite step;
    public Sprite support;

    public void AddToDisplay(OptionField optionField) {
        Sprite sprite = GetSpriteForOptionField(optionField);
        CreateGraphics(sprite, optionField.constraint);
    }

    public void ShowToDisplay(Option option, Unit unit) {
        Sprite sprite = GetSpriteForOption(option, unit);
        CreateGraphics(sprite, null);
    }

    private void CreateGraphics(Sprite sprite, Constraint constraint) {
        if (sprites.FindAll(sr => sr.sprite == sprite).Count > 0) return;
        GameObject rendererGO = Instantiate(prototype, transform);
        SpriteRenderer renderer = rendererGO.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        if (constraint != null) {
            renderer.transform.Find("empty").GetComponent<SpriteRenderer>().enabled = true;
            renderer.GetComponentInChildren<TextMeshPro>().enabled = true;            
        }
        sprites.Add(renderer);
        SortSprites();
        RefreshPositions();
    }

    private void RefreshPositions() {
        int count = sprites.Count;
        float delta = GetHeight() / count;
        Vector3 scale = new Vector3(1, 1, 1) * 0.9f * (count == 1 ? delta/2f : delta);
        Vector3 p = prototype.transform.position + Vector3.forward * (GetHeight()/2 - delta / 2);
        for (int i = 0; i < count; i++) {
            sprites[i].transform.localScale = scale;
            sprites[i].transform.position = p;
            sprites[i].enabled = true;
            p -= Vector3.forward * delta;
        }
    }

    private void SortSprites() {
        sprites.Sort((a,b) => a.sprite == melee || a.sprite == ranged ? -1 : 1);    // Offense first
        sprites.Sort((a,b) => a.sprite == support ? 1 : -1);   // Support last
    }

    private float GetHeight() {
        return fieldMesh.transform.lossyScale.z - 0.1f;
    }

    private Sprite GetSpriteForOptionField(OptionField optionField) {
        return GetSpriteForOption(optionField.option, optionField.unit);
    }

    private Sprite GetSpriteForOption(Option option, Unit unit) {
        switch (option) {
            case Option.Capture:
                return unit.HasKeyword<Melee>() ? melee : ranged;
            case Option.Step:
                return step;
            case Option.Support:
                return support;
            default:
                return default;
        }        
    }

    public void Destroy() {
        for (int i = 0; i < sprites.Count; i++) {
            Destroy(sprites[i].gameObject);
        }
        sprites.Clear();
    }

}
