using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_EventDemo_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_EventDemo_View : UGUIView
{
    //---------------字段---------------
    public TMP_Text text_info;
    public Button btn_send;
    public Button btn_send2;

    public override void Init(Transform trans) {
        text_info = trans.GetChild(0).GetComponent<TMP_Text>();
        btn_send = trans.GetChild(1).GetComponent<Button>();
        btn_send2 = trans.GetChild(2).GetComponent<Button>();

    }
}