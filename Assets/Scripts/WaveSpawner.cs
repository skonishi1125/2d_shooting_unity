using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private WaveData waveData;
    private Camera cam;

    private float timer = 0f;
    private int nextIndex = 0;
    public bool IsFinished {  get; private set; } = false;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // StageControllerからwaveDataが送られてくるまでは、何もしない
        if (waveData == null || IsFinished)
            return;

        timer += Time.deltaTime;

        // waveDataに設定したSpawnが尽きたら終了
        if (nextIndex >= waveData.events.Count)
        {
            Debug.Log("Wave Finished.");
            IsFinished = true;
            return;
        }

        var e = waveData.events[nextIndex];

        if(timer > e.time)
        {
            SpawnEnemy(e);
            nextIndex++;
        }
    }

    // Wave実施時、各初期値の実装
    public void StartWave(WaveData data)
    {
        waveData = data;
        timer = 0f;
        nextIndex = 0;
        IsFinished = false;
    }


    private void SpawnEnemy(WaveData.SpawnEvent e)
    {
        if (e.enemyPrefab == null)
        {
            Debug.LogWarning("WaveにEnemy Prefabが未登録です。");
            return;
        }

        // x軸はSpawnerのpositionを使う
        var viewportSpawnX = cam.WorldToViewportPoint(gameObject.transform.position).x;
        var viewportSpawnY = e.viewportY;

        var viewportSpawnPosition = new Vector3(viewportSpawnX, viewportSpawnY, 0f);
        var worldSpawnPosition = cam.ViewportToWorldPoint(viewportSpawnPosition);
        worldSpawnPosition.z = 0f;

        Instantiate(e.enemyPrefab, worldSpawnPosition, Quaternion.identity);

    }
}
