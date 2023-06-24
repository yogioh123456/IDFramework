using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_Tips
public class UI_Tips : UGUICtrl
{
    public UI_Tips_View selfView;
    public float time;

    public UI_Tips()
    {
        selfView = new UI_Tips_View();
        OnCreate(selfView,"UI/Prefabs/UI_Tips", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {
        base.OpenPanel(data);
        string str = data.ToString();
        selfView.text_info.text = str;
        time = 0;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            CloseSelfPanel();
        }
    }
}