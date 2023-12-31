using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_AudioDemo
public class UI_AudioDemo : UGUICtrl
{
    public UI_AudioDemo_View selfView;

    public UI_AudioDemo()
    {
        selfView = new UI_AudioDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_AudioDemo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_audio.AddButtonEvent(() => {
            Debug.Log("播放音频");
            //Game.AudioManager.PlayAudio("");
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