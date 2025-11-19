using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private SpriteRenderer sr;

    private float currentLife;
    [SerializeField] private float maxLife = 3f;
    [SerializeField] private bool isInvincible; // 無敵フラグ
    [SerializeField] private float invincibleTime = 1f;
    private Coroutine startInvinsibleCo;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

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
        Physics.IgnoreLayerCollision(Layers.Player, Layers.Enemy, true);
        isInvincible = true;

        float elapsed = 0f;
        float blinkInterval = .1f;

        while (elapsed < invincibleTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        sr.enabled = true;
        Physics.IgnoreLayerCollision(Layers.Player, Layers.Enemy, false);
        isInvincible = false;
    }

}
