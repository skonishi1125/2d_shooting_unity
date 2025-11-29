using UnityEngine;

public class CyclingItem : ItemBase
{
    [Header("Cycle Settings")]
    [SerializeField] private float switchInterval = 1f;

    [SerializeField] private int addDamage = 1;
    [SerializeField] private float subInterval = .03f;
    [SerializeField] private float addLifetime = .1f;

    [SerializeField] private Color damageColor;
    [SerializeField] private Color intervalColor;
    [SerializeField] private Color lifeTimeColor;

    private ItemType[] cycle = new[]
    {
        ItemType.Damage,
        ItemType.FireRate,
        ItemType.LifeTime
    };

    private int currentIndex = 0;
    private float timer = 0f;

    private void Start()
    {
        currentIndex = 0;
        SetState(cycle[currentIndex]);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= switchInterval)
        {
            timer -= switchInterval;
            currentIndex = (currentIndex + 1) % cycle.Length;
            SetState(cycle[currentIndex]);
        }
    }

    private void SetState(ItemType type)
    {
        itemType = type;

        switch (type)
        {
            case ItemType.Damage:
                itemColor = damageColor;
                break;
            case ItemType.FireRate:
                itemColor = intervalColor;
                break;
            case ItemType.LifeTime:
                itemColor = lifeTimeColor;
                break;
        }

        if (sr != null)
        {
            sr.color = itemColor;
        }
    }

    protected override void Apply(PlayerStatus status)
    {
        // 現在の itemType に応じて強化内容を変える
        switch (itemType)
        {
            case ItemType.Damage:
                status.AddShotDamage(addDamage);
                break;
            case ItemType.FireRate:
                status.SubFireInterval(subInterval);
                break;
            case ItemType.LifeTime:
                status.AddLifetime(addLifetime);
                break;
        }

        base.Apply(status);
    }

}
