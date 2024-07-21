using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] string mainSceneName = "MainScene";
    [SerializeField] string secondarySceneName = "SecondaryScene";

    public void LoadMainScene() { SceneManager.LoadScene(mainSceneName); }

    public void LoadSecondaryScene() { SceneManager.LoadScene(secondarySceneName); }
}