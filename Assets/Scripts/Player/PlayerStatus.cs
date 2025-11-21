using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Base Status")]
    [SerializeField] private float baseMoveSpeed = 2f;
    [SerializeField] private int baseShotDamage = 1;
    [SerializeField] private float baseFireInterval = .5f;

    // リアルタイム時点での各パラメータ
    public float MoveSpeed { get; private set; }
    public int ShotDamage { get; private set; }
    public float FireInterval { get; private set; }

    private void Awake()
    {
        MoveSpeed = baseMoveSpeed;
        ShotDamage = baseShotDamage;
        FireInterval = baseFireInterval;
    }

    // アイテムなどを取ったときこの処理を呼んでパラメータを上げる
    public void AddMoveSpeed(float amount)
    {
        MoveSpeed += amount;
        Debug.Log(MoveSpeed);
    }
    public void AddShotDamage(int amount)
    {
        ShotDamage += amount;
        Debug.Log(ShotDamage);
    }

    public void SubFireInterval(float amount)
    {
        FireInterval -= amount;
        Debug.Log(FireInterval);
    }

}
