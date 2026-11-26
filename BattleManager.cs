using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("Player Stats")]
    public int playerMaxHealth = 3;
    public int playerCurrentHealth;
    public int playerDamage = 1;
    public int playerEnergy = 0;

    [Header("References")]
    public GameObject playerObject;
    public MonoBehaviour playerMovementScript;
    public Enemy currentActiveEnemy;
    private int enemyCurrentHealth;

    [Header("UI Panels")]
    public GameObject encounterUIPanel;
    public GameObject battleUIPanel;

    [Header("Phase UI Panels")]
    public GameObject mainPhasePanel;
    public GameObject battlePhasePanel;

    [Header("Main Phase Health (Obstacle)")]
    public int mainPhaseMaxHealth = 3;
    public int mainPhaseCurrentHealth = 3;

    [Header("Default Spawn Settings")]
    public Transform defaultSpawnPoint;

    [Header("Next Level / Spawn Settings")]
    public List<Transform> spawnPointList = new List<Transform>();
    private int currentLevelIndex = 0;
    private Transform lastestCheckpoint;

    [Header("Stage Clear Heal Settings")]
    public bool fullHealOnStageClear = true;
    public int healAmountOnClear = 1;

    [Header("Currency & Shop Settings")]
    public int playerCoins = 0;

    [Tooltip("Rewards Will Be Recieved After Passed The Level/Kill The Enemy")]
    public int[] coinRewardsPerLevel = { 100, 75, 50, 30, 15 };
    private int lastEarnedCoins = 0;

    private Vector3 playerOriginalPos;
    private Vector3 enemyOriginalPos;
    public bool isBattleActive;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        SetPhaseUI(false);

        if (defaultSpawnPoint != null)
        {
            lastestCheckpoint = defaultSpawnPoint;
        }
        else if (spawnPointList != null && spawnPointList.Count > 0)
        {
            lastestCheckpoint = spawnPointList[0];
        }

        if (StageBannerUI.Instance != null)
        {
            StageBannerUI.Instance.ShowStageBanner(currentLevelIndex);
        }
    }

    public void SetCurrentEnemy(Enemy enemy)
    {
        currentActiveEnemy = enemy;
    }

    public void SetPhaseUI(bool isBattle)
    {
        if (mainPhasePanel != null)
        {
            mainPhasePanel.SetActive(!isBattle);
        }
        if (battlePhasePanel != null)
        {
            battlePhasePanel.SetActive(isBattle);
        }
        if (!isBattle && StageBannerUI.Instance != null)
        {
            StageBannerUI.Instance.ShowStageBanner(currentLevelIndex);
        }
    }

    public void StartBattle()
    {
        Debug.Log("=== StartBattle Function Activated ===");
        SetPhaseUI(true);

        playerCurrentHealth = playerMaxHealth;
        mainPhaseCurrentHealth = mainPhaseMaxHealth;

        if (encounterUIPanel != null)
        {
            encounterUIPanel.SetActive(false);
            Debug.Log("-> encounterUIPanel Closed");
        }
        else
        {
            Debug.LogError(" encounterUIPanel is Null");
        }

        if (battleUIPanel != null) battleUIPanel.SetActive(true);

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        if (playerObject != null) playerOriginalPos = playerObject.transform.position;

        if (currentActiveEnemy != null)
        {
            enemyOriginalPos = currentActiveEnemy.transform.position;
            currentActiveEnemy.ResetEnemy();
            enemyCurrentHealth = currentActiveEnemy.maxHealth;
            Debug.Log($"<color=yellow>=== Start battle With [{currentActiveEnemy.enemyName}] (Health: {enemyCurrentHealth} | Damage: {currentActiveEnemy.attackDamage}) ===</color>");
        }
        else
        {
            enemyCurrentHealth = 5;
            Debug.Log("<color=yellow>=== Start Battle With Enemy ===</color>");
        }

        Debug.Log($"[Status] Player health: {playerCurrentHealth}/{playerMaxHealth} | Player Damage: {playerDamage}");

        isBattleActive = true;
        StartCoroutine(BattleTurnRoutine());
    }

    public void DamageMainPhaseHealth(int damage)
    {
        if (isBattleActive) return;

        mainPhaseCurrentHealth = Mathf.Max(0, mainPhaseCurrentHealth - damage);
        Debug.Log($"Main Phase HP: {mainPhaseCurrentHealth}/{mainPhaseMaxHealth}");

        if (mainPhaseCurrentHealth <= 0)
        {
            mainPhaseCurrentHealth = mainPhaseMaxHealth;
            PlayerDie();
        }
    }

    IEnumerator BattleTurnRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (isBattleActive)
        {
            if (playerObject != null && currentActiveEnemy != null)
            {
                Debug.Log("<color=cyan>[Players Turn] Attacking...</color>");

                Vector3 dirToEnemy = (currentActiveEnemy.transform.position - playerObject.transform.position).normalized;
                Vector3 targetPos = currentActiveEnemy.transform.position - (dirToEnemy * 1.5f);
                targetPos.y = playerOriginalPos.y;

                yield return StartCoroutine(MoveToPosition(playerObject.transform, targetPos, 0.3f));

                currentActiveEnemy.TakeDamage(playerDamage);
                enemyCurrentHealth = currentActiveEnemy.GetCurrentHealth();
                Debug.Log($"<color=cyan>Player Attack! Damaged {playerDamage} | Enemy Health: {Mathf.Max(0, enemyCurrentHealth)}</color>");

                yield return new WaitForSecondsRealtime(0.8f);

                Debug.Log("<color=cyan>[Players Turn] Return To Their Position</color>");
                yield return StartCoroutine(MoveToPosition(playerObject.transform, playerOriginalPos, 0.3f));
            }

            if (enemyCurrentHealth <= 0)
            {
                WinBattle();
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.5f);

            int enemyDamage = currentActiveEnemy != null ? currentActiveEnemy.attackDamage : 1;
            if (playerObject != null && currentActiveEnemy != null)
            {
                Debug.Log("<color=orange>[Enemys Turn] Attacking...</color>");

                Vector3 dirToPlayer = (playerObject.transform.position - currentActiveEnemy.transform.position).normalized;
                Vector3 targetPos = playerObject.transform.position - (dirToPlayer * 1.5f);
                targetPos.y = enemyOriginalPos.y;

                yield return StartCoroutine(MoveToPosition(currentActiveEnemy.transform, targetPos, 0.3f));

                playerCurrentHealth -= enemyDamage;
                Debug.Log($"<color=orange>Enemy Counter! Player Recieves {enemyDamage} Damage | Player Health: {Mathf.Max(0, playerCurrentHealth)}/{playerMaxHealth}</color>");

                yield return new WaitForSecondsRealtime(0.8f);

                Debug.Log("<color=orange>[Enemys Turn] Return To Their Position</color>");
                yield return StartCoroutine(MoveToPosition(currentActiveEnemy.transform, enemyOriginalPos, 0.3f));
            }

            if (playerCurrentHealth <= 0)
            {
                LoseBattle();
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    IEnumerator MoveToPosition(Transform objTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPos = objTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            objTransform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }
        objTransform.position = targetPosition;
    }

    void WinBattle()
    {
        isBattleActive = false;
        playerEnergy += 1;

        FullHealOnStageClear();

        if (currentActiveEnemy != null)
        {
            Destroy(currentActiveEnemy.gameObject);
        }

        bool reachedFinalStage = IsFinalSpawnPointStage();

        if (!reachedFinalStage)
        {
            AdvanceToNextLevel();
            GiveLevelReward();
            StartCoroutine(DelayedCloseBattle());
        }
        else
        {
            GiveLevelReward();
            if (battleUIPanel != null) battleUIPanel.SetActive(false);
            SetPhaseUI(false);

            if (VictoryUI.Instance != null)
            {
                VictoryUI.Instance.ShowVictory("VICTORY!");
            }
        }
    }

    private bool IsFinalSpawnPointStage()
    {
        if (spawnPointList == null || spawnPointList.Count == 0)
            return false;

        int effectiveIndex = Mathf.Max(0, currentLevelIndex);
        return effectiveIndex >= spawnPointList.Count - 1;
    }

    IEnumerator DelayedCloseBattle()
    {
        yield return new WaitForSecondsRealtime(2f);
        CloseBattle();
    }

    void GiveLevelReward()
    {
        int rewardAmount = 10;
        if (coinRewardsPerLevel != null && coinRewardsPerLevel.Length > 0)
        {
            int index = Mathf.Clamp(currentLevelIndex, 0, coinRewardsPerLevel.Length - 1);
            rewardAmount = coinRewardsPerLevel[index];
        }

        lastEarnedCoins = rewardAmount;
        playerCoins += rewardAmount;
        Debug.Log($"Recieve {rewardAmount} Coins! | Total Coins: {playerCoins}");
    }

    void LoseBattle()
    {
        Debug.Log("<color=red> You Lose The Battle!</color>");
        PlayerDie();
    }

    public void PlayerDie()
    {
        isBattleActive = false;
        SetPhaseUI(false);
        if (battleUIPanel != null) battleUIPanel.SetActive(false);

        mainPhaseCurrentHealth = mainPhaseMaxHealth;
        playerCurrentHealth = playerMaxHealth;

        if (playerObject != null)
        {
            playerObject.SetActive(true);

            PlayerHealth playerHealthComp = playerObject.GetComponent<PlayerHealth>();
            if (playerHealthComp != null)
            {
                playerHealthComp.currentHealth = playerMaxHealth;
            }

            Transform respawnTarget = GetCurrentStageSpawnPoint()   ;

            if (respawnTarget != null)
            {
                playerObject.transform.position = respawnTarget.position;
                lastestCheckpoint = respawnTarget;
                Debug.Log($"<color=blue> Revive! Teleport Back To: {respawnTarget.name}</color>");
            }
            else
            {
                Debug.LogWarning("Not Found (Default Spawn Or Checkpoint)");
            }
        }
    }

    private Transform GetCurrentStageSpawnPoint()
    {
        if (lastestCheckpoint != null)
        {
            return lastestCheckpoint;
        }

        if (spawnPointList != null && spawnPointList.Count > 0)
        {
            int stageIndex = Mathf.Clamp(currentLevelIndex > 0 ? currentLevelIndex - 1 :0, 0, spawnPointList.Count - 1);
            if (spawnPointList[stageIndex] != null)
            {
                return spawnPointList[stageIndex];
            }
        }

        return defaultSpawnPoint;
    }

    void CloseBattle()
    {
        SetPhaseUI(false);
        if (battleUIPanel != null ) battleUIPanel.SetActive(false);

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        Time.timeScale = 1f;
        Debug.Log("=== Battle Ended ===");

        if (StageClearBannerUI.Instance != null && lastEarnedCoins > 0)
        {
            StageClearBannerUI.Instance.ShowStageClearReward(lastEarnedCoins, 1);
            lastEarnedCoins = 0;
        }
    }
    public void OnClickSkip()
    {
        if (playerEnergy >= 3)
        {
            playerEnergy -= 3;
            FullHealOnStageClear();

            Debug.Log("<color=magenta> Uses 3 Energies, Battle Skip!</color>");
            isBattleActive = false;

            if (currentActiveEnemy != null)
            {
                Destroy(currentActiveEnemy.gameObject);
            }

            if (encounterUIPanel != null) encounterUIPanel.SetActive(false);
            if (battleUIPanel != null) battleUIPanel.SetActive(false);

            AdvanceToNextLevel();

            GiveLevelReward();

            CloseBattle();
        }
        else
        {
            Debug.Log("<color=red> Not Enough Energy! (3 Energies Needed)</color>");
        }
    }

    void AdvanceToNextLevel()
    {
        playerDamage += 1;
        Debug.Log($"<color=green> Player Won! | Upgrade: Players Damage Increase To {playerDamage} | Total Energy: {playerEnergy}/3</color>");

        if (spawnPointList != null && spawnPointList.Count > 0 && playerObject != null)
        {
            if (currentLevelIndex < spawnPointList.Count)
            {
                Transform targetSpawnPoint = spawnPointList[currentLevelIndex];

                if (targetSpawnPoint != null)
                {
                    playerObject.transform.position = targetSpawnPoint.position;

                    lastestCheckpoint = targetSpawnPoint;

                    Debug.Log($"<color=blue> [Level Success] Teleported To: {targetSpawnPoint.name} (Index: {currentLevelIndex}) Completed!</color>");

                    currentLevelIndex++;
                }
                else
                {
                    Debug.LogWarning($"Spawn {currentLevelIndex + 1} In List Are Null (Blank)");
                }
            }
            else
            {
                Debug.Log("<color=yellow> You Passed Every Level!</color>");
            }
        }
        else
        {
            Debug.LogWarning("Havent List The Spawn Point In Game Manager");
        }
    }

    private void FullHealOnStageClear()
    {
        mainPhaseCurrentHealth = mainPhaseMaxHealth;
        playerCurrentHealth = playerMaxHealth;

        if (playerObject != null)
        {
            PlayerHealth playerHealthComp = playerObject.GetComponent<PlayerHealth>();
            if (playerHealthComp != null)
            {
                playerHealthComp.currentHealth = playerMaxHealth;
            }
        }

        Debug.Log($"<color=lime>Stage Clear Full Heal! Main HP: {mainPhaseCurrentHealth}/{mainPhaseMaxHealth} | Battle HP: {playerCurrentHealth}/{playerMaxHealth}</color>");
    }
}
