using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button startGameButton;
    public Button quitGameButton;

    [Header("Scene Settings")]
    public string gameplaySceneName = "SampleScene";

    void Awake()
    {
        Time.timeScale = 1.0f;

        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(StartGame);
        }

        if (quitGameButton != null)
        {
            quitGameButton.onClick.RemoveAllListeners();
            quitGameButton.onClick.AddListener(QuitGame);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(gameplaySceneName))
        { 
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            Debug.LogWarning("Gameplay Scene Name Is Empty");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
