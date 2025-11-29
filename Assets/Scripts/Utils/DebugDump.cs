using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Laravel の dd() っぽく中身をダンプするヘルパ(C# だとユーティリティと呼ぶ).
public class DebugDump
{
    // JsonUtility で何でも包むためのラッパー
    [System.Serializable]
    private class Wrapper<T>
    {
        public T value;
        public Wrapper(T v) { value = v; }
    }

    // なんでも JSON でダンプする（基本形）
    public static void Dump<T>(T obj, string label = null, bool pause = false)
    {
        if (obj == null)
        {
            Debug.Log(label != null ? $"{label}: null" : "null");
            if (pause) Debug.Break();
            return;
        }

        var wrapper = new Wrapper<T>(obj);
        string json = JsonUtility.ToJson(wrapper, true); // 整形出力

        if (string.IsNullOrEmpty(label))
            label = typeof(T).Name;

        Debug.Log($"{label} = {json}");

        if (pause)
            Debug.Break(); // エディタ上で一時停止
    }

    // IEnumerable 用の簡易ダンプ（List, 配列など）
    public static void DumpEnumerable<T>(IEnumerable<T> enumerable, string label = null, bool pause = false)
    {
        if (enumerable == null)
        {
            Debug.Log(label != null ? $"{label}: null" : "null");
            if (pause) Debug.Break();
            return;
        }

        var sb = new StringBuilder();
        int i = 0;
        foreach (var item in enumerable)
        {
            sb.AppendLine($"[{i++}] {item}");
        }

        if (string.IsNullOrEmpty(label))
            label = $"IEnumerable<{typeof(T).Name}>";

        Debug.Log($"{label}\n{sb}");

        if (pause)
            Debug.Break();
    }
}
