using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_BindableDemo_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_BindableDemo_View : UGUIView
{
    //---------------字段---------------
    public TMP_Text text_num;
    public Button btn_add;
    public Button btn_reduce;

    public override void Init(Transform trans) {
        text_num = trans.GetChild(1).GetComponent<TMP_Text>();
        btn_add = trans.GetChild(2).GetComponent<Button>();
        btn_reduce = trans.GetChild(3).GetComponent<Button>();

    }
}