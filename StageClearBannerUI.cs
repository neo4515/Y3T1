using System.Collections;
using UnityEngine;
using TMPro;

public class StageClearBannerUI : MonoBehaviour
{
    public static StageClearBannerUI Instance;

    [Header("UI Reference")]
    [Tooltip("Noti GameObject In MainPhasePanel")]
    public GameObject bannerObject;

    [Tooltip("Noti Text When Passed The Level")]
    public TMP_Text rewardText;

    [Header("Display Settings")]
    public float displayDuration = 3f;

    [Tooltip("Unscaled Time")]
    public bool useUnscaledTime = true;

    private Coroutine hideCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (bannerObject != null)
        {
            bannerObject.SetActive(false);
        }
    }

    public void ShowStageClearReward(int coinsEarned, int energyEarned = 1)
    {
        if (bannerObject == null || rewardText == null) return;

        rewardText.text = $"Stage Clear! +{energyEarned} Energy | +{coinsEarned} Coins";
        bannerObject.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        if (useUnscaledTime)
            yield return new WaitForSecondsRealtime(displayDuration);
        else
            yield return new WaitForSeconds(displayDuration);

        if (bannerObject != null)
        {
            bannerObject.SetActive(false);
        }
        hideCoroutine = null;
    }
}
