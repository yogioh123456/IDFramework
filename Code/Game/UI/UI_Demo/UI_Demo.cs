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
        selfView.btn_excel.AddButtonEvent(() => {
            Game.UI.OpenUIPanel<UI_ExcelDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_eventSystem.AddButtonEvent(() => {
            Game.UI.OpenUIPanel<UI_EventDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_bindable.AddButtonEvent(() => {
            Game.UI.OpenUIPanel<UI_BindableDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_time.AddButtonEvent(() => {
            Game.UI.OpenUIPanel<UI_TimeDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_hotreload.AddButtonEvent(() => {
            Game.UI.OpenUIPanel<UI_Login>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_ui.AddButtonEvent(() =>
        {
            Game.UI.OpenUIPanel<UI_UIDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_audio.AddButtonEvent(() =>
        {
            Game.UI.OpenUIPanel<UI_AudioDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_resload.AddButtonEvent(() =>
        {
            Game.UI.OpenUIPanel<UI_ResDemo>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_console.AddButtonEvent(() =>
        {
            Game.UI.OpenUIPanel<UI_DevTool>();
            Game.UI.OpenUI<UI_Back>();
        });
        selfView.btn_bugreport.AddButtonEvent(() =>
        {
            Game.UI.OpenUI<UI_Tips>("功能开发中");
        });
        selfView.btn_network.AddButtonEvent(() =>
        {
            Game.UI.OpenUI<UI_Tips>("功能开发中");
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