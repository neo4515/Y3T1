using UnityEngine;

public class EnemyEncounterTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject encounterUI;

    [Header("Enemy Reference")]
    public Enemy linkedEnemy;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        CheckPlayer(other.gameObject);
    }

    void CheckPlayer(GameObject hitObject)
    {
        if (!hasTriggered && hitObject.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Trigger Activated! Opening an UI...");

            Time.timeScale = 0f;

            if (BattleManager.Instance != null && linkedEnemy != null)
            {
                BattleManager.Instance.SetCurrentEnemy(linkedEnemy);
            }

            if (encounterUI != null)
            {
                encounterUI.SetActive(true);
            }
        }
    }
}
