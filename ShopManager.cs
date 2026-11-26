using System.Collections;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Phase UI References")]
    public GameObject mainPhasePanel;
    public GameObject shopPanel;

    [Header("UI Display")]
    public TMP_Text coinDisplayText;

    [Header("Shop Notification Settings")]
    [Tooltip("Noti Text For An Upgrade")]
    public TMP_Text shopNoticeText;

    [Tooltip("Noti Duration")]
    public float noticeDuration = 2.5f;

    [Header("Upgrade Settings")]
    public int hpUpgradeCost = 50;
    public int hpIncreaseAmount = 1;

    public int dmgUpgradeCost = 50;
    public int dmgIncreaseAmount = 1;

    private Coroutine noticeCoroutine;

    void Start()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        if (shopNoticeText != null)
        {
            shopNoticeText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (coinDisplayText != null && BattleManager.Instance != null)
        {
            coinDisplayText.text = "Coins: " + BattleManager.Instance.playerCoins;
        }
    }

    public void OpenShop()
    {
        if (mainPhasePanel != null)
        {
            mainPhasePanel.SetActive(false);
        }

        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Open Shop!");
        }

        if (shopNoticeText != null)
        {
            shopNoticeText.gameObject.SetActive(false);
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("Close Shop!");
        }

        bool isInBattle = BattleManager.Instance != null && BattleManager.Instance.isBattleActive;
        if (mainPhasePanel != null && !isInBattle)
        {
            mainPhasePanel.SetActive(true);
        }
    }

    public void BuyHPUpgrade()
    {
        if (BattleManager.Instance == null) return;

        if (BattleManager.Instance.playerCoins >= hpUpgradeCost)
        {
            BattleManager.Instance.playerCoins -= hpUpgradeCost;

            BattleManager.Instance.playerMaxHealth += hpIncreaseAmount;
            BattleManager.Instance.playerCurrentHealth = BattleManager.Instance.playerMaxHealth;

            BattleManager.Instance.mainPhaseMaxHealth += hpIncreaseAmount;
            BattleManager.Instance.mainPhaseCurrentHealth = BattleManager.Instance.mainPhaseMaxHealth;

            if (BattleManager.Instance.playerObject != null)
            {
                PlayerHealth playerHealthComp = BattleManager.Instance.playerObject.GetComponent<PlayerHealth>();
                if (playerHealthComp != null)
                {
                    playerHealthComp.currentHealth = BattleManager.Instance.mainPhaseCurrentHealth;
                }
            }

            ShowShopNotice($"HP Upgraded! Max HP: {BattleManager.Instance.playerMaxHealth}",Color.green);
        }
        else
        {
            ShowShopNotice("Not Enough Coins!", Color.red);
        }
    }

    public void BuyDMGUpgrade()
    {
        if (BattleManager.Instance != null)
        {
            if (BattleManager.Instance.playerCoins >= dmgUpgradeCost)
            {
                BattleManager.Instance.playerCoins -= dmgUpgradeCost;
                BattleManager.Instance.playerDamage += dmgIncreaseAmount;

                ShowShopNotice($"DMG Upgraded! Damage: {BattleManager.Instance.playerDamage}", Color.green);
                Debug.Log($"DMG Upgraded! Current DMG: {BattleManager.Instance.playerDamage}");
            }
            else
            {
                ShowShopNotice("Not Enough Coins!", Color.red);
                Debug.Log("Not enough coins for DMG upgrade!");
            }
        }
    }

    private void ShowShopNotice(string message, Color color)
    {
        if (shopNoticeText == null) return;

        shopNoticeText.text = message;
        shopNoticeText.color = color;
        shopNoticeText.gameObject.SetActive(true);

        if (noticeCoroutine != null)
        {
            StopCoroutine(noticeCoroutine);
        }
        noticeCoroutine = StartCoroutine(HideNoticeAfterDelay());
    }

    private IEnumerator HideNoticeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(noticeDuration);

        if (shopNoticeText != null)
        {
            shopNoticeText.gameObject.SetActive(false);
        }
        noticeCoroutine = null; 
    }
}