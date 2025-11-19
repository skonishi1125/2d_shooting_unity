using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float hitPoint;
    [SerializeField] private float maxHitPoint;

    private void Start()
    {
        hitPoint = maxHitPoint;
    }

    public void TakeDamage(float damage)
    {
        hitPoint = hitPoint - damage;
        if (hitPoint <= 0f)
            Die();
    }

    private void Die()
    {
        // Destroy(enemy.gameObject); じゃなく、
        // EnemyHealthもobjectに紐づいているのでこれで消せる
        Destroy(gameObject);

        // TODO
        // エフェクト再生やスコア加算、SEをつけるならここでつける
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Layers.PlayerBullet)
            return;

        var source = collision.GetComponent<IDamageSource>();
        if (source == null)
            return;

        TakeDamage(source.Damage);
    }

}
