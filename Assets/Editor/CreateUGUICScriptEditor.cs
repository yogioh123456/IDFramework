using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class CreateUGUICScriptEditor : MonoBehaviour
{
    private static string selectUIName;
    private static string uiFieldStr;
    private static string uiMethodStr;

    public static Dictionary<string, string> uiNameDic = new Dictionary<string, string>()
    {
        {"text_", "TMP_Text"},
        {"btn_", "Button"},
        {"tog_", "Toggle"},
        {"input_", "TMP_InputField"},
        {"img_", "Image"},
        {"slider_", "Slider"},
        {"pool_", "PoolView"},
        {"trans_", "Transform"},
    };
    
    [MenuItem("GameObject/★ 生成UI(F7) _F7", false, 1)]
    static void CreateUIScripts()
    {
        uiFieldStr = "";
        uiMethodStr = "";
        selectUIName = Selection.gameObjects[0].transform.name;
        CreateUIView();
        CreateUIControl();
        Debug.Log("创建UI脚本完成!");
    }

    /// <summary>
    /// 自动创建View脚本并且绑定
    /// </summary>
    private static void CreateUIView()
    {
        var selectUI = Selection.gameObjects[0].transform;
        CheckUIObject(selectUI, "", true);
        CreateUIScript("_View.cs", AutoGetPanelComp);
    }

    private static void CheckUIObject(Transform selectUI, string findStr, bool b)
    {
        for (int i = 0; i < selectUI.childCount; i++)
        {
            if (b)
            {
                findStr = "";
            }
            string tempStr = findStr;

            //判断前缀，检测命名规则
            tempStr += $".GetChild({i})";
            CheckHeadName(selectUI.GetChild(i), tempStr);
            if (selectUI.GetChild(i).childCount > 0)
            {
                CheckUIObject(selectUI.GetChild(i), tempStr, false);
            }
        }
    }

    private static void CheckHeadName(Transform ui, string findStr)
    {
        foreach (var one in uiNameDic)
        {
            if (ui.name.StartsWith(one.Key))
            {
                uiFieldStr += $"    public {one.Value} {ui.name};\n";
                findStr = $"{ui.name} = trans{findStr}";
                uiMethodStr += $"        {findStr}.GetComponent<{one.Value}>();\n";
            }
        }
    }

    private static void CreateUIControl()
    {
        CreateUIScript(".cs", AutoGenControlScript);
    }


    private delegate string AutonGenScript(string selectName);

    private static void CreateUIScript(string csSuffix, AutonGenScript ac)
    {
        string selectName = selectUIName;
        string path = Define.UIScriptsPath + selectName;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //control脚本只会创建一次
        if (csSuffix.Equals(".cs"))
        {
            bool fileExists = File.Exists(path + "/" + selectName + csSuffix);
            if (fileExists)
            {
                return;
            }
        }
        File.WriteAllText(path + "/" + selectName + csSuffix, ac(selectName), new UTF8Encoding(false));
    }


    //-------------------生成Panel-View层脚本-----------------------
    static string AutoGetPanelComp(string className)
    {
        string content = "";

        string head = string.Format(
            @"using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 {0}
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class {0} : UGUIView
{{
    //---------------字段---------------
{1}
    public override void Init(Transform trans) {{
{2}
    }}
}}", className + "_View", uiFieldStr, uiMethodStr);

        content += head;
        return content;
    }


    //-------------------生成Panel-ctrl层脚本------------------------
    static string AutoGenControlScript(string className)
    {
        string content = "";
        string head = string.Format(@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: {0}
public class {0} : UGUICtrl
{{
    public {1} selfView;

    public {0}()
    {{
        selfView = new {1}();
        OnCreate(selfView,""UI/Prefabs/{2}"", GetType());
    }}

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {{
        //------------------按钮添加事件-----------------
        
    }}

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {{
        base.OpenPanel(data);
        
    }}
}}", className, className + "_View", className);
        content += head;
        return content;
    }

    //自动生成按钮事件
    static string GetPrefabName(string _name)
    {
        return _name.Substring(0, 4).ToLower() + _name.Substring(4, _name.Length - 4);
    }
}