using UnityEngine;

public class SpeedUpItem : ItemBase
{
    [SerializeField] private float addSpeed;
    protected override void Apply(PlayerStatus status)
    {
        status.AddMoveSpeed(addSpeed);

    }
}
