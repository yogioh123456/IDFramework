using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_Start_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_Start_View : UGUIView
{
    //---------------字段---------------
    public Button btn_host;
    public InputField linput_serverIP;
    public Button btn_client;

    public override void Init(Transform trans) {
        btn_host = trans.GetChild(0).GetComponent<Button>();
        linput_serverIP = trans.GetChild(1).GetComponent<InputField>();
        btn_client = trans.GetChild(2).GetComponent<Button>();

    }
}