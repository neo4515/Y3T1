using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Player Stats")]
    public int attackDamage = 1;
    public int energy = 0;
    public int maxHealth = 3;
    public int currentHealth = 3;
    public int currentStage = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WinBattleReward()
    {
        currentStage++;
        attackDamage += 1;
        energy += 1;

        Debug.Log($"Stage Passed! Current Stage: {currentStage} | Damage Increased: {attackDamage} | Total Energy: {energy}");
    }

    public bool UseEnergyForSkip(int cost = 3)
    {
        if (energy >= cost)
        {
            energy -= cost;
            Debug.Log($"Energy Used! Total Energy: {energy}");
            return true;
        }
        else
        {
            Debug.Log("Energy Are Not Enough To Skip!");
            return false;
        }
    }
}
