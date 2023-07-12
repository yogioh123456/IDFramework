using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_DevInfoShow
public class UI_DevInfoShow : UGUICtrl
{
    public UI_DevInfoShow_View selfView;

    public UI_DevInfoShow()
    {
        selfView = new UI_DevInfoShow_View();
        OnCreate(selfView,"UI/Prefabs/UI_DevInfoShow", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_back.AddButtonEvent(() => {
            Game.UI.CloseUI<UI_DevTool>();
            Back();
        });
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {
        base.OpenPanel(data);
        Game.UI.OpenUI<UI_DevTool>();
    }
}