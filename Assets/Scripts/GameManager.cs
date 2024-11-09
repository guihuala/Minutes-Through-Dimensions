using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 预制件
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private GameObject buffItemPrefab;

    // 可用的增益效果列表
    [SerializeField] private List<Buff> availableBuffs;

    // 根物体
    public Transform rootTransform;

    // 游戏关卡和难度控制
    public int currentLevel = 1;
    [SerializeField] private float difficultyMultiplier = 1.1f;

    // 分数
    public int score = 0;

    // 边界
    public Vector2 mapBounds = new Vector2(9.5f, 9.5f);

    // 生成的敌人和障碍数量
    private int numberOfObstacles = 5;
    private int numberOfEnemies = 7;

    // 计时器
    private float startingTime = 60f;
    public float remainingTime;

    // 事件定义
    public event Action<int> OnScoreChanged; // 分数变化事件
    public event Action<float> OnTimeChanged; // 时间变化事件
    public event Action OnLevelChanged; // 关卡变化事件

    [SerializeField] private PlayerController player;

    public List<Vector3> buffSpawnPositions;

    // 物体间隔
    [SerializeField] private float minDistanceFromOrigin = 3f;
    [SerializeField] private float minSpacing = 1.5f;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        remainingTime = startingTime;
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        if (isGameOver) return;

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;

        OnTimeChanged?.Invoke(remainingTime);

        if (remainingTime <= 0)
        {
            GameOver();

            isGameOver = true;
        }
    }

    private void InitializeGame()
    {
        ClearRootObjects();
        AdjustDifficulty();

        if (currentLevel % 5 == 0)
        {
            GenerateBuffItems();
        }
        else
        {
            GenerateBarriers();

            for (int i = 0; i < numberOfEnemies; i++)
            {
                GameObject enemy = SpawnObjectAtRandomPosition(enemyPrefab);
                enemy.transform.SetParent(rootTransform);
            }

            GameObject exit = SpawnObjectAtRandomPosition(exitPrefab);
            exit.transform.SetParent(rootTransform);
        }
    }

    private void AdjustDifficulty()
    {
        float difficulty = Mathf.Pow(difficultyMultiplier, currentLevel - 1);

        numberOfObstacles = Mathf.RoundToInt(5 * difficulty);
        numberOfEnemies = Mathf.RoundToInt(7 * difficulty);
    }

    private void GenerateBarriers()
    {
        int numberOfBarriers = 10;

        for (int i = 0; i < numberOfBarriers; i++)
        {
            SpawnRandomBarrier();
        }
    }

    private void SpawnRandomBarrier()
    {
        int randomIndex = UnityEngine.Random.Range(0, obstaclePrefabs.Length);
        GameObject barrierPrefab = obstaclePrefabs[randomIndex];

        Vector3 randomPosition = GetRandomPositionAvoidingOrigin();

        GameObject barrierInstance = Instantiate(barrierPrefab, randomPosition, Quaternion.identity);
        barrierInstance.transform.SetParent(rootTransform);
    }

    private void GenerateBuffItems()
    {
        int numberOfBuffs = Mathf.Min(buffSpawnPositions.Count, 3);

        for (int i = 0; i < numberOfBuffs; i++)
        {
            Vector3 spawnPosition = buffSpawnPositions[i];

            GameObject buffItem = Instantiate(buffItemPrefab, spawnPosition, Quaternion.identity);
            buffItem.transform.SetParent(rootTransform);

            int randomBuffIndex = UnityEngine.Random.Range(0, availableBuffs.Count);
            Buff randomBuff = availableBuffs[randomBuffIndex];

            BuffItem buffItemScript = buffItem.GetComponent<BuffItem>();
            if (buffItemScript != null)
            {
                buffItemScript.buff = randomBuff;
            }
        }
    }

    private GameObject SpawnObjectAtRandomPosition(GameObject prefab)
    {
        Vector3 randomPosition = GetRandomPositionAvoidingOrigin();
        return Instantiate(prefab, randomPosition, Quaternion.identity);
    }

    private Vector3 GetRandomPositionAvoidingOrigin()
    {
        Vector3 randomPosition;
        int maxAttempts = 100; //避免陷入无限循环
        int attempts = 0;

        do
        {
            randomPosition = new Vector3(
                UnityEngine.Random.Range(-mapBounds.x, mapBounds.x),
                UnityEngine.Random.Range(-mapBounds.y, mapBounds.y),
                0f
            );
            attempts++;
        }
        while ((Vector3.Distance(randomPosition, Vector3.zero) < minDistanceFromOrigin || CheckOverlap(randomPosition))
               && attempts < maxAttempts);

        return randomPosition;
    }

    // 检查新生成的物体是否与已有物体重叠
    private bool CheckOverlap(Vector3 position)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, minSpacing);
        return hitColliders.Length > 0;
    }

    public void GameOver()
    {
        ScoreManager.Instance.SaveScores(score, currentLevel);
        StartCoroutine(LoadGameOver());
    }

    private IEnumerator LoadGameOver()
    {
        player.isDead = true;
        UIManager.Instance.ShowGameOver(remainingTime);

        yield return new WaitForSeconds(1.5f);

        LevelLoder.Instance.TransitionToScene(0);
    }

    public void UpdateScore(int points)
    {
        score += points;
        OnScoreChanged?.Invoke(score);
        ScoreManager.Instance.SaveScores(score, currentLevel); // 更新并保存分数和层数
    }

    public void NextLevel()
    {
        remainingTime = Mathf.Min(remainingTime + 2, startingTime);
        OnTimeChanged?.Invoke(remainingTime);

        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        // 难度调整
        if (currentLevel % 10 == 1)
        {
            difficultyMultiplier = MathF.Max(1f, difficultyMultiplier - 0.01f);
        }

        SfxManager.instance.PlaySFX(2);

        yield return new WaitForSeconds(.5f);
        player.isTransfer = false;

        player.transform.position = Vector3.zero;

        UpdateScore(currentLevel * 10);
        ClearRootObjects();

        currentLevel++;
        OnLevelChanged?.Invoke();
        InitializeGame();
    }

    private void ClearRootObjects()
    {
        foreach (Transform child in rootTransform)
        {
            Destroy(child.gameObject);
        }
    }
}




