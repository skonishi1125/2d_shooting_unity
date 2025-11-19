using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Bullet Details")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック

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
}
