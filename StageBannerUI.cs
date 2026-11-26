using System.Collections;
using TMPro;
using UnityEngine;

public class StageBannerUI : MonoBehaviour
{
    public static StageBannerUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject bannerRoot;

    public TMP_Text stageText;

    [Header("Display Settings")]
    public float displayDuration = 5f;

    public string stagePrefix = "Stage ";

    public bool displayOneBasedIndex = true;

    private Coroutine hideCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (bannerRoot != null)
        {
            bannerRoot.SetActive(false);
        }
    }

    public void ShowStageBanner(int stageIndex)
    {
        int displayNumber = displayOneBasedIndex ? stageIndex + 1 : stageIndex;
        ShowCustomMessage($"{stagePrefix}{displayNumber}");
    }

    public void ShowCustomMessage(string message)
    {
        if (bannerRoot == null || stageText == null) return;

        stageText.text = message;
        bannerRoot.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideAfterDelay(displayDuration));
    }

    public void HideImmediately()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (bannerRoot != null)
        {
            bannerRoot.SetActive(false);
        }
    }

    private IEnumerator HideAfterDelay(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        if (bannerRoot != null)
        {
            bannerRoot.SetActive(false);
        }
        hideCoroutine = null;
    }
}