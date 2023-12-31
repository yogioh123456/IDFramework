using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_ExcelDemo_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_ExcelDemo_View : UGUIView
{
    //---------------字段---------------
    public TMP_Text text_content;
    public Button btn_load;

    public override void Init(Transform trans) {
        text_content = trans.GetChild(1).GetComponent<TMP_Text>();
        btn_load = trans.GetChild(2).GetComponent<Button>();

    }
}