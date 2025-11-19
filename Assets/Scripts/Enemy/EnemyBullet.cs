using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Bullet Details")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック
    [SerializeField] private int damage = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // ←方向のため、-1を掛ける
        rb.linearVelocity = (Vector2)transform.right * -1 * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Layers.Player)
            return;

        // Bullet自体を消す
        Destroy(gameObject);

        var health = collision.GetComponent<PlayerHealth>();
        if (health == null)
        {
            Debug.LogWarning($"PlayerHealth が見つかりません。: {collision.name}");
            return;
        }

        health.TakeDamage(damage);
    }


}
