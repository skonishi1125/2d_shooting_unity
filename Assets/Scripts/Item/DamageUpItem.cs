using UnityEngine;

public class DamageUpItem : ItemBase
{
    [SerializeField] private int addDamage = 1;

    protected override void Apply(PlayerStatus status)
    {
        status.AddShotDamage(addDamage);
        base.Apply(status);
    }
}
