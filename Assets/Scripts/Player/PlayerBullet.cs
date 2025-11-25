using UnityEngine;

public class PlayerBullet : MonoBehaviour, IDamageSource
{

    [Header("Bullet Details")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = .5f;
    [SerializeField] private int damage = 1;
    public float LifeTime => lifeTime;
    public int Damage => damage;

    private Rigidbody2D rb;
    private PooledBullet pooled;
    private bool isDespawned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pooled = GetComponent<PooledBullet>();
    }

    // 弾丸を呼び出したときの初期化メソッド
    public void Init(int damage, float lifetime)
    {
        this.damage = damage;
        this.lifeTime = lifetime;

        rb.linearVelocity = (Vector2)transform.right * speed;
        isDespawned = false;

        // 弾丸の寿命をセットする
        CancelInvoke(nameof(Despawn));
        Invoke(nameof(Despawn), lifeTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾が敵レイヤーに当たった場合は、自身を削除(弾自身の責務)
        // ダメージ自体はIDamageSourceの責務、そのダメージでのライフ計算はEnemyHealthの責務
        //if (collision.gameObject.layer == Layers.Enemy)
        //    Destroy(gameObject);

        if (collision.gameObject.layer != Layers.Enemy)
            return;

        // 体力を減らす処理は、EnemyHealth側で実装済
        Despawn();
    }

    private void Despawn()
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

}
