using UnityEngine;

public class EnemyB : EnemyBase
{
    [Header("Movement")]
    private float time;
    private float baseY; // Sin波のベース座標
    [SerializeField] private float amplitude = .5f; // 上下の幅
    [SerializeField] private float frequency = 2f;  // 上下の速度

    private void Start()
    {
        // ゆらゆら動くための、ベースの座標
        baseY = transform.position.y;
    }

    protected override void Move()
    {
        time += Time.deltaTime;
        float newY = baseY + Mathf.Sin(time * frequency) * amplitude;
        float newX = transform.position.x - speed * Time.deltaTime;
        rb.MovePosition(new Vector2(newX, newY));
    }

}
