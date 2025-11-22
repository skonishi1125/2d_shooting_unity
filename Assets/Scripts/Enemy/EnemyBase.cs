using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageSource
{
    protected Rigidbody2D rb;

    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float lifeTime = 10f;
    [SerializeField] private int contactDamage = 1;

    public bool IsInvincible { get; protected set; } = false;
    public bool CanShoot { get; protected set; } = true;

    public int Damage => contactDamage;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    // 動きはこのクラスの継承先で定義する
    protected abstract void Move();

    public void SetInvincible(bool value) => IsInvincible = value;
    public void SetCanShoot(bool value) => CanShoot = value;
}
