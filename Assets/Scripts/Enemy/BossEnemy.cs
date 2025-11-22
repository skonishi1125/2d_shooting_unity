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
        float dt = Time.fixedDeltaTime;

        // 登場演出フェーズ trueが返る限り, 開始設定まで進ませない
        if (UpdateEnterPhase(dt))
            return;

        // 演出が終わったあと
        // 戦闘開始設定(弾を打つ, パターン割当て)
        TryStartBattle();

        // パターンチェック(パターンの秒数が経過したら、新しく割り当てる)
        UpdatePatternCycle(dt);

        base.FixedUpdate();
    }

    private bool UpdateEnterPhase(float dt)
    {
        if (firstMoveTimer <= 0f)
            return false; // すでに演出終了

        firstMoveTimer -= dt;

        if (firstMoveTimer > waitDuration)
        {
            // 画面外から前に出てくるフェーズ
            EnterMove();
        }
        else
        {
            // 待機フェーズ
            HoldMove();
        }

        // まだ演出中なので、このフレームの残り処理はスキップ
        return true;
    }

    private void TryStartBattle()
    {
        if (CanShoot)
            return;

        SetCanShoot(true);
        currentPatternIndex = 0;
        ApplyCurrentPattern();
    }

    private void UpdatePatternCycle(float dt)
    {
        if (patterns == null || patterns.Count == 0)
        {
            Debug.LogWarning("BossEnemy: パターンが割り当てられていません。");
            return;
        }

        // パターンが継続中の間は、設定処理まで進ませずreturnさせる
        patternTimer -= dt;
        if (patternTimer > 0f)
            return;

        currentPatternIndex++;
        if (currentPatternIndex >= patterns.Count)
            currentPatternIndex = 0;

        ApplyCurrentPattern();
    }


    private void EnterMove()
    {
        Debug.Log("EnterMove");
        rb.linearVelocity = Vector2.left;
    }

    private void HoldMove()
    {
        if (centerFixed)
            return;

        Debug.Log("HoldMove Setting");

        // ゆらゆら動く為の中心軸を取得し、
        // 多少殴れるようなお得感を出すために無敵解除しておく
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
