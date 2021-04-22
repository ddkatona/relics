using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitGraphics : MonoBehaviour {

    [Header("Manual")]
    public UnitViewer unitViewer;
    public Renderer effectIcon;
    public Highlight highlight;
    public UnitMarker unitMarker;
    public KeywordBar keywordBar;
    public Transform header;
    public Renderer body;
    public CostGraphics costGraphics;

    [Header("State")]
    public bool flying;

    [Header("References")]
    public Unit unit;

    #region Subscribtions
    private void OnEnable() {
        Unit.Game.TurnStarted += TurnStart;
        Unit.KeywordManager.OnKeywordsChange += SpecialKeywordGraphicsUpdate;
    }

    private void OnDisable() {
        if (Unit == null) return;
        Unit.Game.TurnStarted -= TurnStart;
        Unit.KeywordManager.OnKeywordsChange -= SpecialKeywordGraphicsUpdate;
    }
    #endregion

    void Update() {
        if (unitViewer.show) return;

        if (flying) {
            Random.InitState(transform.GetInstanceID());
            float phase = Random.Range(0f, Mathf.PI*2);
            transform.position = unit.transform.position + Vector3.up * (0.2f + Mathf.Sin(Time.time * 2f + phase) * 0.2f);
        }                
    }

    public Unit Unit => unit ?? transform.parent.GetComponent<Unit>();

    #region Initizalize
    public void Initialize(Unit unit) {
        this.unit = unit;

        body.material.SetColor("_EmissionColor", unit.owner.playerColor);
        HeaderGraphicsInit();
        ArtGraphicInit();
        InfoCardInit();        

        keywordBar.Initialize(unit);
    }

    public void HeaderGraphicsInit() {
        costGraphics.Initialize(unit);

        // Underlay color
        Color costLabelColor = unit.owner.playerColor.grayscale > 0.5f ? Color.black : Color.white;
        Color underlayColor = new Color(costLabelColor.r, costLabelColor.g, costLabelColor.b, 0.15f);
        header.Find("KeywordVisuals/SpecialUnderlay").GetComponent<Renderer>().material.color = underlayColor;
        header.Find("KeywordVisuals/BasicUnderlay").GetComponent<Renderer>().material.color = underlayColor;
    }

    public void ArtGraphicInit() {
        // Image
        Texture image = unit.image != null ? unit.image : Resources.Load<Texture>("UnitImages/QuestionUnit");
        Material imageMaterial = transform.Find("Main/Art").GetComponent<Renderer>().material;
        imageMaterial.SetTexture("_EmissionMap", image);

        //Color shifting
        Color shadedColor = unit.owner.playerColor / 8f;
        imageMaterial.SetColor("_Color", shadedColor);
    }

    private void InfoCardInit() {
        Transform info = transform.Find("Info/Content");
        info.Find("CardNameText").GetComponent<TextMeshPro>().text = unit.unitName;
        info.Find("CardText").GetComponent<TextMeshPro>().text = SText.Format(unit.cardText);
    }
    #endregion

    #region Special Graphics
    public void RefreshGraphics() {
        if(effectIcon.enabled != !unit.hasHaste)
            effectIcon.enabled = !unit.hasHaste;
    }

    public void TurnStart(Player player) {
        RefreshGraphics();
    }

    private void SpecialKeywordGraphicsUpdate(List<Keyword> ks) {
        if(Unit.KeywordManager.HasKeyword<Flying>()) {
            transform.position = unit.transform.position - Vector3.forward * 0.5f;
            flying = true;
        } else {
            transform.position = unit.transform.position;
            flying = false;
        }
    }
    #endregion

}
