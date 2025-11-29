using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpText : MonoBehaviour
{
    [Header("表示先")]
    [SerializeField] private TextMeshProUGUI helpText;

    [Header("New Input Action Reference")]
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference slowMoveAction;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private InputActionReference moveAction;

    private void Start()
    {
        if (helpText == null)
        {
            Debug.LogWarning("HelpText: 未割当のため自動取得します。");
            helpText = GetComponent<TextMeshProUGUI>();
        }

        string text = helpText.text;

        // それぞれのキー名を New Input System から取得
        string moveKeys = GetMoveDisplayString(moveAction.action);
        string attackKey = GetKeyName(attackAction);
        string slowKey = GetKeyName(slowMoveAction);
        string pauseKey = GetKeyName(pauseAction);

        text = text.Replace("{MOVE}", moveKeys);
        text = text.Replace("{ATTACK}", attackKey);
        text = text.Replace("{SLOW}", slowKey);
        text = text.Replace("{PAUSE}", pauseKey);

        helpText.text = text;
    }

    private string GetKeyName(InputActionReference actionRef)
    {
        if (actionRef == null || actionRef.action == null)
            return "?";

        string binding = actionRef.action.GetBindingDisplayString();
        if (binding == "LMB")
            binding = "左クリック";
        if (binding == "RMB")
            binding = "右クリック";

        return binding;
    }

    private string GetMoveDisplayString(InputAction action)
    {
        var bindings = action.bindings;
        var result = new List<string>();

        // 1. バインディング一覧を全部見る
        for (int i = 0; i < bindings.Count; i++)
        {
            var binding = bindings[i];

            // 2. Composite の先頭（"2D Vector" など）だけ処理する
            if (!binding.isComposite)
                continue;

            // 3. 次の4つ（up/down/left/right）をまとめて読む
            var parts = new List<string>();

            // i+1 から、isPartOfComposite が false になるまで読む
            int index = i + 1;
            while (index < bindings.Count && bindings[index].isPartOfComposite)
            {
                string keyName = action.GetBindingDisplayString(index);
                parts.Add(keyName);
                index++;
            }

            // 4. WASD / Arrow の自動判別
            string display;
            if (parts.Contains("W"))
            {
                display = "WASD";
            }
            else if (parts.Exists(k => k.Contains("Up")))
            {
                display = "↑↓←→";
            }
            else
            {
                display = string.Join("/", parts);
            }

            result.Add(display);

            // 5. 読み終わったので、次のループは composite の終わりまで飛ばす
            i = index - 1;
        }

        return string.Join(" / ", result);
    }



}
