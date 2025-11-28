using UnityEditor;
using UnityEngine;
// test

public class CompileTimerWindow : EditorWindow
{
    // EditorPrefs に保存するキー
    const string KeyIsCompiling = "CompileTimer_IsCompiling";
    const string KeyStartTime = "CompileTimer_StartTime";
    const string KeyLastTime = "CompileTimer_LastTime";

    bool isCompiling;
    float lastCompileTime;

    // メニューからウィンドウを開く
    [MenuItem("Tools/Compile Timer")]
    public static void Open()
    {
        GetWindow<CompileTimerWindow>("Compile Timer");
    }

    void OnEnable()
    {
        // 状態の復元
        isCompiling = EditorPrefs.GetBool(KeyIsCompiling, false);
        lastCompileTime = EditorPrefs.GetFloat(KeyLastTime, 0f);

        // 毎フレーム監視
        EditorApplication.update += UpdateCompileState;
    }

    void OnDisable()
    {
        EditorApplication.update -= UpdateCompileState;
    }

    void UpdateCompileState()
    {
        // いまコンパイル中かどうか
        if (EditorApplication.isCompiling)
        {
            // 「コンパイル開始」の立ち上がりを拾う
            if (!isCompiling)
            {
                isCompiling = true;
                EditorPrefs.SetBool(KeyIsCompiling, true);
                EditorPrefs.SetFloat(KeyStartTime, (float)EditorApplication.timeSinceStartup);
            }
        }
        else
        {
            // 「コンパイル終了」の立ち下がりを拾う
            if (isCompiling)
            {
                isCompiling = false;
                EditorPrefs.SetBool(KeyIsCompiling, false);

                float start = EditorPrefs.GetFloat(KeyStartTime, 0f);
                lastCompileTime = (float)(EditorApplication.timeSinceStartup - start);
                EditorPrefs.SetFloat(KeyLastTime, lastCompileTime);

                Debug.Log($"[CompileTimer] Compile time: {lastCompileTime:F2} sec");

                // ウィンドウの表示を更新
                Repaint();
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Compile Timer", EditorStyles.boldLabel);
        GUILayout.Space(8);

        GUILayout.Label(
            EditorApplication.isCompiling ? "Status: Compiling..." : "Status: Idle"
        );

        GUILayout.Space(4);
        GUILayout.Label($"Last compile: {lastCompileTime:F2} sec");
    }
}
