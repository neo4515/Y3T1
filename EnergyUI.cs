using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [Header("Energy Slots (Left > Right)")]
    public List<Image> energyImages = new List<Image>();

    [Header("Energy Sprites")]
    public Sprite chargedEnergySprite;
    public Sprite emptyEnergySprite;

    [Header("Max Slots Display (0 = Show On List)")]
    public int maxVisibleSlots = 3;

    void Update()
    {
        if (BattleManager.Instance == null || chargedEnergySprite == null || emptyEnergySprite == null)
            return;

        int currentEnergy = Mathf.Max(0, BattleManager.Instance.playerEnergy);
        int activeSlotCount = (maxVisibleSlots > 0)
            ? Mathf.Min(maxVisibleSlots, energyImages.Count)
            : energyImages.Count;

        for (int i = 0; i < energyImages.Count; i++)
        {
            Image slot = energyImages[i];
            if (slot == null) continue;

            bool shouldShowSlot = i < activeSlotCount;
            if (slot.gameObject.activeSelf != shouldShowSlot)
            {
                slot.gameObject.SetActive(shouldShowSlot);
            }

            if (shouldShowSlot)
            {
                Sprite targetSprite = (i < currentEnergy) ? chargedEnergySprite : emptyEnergySprite;
                if (slot.sprite != targetSprite)
                {
                    slot.sprite = targetSprite;
                }
            }
        }
    }
}

