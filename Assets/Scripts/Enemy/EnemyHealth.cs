using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private EnemyBase enemyBase;

    private float hitPoint;
    [SerializeField] private float maxHitPoint;

    [Header("Hit Flash")]
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float flashDuration = 0.05f;
    private float flashTimer = 0f;

    // ドロップに関するアクションイベント
    // GameManager側で設定したメソッドを、このアクション（イベントリスト）に格納
    // 受取る引数は、今回EnemyHealthでないといけない
    public static event Action<EnemyHealth> OnAnyEnemyDied;


    private void Awake()
    {
        if (sr == null)
        {
            Debug.LogWarning("EnemyHealth: Spriteが未取得のため、コード側で割り当てます。");
            sr = GetComponentInChildren<SpriteRenderer>();
        }
        // 実際はEnemyA, B, Bossなどがアタッチされているが、これで取れる
        enemyBase = GetComponent<EnemyBase>();
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
        // 無敵中なら何もしない
        if (enemyBase != null && enemyBase.IsInvincible)
            return;

        hitPoint = hitPoint - damage;
        sr.color = hitColor;
        flashTimer = flashDuration;

        if (hitPoint <= 0f)
            Die();
    }

    private void Die()
    {
        // OnAnyEnemyDiedに登録されている全てのメソッドの呼び出し
        // ? で、存在しなければnullを返すようにしている
        OnAnyEnemyDied?.Invoke(this);

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
