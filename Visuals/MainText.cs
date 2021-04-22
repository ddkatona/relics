using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainText : MonoBehaviour {

    public static MainText single;

    private List<Message> queue = new List<Message>();
    
    public TextMeshProUGUI main;
    public TextMeshProUGUI turn;
    public CanvasGroup mainCG;

    public bool show;

    // Start is called before the first frame update
    void Start() {
        single = this;
    }

    // Update is called once per frame
    void Update() {
        if (queue.Count > 0) {
            if (!show) ShowNext();
        }
        int modifier = show ? 1 : -1;
        mainCG.alpha += Time.deltaTime * 4f * modifier;
    }

    public static void Show(string text, string playerName, float length = 1f) {
        Message msg = new Message(text, length, playerName);
        single.queue.Add(msg);
    }

    private void ShowNext() {
        Message nextMsg = queue[0];
        main.SetText(nextMsg.Text);
        turn.SetText(nextMsg.playerName);
        Invoke("Hide", nextMsg.Length);
        show = true;
    }

    private void Hide() {
        show = false;
        queue.RemoveAt(0);
    }

}

public class Message {
    private readonly string text;
    private readonly float length;
    public string playerName;

    public Message(string text, float length, string name = "") {
        this.text = text;
        this.length = length;
        playerName = name;
    }

    public string Text => text;
    public float Length => length;
}
