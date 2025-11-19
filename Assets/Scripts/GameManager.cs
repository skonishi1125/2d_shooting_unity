using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    private bool IsStageClear = false;
    private bool IsGameOver = false;

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

        IsStageClear = true;
        Debug.Log("GameManager: STAGE CLEAR!");
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
