using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    [SerializeField] private GameObject lifeIconPrefab;
    [SerializeField] private float iconSpacing = .4f;
    private List<Transform> icons; // icons[i].gameObject.SetActive(true/false) などで弄れるようにする

    private int lastLife;

    private void Awake()
    {
        if (health == null || lifeIconPrefab == null)
            Debug.LogWarning("LifeUI: PlayerHealth もしくは lifeIconPrefabが参照できません");

        icons = new List<Transform>();

    }

    private void Start()
    {
        int maxLife = health.maxLife;
        for (int i = 0; i < maxLife; i++)
        {
            // transformはどこにも定義されていないが、MonoBehaviourが所持しているプロパティ
            var iconObj = Instantiate(lifeIconPrefab, transform);
            var iconTransform = iconObj.transform;
            iconTransform.localPosition = new Vector3(i * iconSpacing, 0f, 0f);
            icons.Add(iconTransform);
        }

        lastLife = health.CurrentLife;
    }

    private void Update()
    {
        // health側でTakeDamage()などでライフが減ったとき、反応させる
        if (health.CurrentLife != lastLife)
        {
            RefreshIcons(health.CurrentLife);
            lastLife = health.CurrentLife;
        }
    }

    private void RefreshIcons(int life)
    {
        // 例えばライフが3 -> 2 になるとき、
        // icons[2](ライフ3つめ）が、2 < 2 となり、falseで非表示となる
        for (int i = 0; i < icons.Count; i++)
            icons[i].gameObject.SetActive(i < life);
    }
}
