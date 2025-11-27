using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected float speed = 1f;
    protected float lifetime = 15f;

    [SerializeField] protected ItemType itemType;
    [SerializeField] protected Color itemColor = Color.white;

    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip getSfx;

    public ItemType Type => itemType;
    public Color ItemColor => itemColor;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = itemColor;
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

        AudioSource.PlayClipAtPoint(getSfx, transform.position);

        var status = collision.GetComponent<PlayerStatus>();
        if (status == null)
        {
            Debug.LogWarning($"ItemBase: 衝突処理でPlayerStatus が見つかりません: {collision.name}");
            return;
        }

        Apply(status);

        Destroy(gameObject);
    }


    protected virtual void Apply(PlayerStatus status)
    {
        GameManager.Instance.StatusUIHolder.UpdateAll(status, Type, ItemColor);
    }


}
