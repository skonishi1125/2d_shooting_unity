using UnityEngine;

public class EnemyBullet : MonoBehaviour, IDamageSource
{
    private Rigidbody2D rb;

    [Header("Bullet Details")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック
    [SerializeField] private int damage = 1;
    public int Damage => damage;
    private Vector2 moveDir = Vector2.left; // デフォルト: 左に発射

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // moveDir が設定されていれば、それに沿って飛ばす
        rb.linearVelocity = moveDir.normalized * speed;

        Destroy(gameObject, lifeTime);
    }

    // Shooter からInstantiateした際に、その弾にパラメータを設定する際に呼ぶ
    public void Init(Vector2 direction)
    {
        moveDir = direction.normalized;
        rb.linearVelocity = moveDir * speed;
    }

}
