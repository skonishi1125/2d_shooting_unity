using System;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private enum StageState
    {
        RunningWaves, // 雑魚Wave 進行中
        FightingBoss, // ボス戦
        Cleared       // ステージクリア後
    }

    [SerializeField] private StageData stageData;
    [SerializeField] private WaveSpawner waveSpawner;

    private int currentWaveIndex = 0;
    private bool IsStageFinished = false;

    [SerializeField] private EnemyBase bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    private StageState state = StageState.RunningWaves;
    private EnemyBase currentBoss;


    private void Start()
    {
        if (stageData == null || stageData.waves == null || stageData.waves.Count == 0)
        {
            Debug.LogWarning("StageData が設定されていません。");
            IsStageFinished = true;
            return;
        }

        StartCurrentWave(); // stageData.waves[0]を実行

    }

    private void Update()
    {
        // Stage Clearした場合は、処理をしなくてよい
        if (IsStageFinished)
            return;

        switch (state)
        {
            case StageState.RunningWaves:
                UpdateRunningWaves();
                break;
            case StageState.FightingBoss:
                UpdateFightingBoss();
                break;
            case StageState.Cleared:
                break;
        }

        Debug.Log(state);

    }

    private void UpdateRunningWaves()
    {
        // Wave進行中なら何もしない
        if (!waveSpawner.IsFinished) 
            return;

        // ---------- Wave終了時の処理 ---------- 
        // 繰り上げ
        currentWaveIndex++;

        // まだWaveが残っているなら、再び実行
        if (currentWaveIndex < stageData.waves.Count)
        {
            StartCurrentWave();
            return;
        }

        // ---------- 全Waveを終えた時の処理 ----------
        // 画面上に敵が残っていなければ、ボスを生成し、stateを更新
        if (AreAllEnemiesDefeated())
        {
            SpawnBoss();
            state = StageState.FightingBoss;
        }
    }

    private void StartCurrentWave()
    {
        var wave = stageData.waves[currentWaveIndex];
        waveSpawner.StartWave(wave);
    }

    private void SpawnBoss()
    {
        currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
    }

    // 画面上の敵全てを倒したかどうかの判定
    private bool AreAllEnemiesDefeated()
    {
        // "Enemy" タグが付いた GameObject が 1つもなければ true
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    private void UpdateFightingBoss()
    {
        if (currentBoss != null)
            return;

        // ボス討伐 (EnemyHealth.Die() で Destroy時）
        IsStageFinished = true;
        state = StageState.Cleared;

        GameManager.Instance.StageClear();
    }

}
