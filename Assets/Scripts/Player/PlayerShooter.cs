using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // GameObject bulletPrefabともできるが、
    // その場合InstantiateするとGameObjectで返るようになる
    //[SerializeField] PlayerBullet bulletPrefab; <- bulletPool用意により不要になった

    [SerializeField] Transform firePoint;
    [SerializeField] PlayerBulletPool bulletPool;
    private float fireTimer = 0f;
    private bool canFire;

    private PlayerStatus status;

    private void Awake()
    {
        // Player側でも呼んでいるが、別にこっち側で呼んでもいい
        // (Player が Shooter に Status を渡す必要はない。依存が分かりづらくなる）
        status = GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        if (bulletPool == null || firePoint == null || status == null)
        {
            Debug.LogWarning("bulletPrefab か firePoint か statusが正しく取得できていません。");
            return;
        }
        canFire = true;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            canFire = true;
        }
    }

    public void Fire()
    {
        if (!canFire)
            return;

        //canFire = false;
        //fireTimer = status.FireInterval;
        //PlayerBullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        //bullet.Init(status.ShotDamage, status.LifeTime);

        canFire = false;
        fireTimer = status.FireInterval;

        // Poolから取得する
        var obj = bulletPool.Get(firePoint.position, Quaternion.identity);
        var bullet = obj.GetComponent<PlayerBullet>();
        if (bullet != null)
        {
            bullet.Init(status.ShotDamage, status.LifeTime);
        }

    }

}
