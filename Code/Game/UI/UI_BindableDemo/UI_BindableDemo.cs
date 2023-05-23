using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_BindableDemo
public class UI_BindableDemo : UGUICtrl
{
    public UI_BindableDemo_View selfView;
    private BindableProperty<int> num = new BindableProperty<int>(0);

    public UI_BindableDemo()
    {
        selfView = new UI_BindableDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_BindableDemo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------添加事件-----------------
        num.AddListener(() => {
            selfView.text_num.text = num.Value.ToString();
        });
        
        selfView.btn_add.AddButtonEvent(() => {
            num.Value++;
        });
        
        selfView.btn_reduce.AddButtonEvent(() => {
            num.Value--;
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