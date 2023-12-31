using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_TimeDemo
public class UI_TimeDemo : UGUICtrl
{
    public UI_TimeDemo_View selfView;

    public UI_TimeDemo()
    {
        selfView = new UI_TimeDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_TimeDemo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_test.AddButtonEvent(() => {
            selfView.text_info.text = "Start Timer";
            Debug.Log("开始执行延时函数，2秒......");
            Game.Time.AddTimer(2, () => {
                selfView.text_info.text = "2s later, success!";
                Debug.Log("执行成功！");
            });
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