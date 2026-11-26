using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHeartUI : MonoBehaviour
{
    [Header("Heart Slots (Left > Right)")]
    public List<Image> heartImages = new List<Image>();

    [Header("Optional Auto-Spawn")]
    public Image heartPrefab;
    public Transform heartContainer;

    [Header("Heart Sprites")]
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    [Header("Display Behavior")]
    public bool hideWhenNoActiveEnemy = true;

    void OnEnable()
    {
        RefreshUI();
    }

    void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (fullHeartSprite == null || emptyHeartSprite == null)
            return;

        bool isBattle = BattleManager.Instance != null && BattleManager.Instance.isBattleActive;
        Enemy enemy = (BattleManager.Instance != null) ? BattleManager.Instance.currentActiveEnemy : null;

        if (!isBattle || enemy == null)
        {
            if (hideWhenNoActiveEnemy)
            {
                SetAllHeartsActive(false);
            }
            return;
        }

        int maxHp = Mathf.Max(0, enemy.maxHealth);
        int currentHp = Mathf.Clamp(enemy.currentHealth, 0, maxHp);

        EnsureHeartSlots(maxHp);

        for (int i = 0; i < heartImages.Count; i++)
        {
            Image heart = heartImages[i];
            if (heart == null) continue;

            bool isWithinMax = i < maxHp;
            if (heart.gameObject.activeSelf != isWithinMax)
            {
                heart.gameObject.SetActive(isWithinMax);
            }

            if (isWithinMax)
            {
                Sprite targetSprite = (i < currentHp) ? fullHeartSprite : emptyHeartSprite;
                if (heart.sprite != targetSprite)
                {
                    heart.sprite = targetSprite;
                }
            }
        }
    }

    private void EnsureHeartSlots(int requiredCount)
    {
        if (heartPrefab == null || heartContainer == null) return;

        while (heartImages.Count < requiredCount)
        {
            Image newHeart = Instantiate(heartPrefab, heartContainer);
            newHeart.gameObject.SetActive(false);
            heartImages.Add(newHeart);
        }
    }

    private void SetAllHeartsActive(bool state)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (heartImages[i] != null && heartImages[i].gameObject.activeSelf != state)
            {
                heartImages[i].gameObject.SetActive(state);
            }
        }
    }
}