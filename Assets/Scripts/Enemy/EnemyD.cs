using UnityEngine;

public class EnemyD : EnemyBase
{
    [Header("Circle Movement")]
    [SerializeField] private float radius = 2f;        // 円の半径
    [SerializeField] private float angularSpeed = 90f; // 角速度（度/秒）

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
        // 時間に応じて角度を増やす（度）
        angle += angularSpeed * Time.deltaTime;

        // 度 → ラジアンに変換
        float rad = angle * Mathf.Deg2Rad;

        // x = 中心 + cosθ * 半径
        // y = 中心 + sinθ * 半径
        float x = center.x + Mathf.Cos(rad) * radius;
        float y = center.y + Mathf.Sin(rad) * radius;

        rb.MovePosition(new Vector2(x, y));
    }

}
