using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

    public float duration;

    // Start is called before the first frame update
    void Start() {
        GetComponent<TextMeshProUGUI>().text = EndGameInfo.winnerName + " has won";
        Invoke("Menu", duration);
    }

    // Go to Menu
    private void Menu() {
        SceneManager.LoadScene("MainMenu");
    }

}
