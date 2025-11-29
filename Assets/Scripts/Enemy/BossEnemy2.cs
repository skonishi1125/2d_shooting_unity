using UnityEngine;

public class BossEnemy2 : BossBase
{
    [Header("Movement Settings")]
    [SerializeField] private float horizontalRange = 3f;
    [SerializeField] private float verticalRange = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float startMoveDelay = 1f;

    private Vector2 centerPos;
    private Vector2[] waypoints;// 中心 → 上 → 右 → 下 → 左 →中心...
    private int currentIndex;
    private float delayTimer;

    protected override void OnEnable()
    {
        base.OnEnable();

        centerPos = rb.position;
        delayTimer = 0f;
        waypoints = null;
        currentIndex = 0;
    }

    protected override void EnterMove()
    {
        rb.linearVelocity = Vector2.left;
    }

    protected override void HoldMove()
    {
        if (centerFixed)
            return;

        // 「ここが中心」として記録
        centerPos = rb.position;
        rb.linearVelocity = Vector2.zero;
        centerFixed = true;

        // 中心に出てからしばらく待ってから動くようにする
        delayTimer = startMoveDelay;

        // 四角形（＋中心）ルートを作成
        // 0: 中心, 1: 上, 2: 右, 3: 下, 4: 左
        waypoints = new Vector2[5];
        waypoints[0] = centerPos;
        waypoints[1] = centerPos + new Vector2(0f, verticalRange);
        waypoints[2] = centerPos + new Vector2(horizontalRange, 0f);
        waypoints[3] = centerPos + new Vector2(0f, -verticalRange);
        waypoints[4] = centerPos + new Vector2(-horizontalRange, 0f);

        currentIndex = 0;

        if (IsInvincible)
            SetInvincible(false);
    }

    protected override void Move()
    {
        // まだ「中心で待機中」なら動かない
        if (delayTimer > 0f)
        {
            delayTimer -= Time.fixedDeltaTime;
            return;
        }

        if (waypoints == null || waypoints.Length == 0)
            return;

        // 次の目標地点
        Vector2 target = waypoints[currentIndex];

        // 四角形の辺に沿って等速移動
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            target,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        // 目標地点にほぼ到達したら、次の頂点へ
        if (Vector2.Distance(newPos, target) < 0.01f)
        {
            currentIndex++;

            // 回り続けて上に遷移させる
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 1; // 再び「上」に戻る（中心は最初だけ）
            }
        }
    }
}
