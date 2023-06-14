using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_Demo_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_Demo_View : UGUIView
{
    //---------------字段---------------
    public Button btn_excel;
    public Button btn_ui;
    public Button btn_eventSystem;
    public Button btn_bindable;
    public Button btn_time;
    public Button btn_audio;
    public Button btn_hotreload;
    public Button btn_resload;
    public Button btn_console;
    public Button btn_bugreport;
    public Button btn_network;

    public override void Init(Transform trans) {
        btn_excel = trans.GetChild(0).GetComponent<Button>();
        btn_ui = trans.GetChild(1).GetComponent<Button>();
        btn_eventSystem = trans.GetChild(2).GetComponent<Button>();
        btn_bindable = trans.GetChild(3).GetComponent<Button>();
        btn_time = trans.GetChild(4).GetComponent<Button>();
        btn_audio = trans.GetChild(5).GetComponent<Button>();
        btn_hotreload = trans.GetChild(6).GetComponent<Button>();
        btn_resload = trans.GetChild(7).GetComponent<Button>();
        btn_console = trans.GetChild(8).GetComponent<Button>();
        btn_bugreport = trans.GetChild(9).GetComponent<Button>();
        btn_network = trans.GetChild(10).GetComponent<Button>();

    }
}