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
        if (collision.CompareTag("Enemy"))
        {
            // Bullet自体を消す
            Destroy(gameObject);

            // 敵のHPを削り、0になったらDestroyさせる
            EnemyB enemy = collision.GetComponent<EnemyB>();

            if (enemy == null)
                return;

            Color enemyOriginalColor = enemy.sr.color;
            enemy.hitPoint -= damage;
            enemy.SetSrColor(attackEffectColor);

            if (enemy.hitPoint <= 0)
                Destroy(collision.gameObject);
        }
    }

}
