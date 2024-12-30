using System.Collections;
using System.Collections.Generic;
using System.Text;
using UNetwork;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_KCP
public class UI_KCP : UGUICtrl
{
    public UI_KCP_View selfView;

    public UI_KCP()
    {
        selfView = new UI_KCP_View();
        OnCreate(selfView,"UI/Prefabs/ui_kCP", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_server.AddButtonEvent(() => {
            Game.AddComp<KCPServerManager>();
            Game.AddComp<KCPServerLogic>();
            Game.Get<KCPServerLogic>().Init();
        });
        selfView.btn_client.AddButtonEvent(() => {
            Game.Get<KCPClientLogic>().Init();
        });
        selfView.btn_clientsend.AddButtonEvent(() => {
            var data = Encoding.UTF8.GetBytes("send data2");
            Debug.Log($"Send{data.Length}:" + data.Length);
            Game.Get<KCPClientManager>().Send(data);
        });
        selfView.btn_serversend.AddButtonEvent(() => {
            var data = Encoding.UTF8.GetBytes("send data");
            Debug.Log($"Send{data.Length}:" + data.Length);
            Game.Get<KCPServerManager>().Send(data);
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