using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public enum ShootPattern
    {
        Straight,
        AimAtPlayer,
        Fan3Way,
        Fan5Way,
    }

    private EnemyBase enemyBase;
    [SerializeField] private ShootPattern pattern = ShootPattern.Straight;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f;
    private float fireTimer = 0f;

    private Transform player; // 自機狙い用

    private void Awake()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("bulletPrefab か firePointが未設定です。");
            return;
        }
        enemyBase = GetComponent<EnemyBase>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        // 登場演出中など、CanShootがfalseに設定される
        // その間は敵側で弾を打たない
        if (enemyBase != null && !enemyBase.CanShoot)
            return;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireInterval;
        }
       
    }

    private void Fire()
    {
        switch (pattern)
        {
            case ShootPattern.Straight:
                FireStraight();
                break;
            case ShootPattern.AimAtPlayer:
                FireAimAtPlayer();
                break;
            case ShootPattern.Fan3Way:
                FireFan(3, 15f);
                break;
            case ShootPattern.Fan5Way:
                FireFan(5, 10f);
                break;
        }
    }

    private void FireStraight()
    {
        FireBullet(Vector2.left);
    }

    private void FireAimAtPlayer()
    {
        if (player == null)
            return;

        Vector2 dir = (player.position - firePoint.position);
        FireBullet(dir);
    }

    private void FireFan(int count, float angleStep)
    {
        float startAngle = -(count - 1) * angleStep / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.left;
            FireBullet(dir);
        }
    }

    private void FireBullet(Vector2 direction)
    {
        var obj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        var bullet = obj.GetComponent<EnemyBullet>();
        bullet.Init(direction);
    }

}
