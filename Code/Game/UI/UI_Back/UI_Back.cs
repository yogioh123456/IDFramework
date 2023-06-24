using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_Back
public class UI_Back : UGUICtrl
{
    public UI_Back_View selfView;

    public UI_Back()
    {
        selfView = new UI_Back_View();
        OnCreate(selfView,"UI/Prefabs/UI_Back", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_back.AddButtonEvent(() =>
        {
            CloseSelfPanel();
            Back();
        });
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {
        base.OpenPanel(data);
        
    }
}