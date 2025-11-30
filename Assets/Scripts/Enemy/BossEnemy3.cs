using UnityEngine;

public class BossEnemy3 : BossBase
{
    [Header("Infinity Movement Settings")]
    [SerializeField] private float amplitudeX = 3f;  // 横の広がり
    [SerializeField] private float amplitudeY = 2f;  // 縦の広がり
    [SerializeField] private float amplitudeSpeed = 1.5f;     // 速さ
    [SerializeField] private float startMoveDelay = 1f;

    private Vector2 centerPos;
    private float t;         // 時間カウンタ
    private float delayTimer;

    protected override void OnEnable()
    {
        base.OnEnable();
        t = 0f;
        delayTimer = startMoveDelay;
        centerPos = rb.position;
    }

    protected override void EnterMove()
    {
        rb.linearVelocity = Vector2.left;
    }

    protected override void HoldMove()
    {
        if (centerFixed)
            return;

        // 中心に固定
        centerPos = rb.position;
        rb.linearVelocity = Vector2.zero;
        centerFixed = true;
        delayTimer = startMoveDelay;

        if (IsInvincible)
            SetInvincible(false);
    }

    protected override void Move()
    {
        if (delayTimer > 0f)
        {
            delayTimer -= Time.fixedDeltaTime;
            return;
        }

        t += Time.fixedDeltaTime * amplitudeSpeed;

        float x = amplitudeX * Mathf.Sin(t);
        float y = amplitudeY * Mathf.Sin(t) * Mathf.Cos(t);

        Vector2 target = centerPos + new Vector2(x, y);
        rb.MovePosition(target);
    }
}
