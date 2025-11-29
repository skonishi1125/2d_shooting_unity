using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Base Status")]
    [SerializeField] private float baseMoveSpeed = 2f;
    [SerializeField] private float baseLifeTime = .5f;
    [SerializeField] private int baseShotDamage = 1;
    [SerializeField] private float baseFireInterval = .5f;

    // リアルタイム時点での各パラメータ
    public float MoveSpeed { get; private set; }
    public float LifeTime { get; private set; }
    public int ShotDamage { get; private set; }
    public float FireInterval { get; private set; }

    // プロパティ
    // 値を公開するが、内部計算して結果を返すやり方
    public int FireRateLevel
    {
        get
        {
            const float baseInterval = 0.5f;
            const float step = 0.03f;
            const int maxLevel = 10;

            // (0.50 - 0.50) / 0.03 = 0 → 1
            // (0.50 - 0.47) / 0.03 = 1 → 2
            // (0.50 - 0.44) / 0.06 = 2 → 3
            float raw = (baseInterval - FireInterval) / step;
            int level = 1 + Mathf.RoundToInt(raw);

            return Mathf.Clamp(level, 1, maxLevel);
        }
    }

    // StatusUIに渡すための整数値
    public int ShotLifeTimeLevel
    {
        get
        {
            const float baseLifetime = 0.5f;
            const float step = 0.1f;
            const int maxLevel = 10;

            // (0.5 - 0.5) / 0.1 = 0 → 1
            // (0.6 - 0.5) / 0.1 = 1 → 2
            // (0.7 - 0.5) / 0.1 = 2 → 3
            float raw = (LifeTime - baseLifetime) / step;
            int level = 1 + Mathf.RoundToInt(raw);

            return Mathf.Clamp(level, 1, maxLevel);
        }
    }

    private void Awake()
    {
        MoveSpeed = baseMoveSpeed;
        LifeTime = baseLifeTime;
        ShotDamage = baseShotDamage;
        FireInterval = baseFireInterval;

        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.InitRunDataIfNeeded(this);
            gm.StatusUIHolder.SetUpRows(this);
        }
    }

    public void SetFromRunData(PlayerRunData data)
    {
        MoveSpeed = data.moveSpeed;
        LifeTime = data.lifetime;
        ShotDamage = data.shotDamage;
        FireInterval = data.fireInterval;
    }

    // 変更後に GameManager にも反映するユーティリティ
    private void SyncToGameManager()
    {
        var gm = GameManager.Instance;
        if (gm == null)
            return;
        gm.SyncRunDataFrom(this);
    }

    // アイテムなどを取ったときこの処理を呼んでパラメータを上げる
    public void AddMoveSpeed(float amount)
    {
        MoveSpeed += amount;
        SyncToGameManager();
        Debug.Log($"MoveSpeed: " + MoveSpeed);
    }

    public void AddLifetime(float amount)
    {
        LifeTime += amount;
        SyncToGameManager();
        Debug.Log($"LifeTime: " + LifeTime);
    }
    public void AddShotDamage(int amount)
    {
        ShotDamage += amount;
        SyncToGameManager();
        Debug.Log($"ShotDamage: " + ShotDamage);
    }

    public void SubFireInterval(float amount)
    {
        FireInterval -= amount;
        SyncToGameManager();
        Debug.Log($"FireInterval: " + FireInterval);
    }


}
