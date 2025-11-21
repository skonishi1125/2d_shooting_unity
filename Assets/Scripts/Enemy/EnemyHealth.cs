using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    protected SpriteRenderer sr;

    private float hitPoint;
    [SerializeField] private float maxHitPoint;

    [Header("Hit Flash")]
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float flashDuration = 0.05f;
    private float flashTimer = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hitPoint = maxHitPoint;
        baseColor = sr.color;
    }

    private void Update()
    {
        UpdateHitFlash();
    }

    // 演出が増えてきた場合、
    // EnemyHitFlashというコンポーネントを作って切り出しても良い
    private void UpdateHitFlash()
    {
        if (flashTimer <= 0f)
            return;

        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0f)
        {
            sr.color = baseColor;
        }
    }


    public void TakeDamage(float damage)
    {
        hitPoint = hitPoint - damage;
        sr.color = hitColor;
        flashTimer = flashDuration;

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
