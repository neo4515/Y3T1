using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterUIController : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject encounterUI;

    void Start()
    {
        if (encounterUI != null)
        {
            encounterUI.SetActive(false);
        }
    }

    public void OnClickFight()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartBattle();
        }
    }

    public void OnClickSkip()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnClickSkip();
        }
    }
}
