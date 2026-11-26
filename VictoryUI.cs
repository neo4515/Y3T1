using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("ตัว Panel ของหน้าจอ Victory")]
    public GameObject victoryPanel;

    [Tooltip("ข้อความหัวข้อ เช่น Victory!")]
    public TMP_Text victoryTitleText;

    [Tooltip("ปุ่มเริ่มเกมใหม่อีกครั้ง")]
    public Button restartButton;

    [Tooltip("ปุ่มกลับไปหน้า Main Menu")]
    public Button mainMenuButton;

    [Header("Scene & Pause Settings")]
    [Tooltip("ชื่อ Scene หน้า Main Menu (ต้องเพิ่มใน Build Settings ด้วย)")]
    public string mainMenuSceneName = "MainMenu";

    [Tooltip("หยุดเวลาเกม (Time.timeScale = 0) เมื่อขึ้นหน้า Victory")]
    public bool pauseGameOnVictory = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnClickRestart);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
        }
    }

    public void ShowVictory(string customTitle = "VICTORY!")
    {
        if (victoryTitleText != null)
        {
            victoryTitleText.text = customTitle;
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (pauseGameOnVictory)
        {
            Time.timeScale = 0f;
        }
    }

    public void HideVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("Main Menu Scene Name is empty!");
        }
    }
}