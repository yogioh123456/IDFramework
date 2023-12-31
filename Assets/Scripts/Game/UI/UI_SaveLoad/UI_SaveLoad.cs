using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_SaveLoad
public class UI_SaveLoad : UGUICtrl
{
    public UI_SaveLoad_View selfView;

    public UI_SaveLoad()
    {
        selfView = new UI_SaveLoad_View();
        OnCreate(selfView,"UI/Prefabs/UI_SaveLoad", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_save.AddButtonEvent(() => {
            string str = selfView.input_info.text;
            MyData myData = new MyData();
            myData.age = 22;
            myData.name = str;
            Game.SaveLoad.Save("test", myData);
            Debug.Log("存储成功");
        });
        
        
        selfView.btn_load.AddButtonEvent(() => {
            MyData str = Game.SaveLoad.Load<MyData>("test");
            selfView.text_loadInfo.text = str.name;
            Debug.Log("读取成功");
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

public struct MyData {
    public int age;
    public string name;
}