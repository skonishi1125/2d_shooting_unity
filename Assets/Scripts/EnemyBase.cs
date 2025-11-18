using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float lifeTime = 10f;

    protected Rigidbody2D rb;

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
}
