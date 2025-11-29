using DG.Tweening;
using System.Collections;
using TMPro;
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

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;
    public bool IsPausing { get; private set; } = false;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverUI;
    private bool IsGameOver = false;

    [Header("Status UI")]
    [SerializeField] public StatusUIHolder StatusUIHolder;
    [SerializeField] private GameObject statusUIGroup;
    [SerializeField] private GameObject manualRow;

    [Header("Stages")]
    [SerializeField] private GameObject stageCall;
    [SerializeField] private TextMeshProUGUI stageCallText;
    [SerializeField] private CanvasGroup fadeCanvas; // フェードアウト用の黒いキャンバス

    [Header("Clear")]
    [SerializeField] private GameObject clearCall;
    [SerializeField] private TextMeshProUGUI clearCallText;
    [SerializeField] private CanvasGroup clearToTitleButton;

    public bool IsStageClear { get; private set; } = false;
    private int currentStageIndex = 1; // Stage1

    // キルカウンタ
    private int enemyKillCount = 0;
    private int enemyKillDropRate = 5; // ex: 5なら5体ごとにアイテムを落とす

    private void Awake()
    {
        CheckGameManager();
        if (StatusUIHolder == null || pauseMenu == null)
            Debug.LogWarning("GameManager: 設定値が正しくありません。");

        SetStatusUIActive(false);

    }

    private void SetStatusUIActive(bool isDisplayed)
    {
        manualRow.SetActive(isDisplayed);
        statusUIGroup.SetActive(isDisplayed);
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
        ResetManagerState();
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
            SetStatusUIActive(true);
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

    // Pause / CLEARしてTitleへ戻る時にステータスをリセットする
    private void ResetRunData()
    {
        HasRunData = false;
        RunData = new PlayerRunData();

    }

    public void TogglePausing()
    {
        IsPausing = !IsPausing;

        Time.timeScale = IsPausing ? 0f : 1f;
        pauseMenu.SetActive(IsPausing);
    }

    public void StageClear()
    {
        if (IsStageClear)
            return;

        Debug.Log("GameManager: STAGE CLEAR!");
        IsStageClear = true;

        // コルーチンを回す前に、ゲームクリアか判断する
        if (currentStageIndex > 2)
        {
            Debug.Log("Complete!");
            clearCall.SetActive(true);
            return;
        }

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
            //SceneManager.LoadScene(Scenes.Title);
            Debug.Log("clear!");
            clearCall.SetActive(true);

            return;
        }

        SceneManager.LoadScene(Scenes.Stage + currentStageIndex);
        DisplayStageCall(currentStageIndex);
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

    // ゲームの開始処理 Start()が予約されているのでToをつけておく
    public void StartStage(int index)
    {
        SetStatusUIActive(true);
        SceneManager.LoadScene(Scenes.Stage + index);
        DisplayStageCall(index);
    }
    public void Retry()
    {
        gameOverUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToTitle()
    {
        TogglePausing();
        SetStatusUIActive(false);

        ResetRunData();

        stageCall.SetActive(false);

        SceneManager.LoadScene(Scenes.Title);
    }

    public void ClearToTitle()
    {
        SetStatusUIActive(false);
        stageCall.SetActive(false);
        clearCall.SetActive(false);

        ResetRunData();

        SceneManager.LoadScene(Scenes.Title);
    }

    private void DisplayStageCall(int index)
    {
        // Coroutineでもできるが、DOTweenでやってみる
        stageCall.SetActive(true);
        var canvasGroup = stageCall.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        stageCallText.text = "Stage" + index;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.4f).SetEase(Ease.OutQuad)) // フェードイン
           .AppendInterval(1.6f)                                       // しばらく表示
           .Append(canvasGroup.DOFade(0f, 0.6f).SetEase(Ease.Linear))  // フェードアウト
           .OnComplete(() =>
           {
               // 完全に透明になったら非表示にする
               stageCall.SetActive(false);
           });

        // ポーズ中(timeScale = 0)でも動作させる
        seq.SetUpdate(true);
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

}
