using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private StageData stageData;
    [SerializeField] private WaveSpawner waveSpawner;

    private int currentWaveIndex = 0;
    private bool IsStageFinished = false;

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
        if (IsStageFinished) // Stageクリア後は止める
            return;

        
        if (!waveSpawner.IsFinished) // WaveSpawnerが動作中は止める
            return;

        currentWaveIndex++;

        if (currentWaveIndex >= stageData.waves.Count)
        {
            IsStageFinished = true;
            GameManager.Instance.StageClear();
            return;
        }

        // Waveが終わり、まだゲームが続いているなら次のWaveを実行
        StartCurrentWave();

    }

    private void StartCurrentWave()
    {
        var wave = stageData.waves[currentWaveIndex];
        waveSpawner.StartWave(wave);
    }

}
