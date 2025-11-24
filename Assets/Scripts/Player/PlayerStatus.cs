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
    public float LifeTime {  get; private set; }
    public int ShotDamage { get; private set; }
    public float FireInterval { get; private set; }

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
