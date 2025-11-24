using UnityEngine;
using System.Collections.Generic;


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

    public void SetValue(int value)
    {
        // まず既存を全部消す
        foreach (var b in bars)
            Destroy(b);
        bars.Clear();

        // value の数だけ ■ を生成
        for (int i = 0; i < value; i++)
        {
            var bar = Instantiate(barPrefab, barHolder);
            bars.Add(bar);
        }
    }
}
