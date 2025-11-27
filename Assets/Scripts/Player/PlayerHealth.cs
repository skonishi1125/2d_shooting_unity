using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private SpriteRenderer graphics;

    private int currentLife;
    public int CurrentLife => currentLife;

    [SerializeField] public int maxLife = 3;
    [SerializeField] private bool isInvincible; // 無敵フラグ
    [SerializeField] private float invincibleTime = 1f;
    private Coroutine startInvinsibleCo;

    private void Awake()
    {
        if (graphics == null)
        {
            Debug.LogWarning("Spriteが未取得のため、コード側で割り当てます。");
            graphics = GetComponentInChildren<SpriteRenderer>();
        }
        currentLife = maxLife;

    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
            return;

        currentLife -= amount;
        if (currentLife <= 0)
        {
            Die();
            return;
        }

        StartInvincibleCo();

    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.GameOver();
    }

    // 被弾後の無敵時間
    private void StartInvincibleCo()
    {
        // 無敵中に何かダメージを受けることがあったとき、再度無敵エフェクトをかける
        if (startInvinsibleCo != null)
            StopCoroutine(startInvinsibleCo);

        startInvinsibleCo = StartCoroutine(StartInvincible());
    }

    private IEnumerator StartInvincible()
    {
        SetIgnoreLayerCollison(true);
        isInvincible = true;

        float elapsed = 0f;
        float blinkInterval = .1f;

        while (elapsed < invincibleTime)
        {
            graphics.enabled = !graphics.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        graphics.enabled = true;


        SetIgnoreLayerCollison(false);

        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Layers.Enemy &&
            collision.gameObject.layer != Layers.EnemyBullet &&
            collision.gameObject.layer != Layers.Hazard)
            return;

        var source = collision.GetComponent<IDamageSource>();
        if (source == null)
            return;

        TakeDamage(source.Damage);
    }

    private void SetIgnoreLayerCollison(bool isIgnore)
    {
        Physics.IgnoreLayerCollision(Layers.Player, Layers.Enemy, isIgnore);
        Physics.IgnoreLayerCollision(Layers.Player, Layers.EnemyBullet, isIgnore);
    }

}
