using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyB enemy;
    private float hitPoint;
    [SerializeField] private float maxHitPoint;

    private void Start()
    {
        hitPoint = maxHitPoint;
        enemy = GetComponent<EnemyB>();
    }

    public void TakeDamage(float damage)
    {

        if (enemy == null)
            return;

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

}
