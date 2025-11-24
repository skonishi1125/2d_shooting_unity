using UnityEngine;

public class StatusUIHolder : MonoBehaviour
{
    [Header("Status Rows")]
    [SerializeField] private StatusRowUI damageRow;
    [SerializeField] private StatusRowUI fireRateRow;
    [SerializeField] private StatusRowUI distanceRow;

    // アイテムの色が変わるとこちらも変えないといけないので、
    // 本当はアイテム側から取得したほうが良い。暫定
    [Header("Item Colors")]
    [SerializeField] private Color damageUpItemColor;
    [SerializeField] private Color fireRateItemColor;
    [SerializeField] private Color distanceItemColor;

    public void SetUpRows(PlayerStatus status)
    {
        damageRow.SetValue(status.ShotDamage, damageUpItemColor);
        fireRateRow.SetValue(status.FireRateLevel, fireRateItemColor);
        distanceRow.SetValue(status.ShotLifeTimeLevel, distanceItemColor);
    }

    public void UpdateAll(PlayerStatus status, ItemType type, Color itemColor)
    {
        switch (type)
        {
            case ItemType.Damage:
                damageRow.SetValue(status.ShotDamage, itemColor);
                break;
            case ItemType.FireRate:
                fireRateRow.SetValue(status.FireRateLevel, itemColor);
                break;
            case ItemType.LifeTime:
                distanceRow.SetValue(status.ShotLifeTimeLevel, itemColor);
                break;
        }
    }
}
