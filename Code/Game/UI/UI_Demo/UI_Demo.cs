using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_Demo
public class UI_Demo : UGUICtrl
{
    public UI_Demo_View selfView;

    public UI_Demo()
    {
        selfView = new UI_Demo_View();
        OnCreate(selfView,"UI/Prefabs/UI_Demo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {
        base.OpenPanel(data);
        
    }
}