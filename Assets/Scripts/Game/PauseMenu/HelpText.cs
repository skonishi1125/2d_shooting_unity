using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpText : MonoBehaviour
{
    [Header("表示先")]
    [SerializeField] private TextMeshProUGUI helpText;

    [Header("割当てる Input Actions")]
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

        return actionRef.action.GetBindingDisplayString();
    }

    private string GetMoveDisplayString(InputAction action)
    {
        var bindings = action.bindings;
        //Debug.Log(bindings);
        var list = new System.Collections.Generic.List<string>();

        for (int i = 0; i < bindings.Count; i++)
        {
            var b = bindings[i];

            // コンポジットの先頭（例：2D Vector）だけを見る
            if (!b.isComposite)
                continue;

            // このコンポジットの4方向をまとめて読む
            string up = action.GetBindingDisplayString(i + 1);
            string down = action.GetBindingDisplayString(i + 2);
            string left = action.GetBindingDisplayString(i + 3);
            string right = action.GetBindingDisplayString(i + 4);

            // 今回は簡単に「Upキー」だけで表現を決める
            // Wなら WASD、Arrow なら 方向キー、といった感じで
            if (up == "W")
            {
                list.Add("WASD");
            }
            else if (up.Contains("Arrow"))
            {
                list.Add("↑↓←→");
            }
            else
            {
                // 汎用的に書くならこういう形でも良い
                list.Add($"{up}/{down}/{left}/{right}");
            }

            // 4パーツを読み終わったので i を先に進める
            i += 4;
        }

        // 複数スキームある場合は「WASD / ↑↓←→」のように連結
        return string.Join(" / ", list);
    }





}
