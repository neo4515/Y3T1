using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public string enemyName = "Street Enemy";
    public int maxHealth = 3;
    public int attackDamage = 1;

    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{enemyName} Got Attacked! Total Health: {Mathf.Max(0, currentHealth)}");
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void ResetEnemy()
    {
        currentHealth = maxHealth;
    }
}
