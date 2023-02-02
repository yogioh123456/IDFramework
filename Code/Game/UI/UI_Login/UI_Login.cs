using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_Login
public class UI_Login : UGUICtrl
{
    public UI_Login_View selfView;

    public UI_Login()
    {
        selfView = new UI_Login_View();
        OnCreate(selfView,"UI/Prefabs/ui_login", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_start.AddButtonEvent(Test);
    }

    private void Test() {
        int x = 10;
        var data = Game.UI;
        Debug.Log("测试按钮" + 10);
    }
    
    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {
        base.OpenPanel(data);
        
    }
}