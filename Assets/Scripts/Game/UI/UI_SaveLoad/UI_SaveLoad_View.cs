using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_SaveLoad_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_SaveLoad_View : UGUIView
{
    //---------------字段---------------
    public Button btn_save;
    public Button btn_load;
    public TMP_InputField input_info;
    public TMP_Text text_loadInfo;
    public TMP_Text text_load;
    public TMP_Text text_save;

    public override void Init(Transform trans) {
        btn_save = trans.GetChild(0).GetComponent<Button>();
        btn_load = trans.GetChild(1).GetComponent<Button>();
        input_info = trans.GetChild(3).GetComponent<TMP_InputField>();
        text_loadInfo = trans.GetChild(4).GetComponent<TMP_Text>();
        text_load = trans.GetChild(5).GetComponent<TMP_Text>();
        text_save = trans.GetChild(6).GetComponent<TMP_Text>();

    }
}