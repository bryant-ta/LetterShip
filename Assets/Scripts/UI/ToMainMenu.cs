using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour {
    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
