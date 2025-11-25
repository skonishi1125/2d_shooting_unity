using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerRunData
{
    public float moveSpeed;
    public float lifetime;
    public int shotDamage;
    public float fireInterval;

    public void SetFromStatus(PlayerStatus status)
    {
        moveSpeed = status.MoveSpeed;
        lifetime = status.LifeTime;
        shotDamage = status.ShotDamage;
        fireInterval = status.FireInterval;
    }
    public void ApplyTo(PlayerStatus status)
    {
        status.SetFromRunData(this);
    }

}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerRunData RunData { get; private set; } = new PlayerRunData();
    public bool HasRunData { get; private set; }

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverUI;
    private bool IsGameOver = false;

    [Header("Status UI")]
    [SerializeField] public StatusUIHolder StatusUIHolder;

    [Header("Stage Clear")]
    [SerializeField] private CanvasGroup fadeCanvas; // フェードアウト用の黒いキャンバス
    public bool IsStageClear { get; private set; } = false;
    private int currentStageIndex = 1; // Stage1

    // キルカウンタ
    private int enemyKillCount = 0;
    private int enemyKillDropRate = 5; // ex: 5なら5体ごとにアイテムを落とす

    private void Awake()
    {
        CheckGameManager();
        if (gameOverUI == null || StatusUIHolder == null)
            Debug.LogWarning("GameManagerにUIが正しく設定されていません。");
    }

    // シーン移動時に各値を戻すための設定
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnemyHealth.OnAnyEnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemyHealth.OnAnyEnemyDied -= OnEnemyDied;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded");
        ResetManagerState();
        // ついでにStageControllerやPlayerなどを探して参照をセットしたりもできる
    }

    private void CheckGameManager()
    {
        // シングルトンパターンで設計
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);// Stage2, 3とSceneをまたいで残したい
    }

    public void InitRunDataIfNeeded(PlayerStatus status)
    {
        // Stage1開始時: 基本ステータスでRunDataを初期化
        if (!HasRunData)
        {
            RunData.SetFromStatus(status);
            HasRunData = true;
        }
        else
        {
            // 2 面目以降：RunData の値を PlayerStatus に反映
            RunData.ApplyTo(status);
        }
    }

    public void SyncRunDataFrom(PlayerStatus status)
    {
        // RunDataが生成済かどうかのチェック
        if (!HasRunData)
            return;

        RunData.SetFromStatus(status);
    }

    private void OnEnemyDied(EnemyHealth deadEnemy)
    {
        enemyKillCount++;

        if (enemyKillCount % enemyKillDropRate == 0)
        {
            var drop = deadEnemy.GetComponent<EnemyDrop>();
            if (drop != null)
            {
                drop.SpawnDrop();
            }
        }
    }


    public void StageClear()
    {
        if (IsStageClear)
            return;

        Debug.Log("GameManager: STAGE CLEAR!");
        IsStageClear = true;

        StartCoroutine(StageClearCo());

    }

    private IEnumerator StageClearCo()
    {
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeOutCo());

        LoadNextStage();

    }

    private IEnumerator FadeOutCo()
    {
        float t = 0f;
        float duration = 2f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
    }
    private void LoadNextStage()
    {
        // 次ステージ
        currentStageIndex++;

        if (currentStageIndex > 3)
        {
            // 全クリ 現状タイトルに戻るが、リザルトがあるならリザルトに
            SceneManager.LoadScene("Title");
            return;
        }

        SceneManager.LoadScene("Stage" + currentStageIndex);
    }

    private void ResetManagerState()
    {
        IsStageClear = false;
        //enemyKillCount = 0;  リセットしなくてもいいかも
        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        // UI表示
        gameOverUI.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverUI.SetActive(false);

    }

}
