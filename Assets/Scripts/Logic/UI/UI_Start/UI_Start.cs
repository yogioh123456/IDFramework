using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_Start
public class UI_Start : UGUICtrl
{
    public UI_Start_View selfView;

    public UI_Start()
    {
        selfView = new UI_Start_View();
        OnCreate(selfView,"UI/Prefabs/ui_start", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_host.AddButtonEvent(() =>
        {
            Game.AddServerComp();
            Game.ServerNet.StartServer(9996, 100);
            //Game.ClientNet.Connect("127.0.0.1", 7778);
            //GameHelperClient.isHost = true;
        });
        selfView.btn_client.AddButtonEvent(() =>
        {
            string str = selfView.linput_serverIP.text;
            if (string.IsNullOrEmpty(str))
            {
                str = "127.0.0.1";
            }
            Game.ClientNet.Connect(str, 9996);
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