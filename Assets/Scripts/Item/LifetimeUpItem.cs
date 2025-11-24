using UnityEngine;

public class LifetimeUpItem : ItemBase
{
    [SerializeField] private float addLifetime = .1f;

    protected override void Apply(PlayerStatus status)
    {
        status.AddLifetime(addLifetime);
    }
}
