using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private EnemyBase enemyBase;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f;
    private float fireTimer = 0f;

    private void Awake()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("bulletPrefab か firePointが未設定です。");
            return;
        }
        enemyBase = GetComponent<EnemyBase>();
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
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }


}
