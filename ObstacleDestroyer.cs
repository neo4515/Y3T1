using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.GetComponent<ObstacleController>() != null)
        {
            Destroy(other.gameObject);  
        }
    }
}
