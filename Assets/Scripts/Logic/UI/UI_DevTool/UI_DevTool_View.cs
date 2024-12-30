using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI View层 UI_DevTool_View
/// 类型 Panel
/// 注意：本段代码由系统自动生成
/// </summary>
public class UI_DevTool_View : UGUIView
{
    //---------------字段---------------
    public Transform trans_point;
    public PoolView pool_frame;

    public override void Init(Transform trans) {
        trans_point = trans.GetChild(1).GetComponent<Transform>();
        pool_frame = trans.GetChild(2).GetComponent<PoolView>();

    }
}