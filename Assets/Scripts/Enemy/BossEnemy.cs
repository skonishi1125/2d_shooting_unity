using System.Collections.Generic;
using UnityEngine;

using static EnemyShooter;

[System.Serializable]
public class BossPattern
{
    public ShootPattern pattern; // 発射パターン
    public float duration; // パターン継続時間
    public float fireInterval; // パターン中の発射間隔
}

public class BossEnemy : EnemyBase
{
    [Header("Bullet Pattern Settings")]
    [SerializeField] private EnemyShooter shooter;
    [SerializeField] private List<BossPattern> patterns;
    private int currentPatternIndex = 0;
    private float patternTimer;

    [Header("Movement Settings")]
    [SerializeField] private float moveRange = 3f;   // 左右の振れ幅
    [SerializeField] private float moveSpeed = 1f;   // 揺れる速さ
    private Vector2 centerPos;   // 揺れの中心位置
    private float moveTimer;     // 経過時間

    [Header("First Move Settings")]
    [SerializeField] private float enterDuration = 6f; // 前に出てくる時間
    [SerializeField] private float waitDuration = 3f; // 止まっている時間
    private float firstMoveTimer;
    private bool centerFixed = false;

    protected override void Awake()
    {
        base.Awake();
        shooter = GetComponent<EnemyShooter>();
    }

    protected override void OnEnable()
    {
        // base.OnEnable()は呼ばない（lifetime制御しない）

        firstMoveTimer = enterDuration + waitDuration;

        // 無敵フラグ, 弾を打たない設定ON
        SetInvincible(true);
        SetCanShoot(false);

        centerPos = rb.position;
        moveTimer = 0f;
        centerFixed = false;
    }

    protected override void FixedUpdate()
    {
        var time = Time.deltaTime;

        // 最初の動作演出
        if (firstMoveTimer > 0f)
        {
            firstMoveTimer -= time;

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

                    if (IsInvincible)
                    {
                        SetInvincible(false); // 無敵解除。多少殴れるお得な隙間を用意しておく
                    }
                }
            }

            return;
        }

        // 登場演出が全て終わると、弾を打ち始める
        if (!CanShoot)
        {
            SetCanShoot(true); // 弾を打てるようにする
            currentPatternIndex = 0;
            ApplyCurrentPattern();
        }

        // 登場演出終了後の通常動作パート
        if (firstMoveTimer <= 0f)
        {
            // 指定のパターン時間が終わったら、次のパターンに値を渡して再度時間をセットする
            patternTimer -= Time.deltaTime;
            if (patternTimer <= 0f)
            {
                currentPatternIndex++;
                if (currentPatternIndex >= patterns.Count)
                    currentPatternIndex = 0;
                ApplyCurrentPattern();
            }
        }

        base.FixedUpdate();
    }

    private void EnterMove()
    {
        rb.linearVelocity = Vector2.left;
    }

    protected override void Move()
    {
        moveTimer += Time.fixedDeltaTime;

        // -1〜1 で左右に振れる値
        float offsetY = Mathf.Sin(moveTimer * moveSpeed) * moveRange;
        // 中心位置からのオフセットを反映した目標位置
        Vector2 targetPos = new Vector2(centerPos.x, centerPos.y + offsetY);

        rb.MovePosition(targetPos);
    }

    // patternsで書かれた設定をShooterに適用する
    private void ApplyCurrentPattern()
    {
        var p = patterns[currentPatternIndex];

        shooter.SetPattern(p.pattern);
        shooter.SetFireInterval(p.fireInterval);

        patternTimer = p.duration;
    }

}
