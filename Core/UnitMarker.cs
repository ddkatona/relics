using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMarker : MonoBehaviour {

    public SpriteRenderer marker;
    public bool show;

    public List<Sprite> sprites = new List<Sprite>();

    void Update(){
        if(show) {
            float shift = gameObject.GetInstanceID() / Mathf.PI;
            shift = 0;

            // Rotate
            //transform.Rotate(transform.forward, -Time.deltaTime * 270f);

            // Alpha
            Color markerColor = marker.color;
            markerColor.a = Mathf.Sin(Time.time * 8f + shift) * 0.25f + 0.75f;
            marker.color = markerColor;

            // Scale
            marker.transform.localScale = new Vector3(1f, 1f, 1f) * (Mathf.Sin(Time.time * 4 + shift) * 0.1f + 0.5f);
        }
    }
    
    public void Switch(bool on, int iconID = 0) {
        show = on;
        marker.enabled = on;
        marker.sprite = sprites[iconID];
    }

}
