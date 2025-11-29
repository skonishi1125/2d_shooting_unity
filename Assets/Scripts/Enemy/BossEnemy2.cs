using UnityEngine;

public class BossEnemy2 : BossBase
{
    [Header("Movement Settings")]
    [SerializeField] private float moveRange = 3f;
    [SerializeField] private float moveSpeed = 1f;

    private Vector2 centerPos;
    private float moveTimer;

    protected override void OnEnable()
    {
        base.OnEnable();

        centerPos = rb.position;
        moveTimer = 0f;
    }

    protected override void EnterMove()
    {
        rb.linearVelocity = Vector2.left;
    }

    protected override void HoldMove()
    {
        if (centerFixed)
            return;

        centerPos = rb.position;
        moveTimer = 0f;
        rb.linearVelocity = Vector2.zero;
        centerFixed = true;

        if (IsInvincible)
            SetInvincible(false);
    }

    protected override void Move()
    {
        moveTimer += Time.fixedDeltaTime;

        float offsetY = Mathf.Sin(moveTimer * moveSpeed) * moveRange;
        Vector2 targetPos = new Vector2(centerPos.x, centerPos.y + offsetY);
        rb.MovePosition(targetPos);
    }
}
