using UnityEngine;

public class StatusUIHolder : MonoBehaviour
{
    [SerializeField] private StatusRowUI damageRow;
    [SerializeField] private StatusRowUI fireRateRow;
    [SerializeField] private StatusRowUI distanceRow;

    public void UpdateAll(PlayerStatus status)
    {
        damageRow.SetValue(status.ShotDamage);
        fireRateRow.SetValue(status.FireRateLevel);
        distanceRow.SetValue(status.ShotLifeTimeLevel);
    }

}
