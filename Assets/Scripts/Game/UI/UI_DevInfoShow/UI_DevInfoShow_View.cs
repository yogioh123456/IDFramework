using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_DevInfoShow_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_DevInfoShow_View : UGUIView
{
    //---------------字段---------------
    public Button btn_back;

    public override void Init(Transform trans) {
        btn_back = trans.GetChild(1).GetComponent<Button>();

    }
}