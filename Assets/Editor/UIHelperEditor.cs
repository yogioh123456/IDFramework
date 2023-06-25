using UnityEditor;
using UnityEngine;

public class UIHelperEditor {
    [MenuItem("Tools/UI命名规范")]
    static void ShowUIName() {
        var data = CreateUGUICScriptEditor.uiNameDic;
        string str = "";
        foreach (var one in data) {
            str += one.Key + " : " + one.Value + "\n";
        }
        EditorUtility.DisplayDialog("UI命名规范", str, "确定");
    }
}
