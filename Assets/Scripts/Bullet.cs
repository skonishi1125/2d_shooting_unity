using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Build.Content;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Bullet Details")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f; // 画面外に出た時のチェック
    [SerializeField] private float damage = 1f;
    [SerializeField] Color attackEffectColor = Color.white;

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
        if (!collision.CompareTag("Enemy"))
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
