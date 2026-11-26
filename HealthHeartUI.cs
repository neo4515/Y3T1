using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeartUI : MonoBehaviour
{
    public enum HealthSourceType
    {
        MainPhaseHealth,
        BattlePlayerHealth
    }

    [Header("Health Source")]
    public HealthSourceType healthSource = HealthSourceType.MainPhaseHealth;

    [Header("Heart Slots (เตรียมช่อง Image เผื่อ Max HP สูงสุด)")]
    public List<Image> heartImages = new List<Image>();

    [Header("Heart Sprites")]
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    void Update()
    {
        if (BattleManager.Instance == null || fullHeartSprite == null || emptyHeartSprite == null)
            return;

        int currentHp = 0;
        int maxHp = 0;

        if (healthSource == HealthSourceType.MainPhaseHealth)
        {
            currentHp = Mathf.Max(0, BattleManager.Instance.mainPhaseCurrentHealth);
            maxHp = Mathf.Max(0, BattleManager.Instance.mainPhaseMaxHealth);
        }
        else
        {
            currentHp = Mathf.Max(0, BattleManager.Instance.playerCurrentHealth);
            maxHp = Mathf.Max(0, BattleManager.Instance.playerMaxHealth);
        }

        for (int i = 0; i < heartImages.Count; i++)
        {
            Image heart = heartImages[i];
            if (heart == null) continue;

            bool shouldShow = i < maxHp;
            if (heart.gameObject.activeSelf != shouldShow)
            {
                heart.gameObject.SetActive(shouldShow);
            }

            if (shouldShow)
            {
                Sprite targetSprite = (i < currentHp) ? fullHeartSprite : emptyHeartSprite;
                if (heart.sprite != targetSprite)
                {
                    heart.sprite = targetSprite;
                }
            }
        }
    }
}