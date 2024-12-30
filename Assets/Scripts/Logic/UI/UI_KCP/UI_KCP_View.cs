using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_KCP_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_KCP_View : UGUIView
{
    //---------------字段---------------
    public TMP_InputField input_address;
    public Button btn_server;
    public Button btn_client;
    public Button btn_serversend;
    public Button btn_clientsend;

    public override void Init(Transform trans) {
        input_address = trans.GetChild(0).GetComponent<TMP_InputField>();
        btn_server = trans.GetChild(1).GetComponent<Button>();
        btn_client = trans.GetChild(2).GetComponent<Button>();
        btn_serversend = trans.GetChild(3).GetComponent<Button>();
        btn_clientsend = trans.GetChild(4).GetComponent<Button>();

    }
}