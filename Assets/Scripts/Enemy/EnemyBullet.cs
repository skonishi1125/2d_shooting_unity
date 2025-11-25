using UnityEngine;

public class EnemyBullet : MonoBehaviour, IDamageSource
{
    [Header("Bullet Details")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック
    [SerializeField] private int damage = 1;
    public int Damage => damage;
    private Vector2 moveDir = Vector2.left; // デフォルト: 左に発射

    private Rigidbody2D rb;
    private PooledBullet pooled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pooled = GetComponent<PooledBullet>();
    }

    // Shooter からInstantiateした際に、その弾にパラメータを設定する際に呼ぶ
    public void Init(Vector2 direction)
    {
        moveDir = direction.normalized;
        rb.linearVelocity = moveDir.normalized * speed;

        CancelInvoke(nameof(Despawn));
        Invoke(nameof(Despawn), lifeTime);
    }

    private void Despawn()
    {
        // プールがあれば戻す・なければ保険で Destroy
        if (pooled != null)
            pooled.Despawn();
        else
            Destroy(gameObject);
    }

}
