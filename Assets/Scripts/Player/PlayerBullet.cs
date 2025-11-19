using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Bullet Details")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック
    [SerializeField] private float damage = 1f;
    private int enemyLayer;

    [SerializeField] Color attackEffectColor = Color.white;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnEnable()
    {
        rb.linearVelocity = (Vector2)transform.right * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != enemyLayer)
            return;

        // Bullet自体を消す
        Destroy(gameObject);

        var health = collision.GetComponent<EnemyHealth>();
        if (health == null)
        {
            Debug.LogWarning($"EnemyHealth が見つかりません。: {collision.name}");
            return;
        }

        health.TakeDamage(damage);
    }

}
