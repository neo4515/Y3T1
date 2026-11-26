using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Hit! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealToFull(int maxHealth)
    {
        currentHealth = maxHealth;
        Debug.Log("Healing Complete: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Game Over!");

        gameObject.SetActive(false);
    }
}
