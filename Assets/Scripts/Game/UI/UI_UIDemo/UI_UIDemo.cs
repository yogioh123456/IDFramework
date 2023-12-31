using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_UIDemo
public class UI_UIDemo : UGUICtrl
{
    public UI_UIDemo_View selfView;

    public UI_UIDemo()
    {
        selfView = new UI_UIDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_UIDemo", GetType());
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