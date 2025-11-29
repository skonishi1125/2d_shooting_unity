using System;
using UnityEngine;

public class StatusUIHolder : MonoBehaviour
{
    [Header("Status Rows")]
    [SerializeField] private StatusRowUI damageRow;
    [SerializeField] private StatusRowUI fireRateRow;
    [SerializeField] private StatusRowUI distanceRow;

    [Header("Item Colors")]
    [SerializeField] private Color damageUpItemColor;
    [SerializeField] private Color fireRateItemColor;
    [SerializeField] private Color distanceItemColor;

    public void SetUpRows(PlayerStatus status)
    {
        damageRow.SetValue(status.DamageLevel, damageUpItemColor);
        fireRateRow.SetValue(status.FireRateLevel, fireRateItemColor);
        distanceRow.SetValue(status.ShotLifeTimeLevel, distanceItemColor);
    }

    public void UpdateAll(PlayerStatus status, ItemType type, Color itemColor)
    {
        switch (type)
        {
            case ItemType.Damage:
                damageRow.SetValue(status.DamageLevel, itemColor);
                break;
            case ItemType.FireRate:
                fireRateRow.SetValue(status.FireRateLevel, itemColor);
                break;
            case ItemType.LifeTime:
                distanceRow.SetValue(status.ShotLifeTimeLevel, itemColor);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    // タイトルに戻ったときやリトライ時、UIの表示を1本ずつに戻す
    public void ResetRows()
    {
        damageRow.ResetValue(damageUpItemColor);
        fireRateRow.ResetValue(fireRateItemColor);
        distanceRow.ResetValue(distanceItemColor);
    }

}
