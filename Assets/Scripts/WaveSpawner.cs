using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private WaveData waveData;
    private float timer = 0f;
    private int nextIndex = 0;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // waveDataに設定したSpawnが尽きたら終了
        if (nextIndex >= waveData.events.Count)
            return;

        var e = waveData.events[nextIndex];

        if(timer > e.time)
        {
            SpawnEnemy(e);
            nextIndex++;
        }

    }

    private void SpawnEnemy(WaveData.SpawnEvent e)
    {

        // x軸はSpawnerのpositionを使う
        var viewportSpawnX = cam.WorldToViewportPoint(gameObject.transform.position).x;
        var viewportSpawnY = e.viewportY;

        var viewportSpawnPosition = new Vector3(viewportSpawnX, viewportSpawnY, 0f);
        var worldSpawnPosition = cam.ViewportToWorldPoint(viewportSpawnPosition);
        worldSpawnPosition.z = 0f;

        Instantiate(e.enemyPrefab, worldSpawnPosition, Quaternion.identity);

    }
}
