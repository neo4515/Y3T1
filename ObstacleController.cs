using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [HideInInspector]
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (BattleManager.Instance != null && !BattleManager.Instance.isBattleActive)
        {
            int obstacleDamage = 1;
            BattleManager.Instance.DamageMainPhaseHealth(obstacleDamage);
        }
        Destroy(gameObject);
    }
}
