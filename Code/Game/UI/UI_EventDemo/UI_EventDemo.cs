using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_EventDemo
public class UI_EventDemo : UGUICtrl
{
    public UI_EventDemo_View selfView;

    public UI_EventDemo()
    {
        selfView = new UI_EventDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_EventDemo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_send.AddButtonEvent(() => {
            Game.Event.Dispatch("TestShow1", "Hello World!");
        });
        
        selfView.btn_send2.AddButtonEvent(() => {
            Game.Event.Dispatch("TestShow2", "Hello World!", 22222);
        });
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data) 
    {
        base.OpenPanel(data);
        //打开界面时，自动监听所以打了[EventMsg]的方法
        this.RegisterEvent();
    }

    protected override void ClosePanel() 
    {
        //关闭界面时，自动移除监听所以打了[EventMsg]的方法
        this.UnregisterEvent();
    }
    
    [EventMsg]
    private void TestShow1(string msg) 
    {
        selfView.text_info.text = msg;
    }
    
    [EventMsg]
    private void TestShow2(string msg, int num) 
    {
        selfView.text_info.text = msg + num;
    }
}