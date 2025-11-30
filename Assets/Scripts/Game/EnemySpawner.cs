using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Camera cam;

    [Header("Spawn Information")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = .3f;
    private float spawnTimer = 0f;
    [SerializeField] private float viewportSpawnX;
    [SerializeField] private float viewportSpawnY;
    [SerializeField] private Vector2 viewportSpawnPosition;
    [SerializeField] private Vector2 worldSpawnPosition;

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Spawn();
            spawnTimer = spawnInterval;
        }
    }

    private void Spawn()
    {
        if (enemyPrefab == null)
            return;

        // x軸はSpawnerのposition, y軸は画面のy軸内で指定した箇所に沸かせる
        viewportSpawnX = cam.WorldToViewportPoint(gameObject.transform.position).x;
        viewportSpawnY = Random.Range(0f, 1.0f);

        viewportSpawnPosition = new Vector2(viewportSpawnX, viewportSpawnY);
        worldSpawnPosition = cam.ViewportToWorldPoint(viewportSpawnPosition);


        Instantiate(enemyPrefab, worldSpawnPosition, Quaternion.identity);
    }
}
