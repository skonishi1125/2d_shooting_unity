using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    private bool IsGameOver = false;

    [Header("Stage Clear")]
    [SerializeField] private CanvasGroup fadeCanvas; // フェードアウト用の黒いキャンバス
    public bool IsStageClear { get; private set; } = false;
    private int currentStageIndex = 1; // Stage1



    private void Awake()
    {
        CheckGameManager();
        if (gameOverUI == null)
            Debug.LogWarning("GameOverUI が設定されていません。");
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
        }

        SceneManager.LoadScene("Stage" + currentStageIndex);
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
    }


}
