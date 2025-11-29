using System.Collections.Generic;
using UnityEngine;
using static EnemyShooter;

[System.Serializable]
public class BossPattern
{
    public ShootPattern pattern;
    public float duration;      // パターン継続時間
    public float fireInterval;  // パターン中の発射間隔
}

public abstract class BossBase : EnemyBase
{
    [Header("Bullet Pattern Settings")]
    [SerializeField] protected EnemyShooter shooter;
    [SerializeField] protected List<BossPattern> patterns;
    protected int currentPatternIndex = 0;
    protected float patternTimer;

    [Header("First Move Settings")]
    [SerializeField] protected float enterDuration = 6f; // 前に出てくる時間
    [SerializeField] protected float waitDuration = 3f;  // 止まっている時間
    protected float firstMoveTimer;
    protected bool centerFixed = false;

    protected override void Awake()
    {
        base.Awake();
        if (shooter == null)
            shooter = GetComponent<EnemyShooter>();
    }

    protected override void OnEnable()
    {
        // base.OnEnable() は呼ばない（lifeTime制御しない）
        firstMoveTimer = enterDuration + waitDuration;

        SetInvincible(true);
        SetCanShoot(false);

        centerFixed = false;
    }

    protected override void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // 登場演出フェーズ
        if (UpdateEnterPhase(dt))
            return;

        // 戦闘開始設定
        TryStartBattle();

        // パターン切り替え
        UpdatePatternCycle(dt);

        // 通常の移動処理（継承先で実装）
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

    protected virtual void TryStartBattle()
    {
        if (CanShoot)
            return;

        SetCanShoot(true);
        currentPatternIndex = 0;
        ApplyCurrentPattern();
    }

    protected virtual void UpdatePatternCycle(float dt)
    {
        if (patterns == null || patterns.Count == 0)
        {
            Debug.LogWarning("BossBase: パターンが割り当てられていません。");
            return;
        }

        patternTimer -= dt;
        if (patternTimer > 0f)
            return;

        currentPatternIndex++;
        if (currentPatternIndex >= patterns.Count)
            currentPatternIndex = 0;

        ApplyCurrentPattern();
    }

    protected virtual void ApplyCurrentPattern()
    {
        var p = patterns[currentPatternIndex];

        shooter.SetPattern(p.pattern);
        shooter.SetFireInterval(p.fireInterval);

        patternTimer = p.duration;
    }

    // ここは具体ボスごとに実装させる
    protected abstract void EnterMove();
    protected abstract void HoldMove();

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

}
