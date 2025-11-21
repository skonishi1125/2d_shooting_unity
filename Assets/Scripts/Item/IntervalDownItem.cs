using UnityEngine;

public class IntervalDownItem : ItemBase
{
    [SerializeField] private float subInterval = .05f;

    protected override void Apply(PlayerStatus status)
    {
        status.SubFireInterval(subInterval);
    }
}
