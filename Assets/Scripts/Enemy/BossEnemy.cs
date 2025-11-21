using UnityEngine;

public class BossEnemy : EnemyBase
{
    [SerializeField] private float moveRange = 3f;   // 左右の振れ幅
    [SerializeField] private float moveSpeed = 1f;   // 揺れる速さ
    private Vector2 centerPos;   // 揺れの中心位置
    private float moveTimer;     // 経過時間

    [SerializeField] private float enterDuration = 6f; // 前に出てくる時間
    [SerializeField] private float waitDuration = 3f; // 止まっている時間
    private float firstMoveTimer;
    private bool centerFixed = false;

    protected override void OnEnable()
    {
        firstMoveTimer = enterDuration + waitDuration;

        centerPos = rb.position;
        moveTimer = 0f;
        centerFixed = false;
    }

    protected override void FixedUpdate()
    {
        // 最初の動作演出
        if (firstMoveTimer > 0f)
        {
            firstMoveTimer -= Time.fixedDeltaTime;

            // 出てきて、止まるまで
            if (firstMoveTimer > waitDuration)
            {
                EnterMove();
            }
            else
            {
                // waitDuration
                // 初回のみ停止位置を揺れの中心に確定
                if (!centerFixed)
                {
                    centerPos = rb.position;
                    moveTimer = 0f;
                    rb.linearVelocity = Vector2.zero;
                    centerFixed = true;
                }
            }

            return;
        }

        base.FixedUpdate();
    }

    private void EnterMove()
    {
        rb.linearVelocity = Vector2.left;
    }

    protected override void Move()
    {
        // 経過時間を増やす
        moveTimer += Time.fixedDeltaTime;

        // -1〜1 で左右に振れる値
        float offsetY = Mathf.Sin(moveTimer * moveSpeed) * moveRange;

        // 中心位置からのオフセットを反映した目標位置
        Vector2 targetPos = new Vector2(centerPos.x, centerPos.y + offsetY);

        // Rigidbody2D を通して位置を動かす
        rb.MovePosition(targetPos);
    }

    // TODO: 無敵有効化 / 無敵無効化を作りたい



}
