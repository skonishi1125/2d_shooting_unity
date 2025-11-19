using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f;
    private float fireTimer = 0f;

    private void Start()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("bulletPrefab か firePointが未設定です。");
            return;
        }
    }

    private void Update()
    {
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
