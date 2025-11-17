using UnityEditor.Build.Content;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = (Vector2)transform.right * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO; レイヤーとして分けたほうが良い
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject); // TODO: 硬い敵の場合は体力を減らすように
        }
    }

}
