using UnityEngine;

public class EnemyG : EnemyBase
{
    [Header("Circle Movement")]
    [SerializeField] private float radius = 2f;        // 円の半径
    [SerializeField] private float angularSpeed = 40f; // 角速度（度/秒）

    private Vector2 center;   // 回転の中心
    private float angle;      // 現在の角度（度）

    private void Start()
    {
        // 出現した場所を中心にする
        center = transform.position;
        angle = 0f;
    }

    protected override void Move()
    {
        // 中心自体を左へ流す
        center.x -= speed * Time.deltaTime;

        // 回転
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        float x = center.x + Mathf.Cos(rad) * radius;
        float y = center.y + Mathf.Sin(rad) * radius;

        rb.MovePosition(new Vector2(x, y));
    }

}
