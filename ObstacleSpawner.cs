using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleType
    {
        public GameObject prefab;
        public float customSpeed = 10f;
    }

    [Header("Obstacle Settings")]
    public ObstacleType[] obstacleTypes = new ObstacleType[3];

    [Header("Spawner Settings")]
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomObstacle();
            timer = 0f;
        }
    }

    void SpawnRandomObstacle()
    {
        if (obstacleTypes == null || obstacleTypes.Length == 0) return;

        int randomIndex = Random.Range(0, obstacleTypes.Length);
        ObstacleType selectedType = obstacleTypes[randomIndex];

        if (selectedType.prefab == null) return;

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion spawnRot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        GameObject newObstacle = Instantiate(selectedType.prefab, spawnPos, spawnRot);

        ObstacleController obsScript = newObstacle.GetComponent<ObstacleController>();
        if (obsScript != null)
        {
            obsScript.speed = selectedType.customSpeed;
        }
    }
}
