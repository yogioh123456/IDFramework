using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_Tips_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_Tips_View : UGUIView
{
    //---------------字段---------------
    public TMP_Text text_info;

    public override void Init(Transform trans) {
        text_info = trans.GetChild(0).GetComponent<TMP_Text>();

    }
}