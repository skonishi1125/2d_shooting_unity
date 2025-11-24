using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class StatusRowUI : MonoBehaviour
{
    [SerializeField] private Transform barHolder;
    [SerializeField] private GameObject barPrefab;

    private readonly List<GameObject> bars = new List<GameObject>();

    private void Awake()
    {
        if (barHolder == null || barPrefab == null)
        {
            Debug.LogWarning("StatusRowUI: Objectの割り当てができていません。");
        }
    }

    public void SetValue(int value, Color color)
    {
        // 既存の■を全て消して、valueの数だけ生成し直す
        foreach (var b in bars)
            Destroy(b);
        bars.Clear();

        for (int i = 0; i < value; i++)
        {
            var bar = Instantiate(barPrefab, barHolder);
            var img = bar.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.color = color;
            }
            bars.Add(bar);
        }
    }
}
