using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassStone : GraphicalObject {

    // References
    public Renderer R;
    public TextMeshPro tmp;
    public Renderer arrows;

    // Settings
    public float lowPosition;
    public float highPosition;
    public Color blue;
    public Color green;
    public Color gray;

    // State
    public float arrowAlphaTarget;
    public float arrowAlphaVelocity;
    public Color targetColor;
    public float t;

    #region Subscribtions
    void OnEnable() {
        Game.TurnStarted += OnTurnStart;
        Game.TurnEnded += OnTurnEnd;
    }
    void OnDisable() {
        Game.TurnStarted -= OnTurnStart;
        Game.TurnEnded -= OnTurnEnd;
    }
    #endregion

    public Game Game => transform.parent.GetComponent<Game>();

    private void Update() {
        // Pressing (Sin) motion
        if (t < Mathf.PI) {
            float verticalDistance = Mathf.Abs(lowPosition - highPosition);
            transform.position = new Vector3(transform.position.x, -Mathf.Sin(t) * verticalDistance, transform.position.z);
            t += Time.deltaTime * 8f;
        }

        // Color fading
        float intensity = 1f;
        float speed = 3f;
        Color current = R.material.GetColor("_EmissionColor");
        Color way = targetColor * intensity - current;
        current += way * Time.deltaTime * speed;
        R.material.SetColor("_EmissionColor", current);

        // Arrow fading
        Color arrowColor = arrows.material.color;
        arrowColor.a = Mathf.SmoothDamp(arrowColor.a, arrowAlphaTarget, ref arrowAlphaVelocity, 0.2f);
        arrows.material.color = arrowColor;
        arrows.enabled = arrowColor.a > 0.1f;
    }

    public void Press() {
        t = 0; // Initialize animations

        // Particles
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.enabled = true;

        ParticleSystemRenderer pr = GetComponentInChildren<ParticleSystemRenderer>();
        Color current = R.material.GetColor("_EmissionColor");
        pr.material.SetColor("_EmissionColor", current);

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(R.material.color, 0.0f), new GradientColorKey(R.material.color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        col.color = grad;

        ps.Play();

        // Sound
        GetComponent<AudioSource>().Play();
    }

    public void SetText(string text) {
        tmp.SetText(text);
    }

    public void ShowArrow(bool show) {
        arrowAlphaTarget = show ? 1f : 0f;
    }

    private void OnTurnStart(Player commingPlayer) {
        if (Game.board.IsCopied()) return;
        List<Unit> units = Game.board.GetAlliesOf(commingPlayer);
        units = units.FindAll(unit => unit.GetMoves().Count > 0);

        float unitsThatCanMove = units.Count;
        if (unitsThatCanMove == 0) {
            // No possible Moves
            targetColor = green;
            SetText("PASS");
        } else {
            // 1+ possible moves
            targetColor = blue;
            SetText("PASS");
        }

        if (Game.consecutivePasses == 0) {
            ShowArrow(true);
        } else {
            ShowArrow(Game.round % 2 == Game.GetOtherPlayer(commingPlayer).ID);
            SetText("END ROUND");
        }
    }

    private void OnTurnEnd(Player player) {
        targetColor = gray;
        //SetText("Turning...");
        SetText("");
    }

}