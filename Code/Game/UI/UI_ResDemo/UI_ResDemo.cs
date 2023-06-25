using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_ResDemo
public class UI_ResDemo : UGUICtrl
{
    public UI_ResDemo_View selfView;
    private GameObject go;

    public UI_ResDemo()
    {
        selfView = new UI_ResDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_ResDemo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_load.AddButtonEvent(() =>
        {
            Debug.Log("加载资源+++++++");
            go = AssetManager.LoadPrefab("Prefabs/Cube");
        });
        selfView.btn_unload.AddButtonEvent(() => {
            Debug.Log("卸载资源-------");
            AssetManager.UnLoadPrefab(go);
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