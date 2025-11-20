using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Base Status")]
    [SerializeField] private float baseMoveSpeed = 3f;
    [SerializeField] private int baseShotDamage = 1;

    // リアルタイム時点での各パラメータ
    public float MoveSpeed { get; private set; }
     public int ShotDamage { get; private set; }

    private void Awake()
    {
        MoveSpeed = baseMoveSpeed;
        ShotDamage = baseShotDamage;
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


}
