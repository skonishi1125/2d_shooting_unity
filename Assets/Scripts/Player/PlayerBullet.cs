using UnityEngine;

public class PlayerBullet : MonoBehaviour, IDamageSource
{
    private Rigidbody2D rb;

    [Header("Bullet Details")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = .5f;
    [SerializeField] private int damage = 1;

    public int Damage => damage;
    public float LifeTime => lifeTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = (Vector2)transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾が敵レイヤーに当たった場合は、自身を削除(弾自身の責務)
        // ダメージ自体はIDamageSourceの責務、そのダメージでのライフ計算はEnemyHealthの責務
        if (collision.gameObject.layer == Layers.Enemy)
            Destroy(gameObject);
    }

    public void Init(int damage, float lifetime)
    {
        this.damage = damage;
        this.lifeTime = lifetime;
        Destroy(gameObject, this.lifeTime);
    }

}
