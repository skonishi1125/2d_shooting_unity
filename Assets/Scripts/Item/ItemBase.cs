using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float speed = 2f;
    protected float lifetime = 10f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.left * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Layers.Player)
            return;

        var status = collision.GetComponent<PlayerStatus>();
        if (status == null)
        {
            Debug.LogWarning($"ItemBase: 衝突処理でPlayerStatus が見つかりません: {collision.name}");
            return;
        }

        Apply(status);

        Destroy(gameObject);
    }

    // 取得時の影響はアイテム別になるので、継承先で定義する
    //protected abstract void Apply(PlayerStatus status);
    protected virtual void Apply(PlayerStatus status)
    {
        GameManager.Instance.StatusUIHolder.UpdateAll(status);
    }

}
