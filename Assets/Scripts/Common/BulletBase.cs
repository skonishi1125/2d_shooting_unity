using UnityEngine;

public abstract class BulletBase : MonoBehaviour, IDamageSource
{
    [Header("Bullet Details")]
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = .5f;
    [SerializeField] protected int damage = 1;
    public int Damage => damage;
    public float LifeTime => lifeTime;

    protected Rigidbody2D rb;
    protected PooledBullet pooled;
    protected bool isDespawned;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pooled = GetComponent<PooledBullet>();
    }
    protected void ScheduleDespawn()
    {
        isDespawned = false;
        CancelInvoke(nameof(Despawn));
        Invoke(nameof(Despawn), lifeTime);
    }

    protected virtual void Despawn()
    {
        if (isDespawned)
            return;

        isDespawned = true;
        CancelInvoke(nameof(Despawn));// 二重呼び出し対策

        // プールがあれば戻す・なければ保険で Destroy
        if (pooled != null)
            pooled.Despawn();
        else
            Destroy(gameObject);
    }

    // 継承先で定義
    protected abstract void OnTriggerEnter2D(Collider2D collision);


}
