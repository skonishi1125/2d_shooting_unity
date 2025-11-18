using UnityEngine;

public class EnemyB : MonoBehaviour
{
    public SpriteRenderer sr { get; private set; }
    private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifeTime = 30f;

    [Header("Movement")]
    [SerializeField] private float baseY; // Sin波のベース座標
    [SerializeField] private float amplitude = .5f; // 上下の幅
    [SerializeField] private float frequency = 2f;  // 上下の速度
    private float time;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // ゆらゆら動くための、ベースの座標
        baseY = transform.position.y;
    }

    private void FixedUpdate()
    {
        time = time + Time.deltaTime;
        float newY = baseY + Mathf.Sin(time * frequency) * amplitude;
        float newX = transform.position.x - speed * Time.deltaTime;
        rb.MovePosition(new Vector2(newX, newY));
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.left * speed;
        Destroy(gameObject, lifeTime);
    }

    public void SetSrColor(Color color)
    {
        sr.color = color;
    }


}
