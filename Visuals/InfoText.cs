using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoText : MonoBehaviour {

    public static InfoText single;

    private List<Message> queue = new List<Message>();

    public TextMeshProUGUI main;
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

    public static void Show(string text, float length = 1.0f) {
        Message msg = new Message(text, length);
        single.queue.Add(msg);
    }

    private void ShowNext() {
        Message nextMsg = queue[0];
        main.SetText(nextMsg.Text);
        Invoke("Hide", nextMsg.Length);
        show = true;
    }

    private void Hide() {
        show = false;
        queue.RemoveAt(0);
    }

}
