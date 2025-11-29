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
    [SerializeField] private GameObject statusUI;
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
    private const int MaxStageIndex = 3;

    // キルカウンタ
    private int enemyKillCount = 0;
    private int enemyKillDropRate = 5; // ex: 5なら5体ごとにアイテムを落とす

    private void Awake()
    {
        CheckGameManager();
        if (StatusUIHolder == null || pauseMenu == null)
            Debug.LogWarning("GameManager: 設定値が正しくありません。");
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
        // ① シーン共通処理
        IsStageClear = false;
        fadeCanvas.alpha = 0f;
        IsPausing = false;
        IsGameOver = false;
        Time.timeScale = 1f;

        stageCall.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverUI.SetActive(false);
        clearCall.SetActive(false);

        // ② シーンごとの分岐
        if (scene.name == Scenes.Title)
        {
            SetStatusUIActive(false);
            ResetEachData();

            return;
        }

        // ここまで来たら、Stageシーン
        if (scene.name.StartsWith("Stage"))
        {
            SetStatusUIActive(true);
            DisplayStageCall(currentStageIndex);
        }
    }

    private void SetStatusUIActive(bool isDisplayed)
    {
        manualRow.SetActive(isDisplayed);
        statusUI.SetActive(isDisplayed);
    }

    // ステータス、キルカウンタ, Stage進行度の初期化
    private void ResetEachData()
    {
        ResetRunData();
        enemyKillCount = 0;
        currentStageIndex = 1;
    }

    // GameOver画面のリトライ
    // リトライ時は、Stage1からやり直させる形にする
    public void Retry()
    {
        gameOverUI.SetActive(false);
        ResetEachData(); // リトライ時は初期化しておく
        SceneManager.LoadScene(Scenes.Stage + currentStageIndex);
    }


    // Pauseからタイトルへ戻る
    public void ReturnToTitle()
    {
        StopAllCoroutines(); // ボス討伐後にpauseしてタイトルに戻ったケースの考慮
        SceneManager.LoadScene(Scenes.Title);
    }

    // クリア画面からタイトルへ戻る
    public void ClearToTitle()
    {
        SceneManager.LoadScene(Scenes.Title);
    }

    // Pause / CLEAR からTitle遷移時にステータスをリセットする
    private void ResetRunData()
    {
        HasRunData = false;
        RunData = new PlayerRunData();
        StatusUIHolder.ResetRows(); // StatusUIに付与されたBarの表示も消す
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

    // Stage1, というような表示処理
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

        IsStageClear = true;

        // コルーチンを回す前に、ゲームクリアか判断する
        if (currentStageIndex >= MaxStageIndex)
        {
            Debug.Log("Complete!");
            clearCall.SetActive(true);
            return;
        }

        currentStageIndex++;
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

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        // UI表示
        gameOverUI.SetActive(true);
    }

    // ステージ開始処理
    // どのステージに行くかだけ担当して、入った後何をするかはOnSceneLoadedに任せる。
    public void StartStage(int index)
    {
        currentStageIndex = index;
        SceneManager.LoadScene(Scenes.Stage + index);
        //DisplayStageCall(index); OnSceneLoadedで処理するので、不要
    }

    // ステージ移動処理
    private void LoadNextStage()
    {
        SceneManager.LoadScene(Scenes.Stage + currentStageIndex);
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
