using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Ctrl层: UI_ExcelDemo
public class UI_ExcelDemo : UGUICtrl
{
    public UI_ExcelDemo_View selfView;

    public UI_ExcelDemo()
    {
        selfView = new UI_ExcelDemo_View();
        OnCreate(selfView,"UI/Prefabs/UI_ExcelDemo", GetType());
    }

    /// <summary>
    /// 按钮添加事件
    /// </summary>
    protected override void ButtonAddClick()
    {
        //------------------按钮添加事件-----------------
        selfView.btn_load.AddButtonEvent(LoadExcelData);
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    protected override void OpenPanel(object data)
    {
        base.OpenPanel(data);
        
    }

    private void LoadExcelData() {
        var data = ExcelManager.allExcelData["config"];
        // 字符串类型
        string v = data.DIC("welcome").DIC<string>("name");
        selfView.text_content.text = v;
        Debug.Log("Excel表数据读取成功");
        Debug.Log(v);
        // bool类型
        bool testBool = data.DIC("welcome").DIC<bool>("isOn");
        Debug.Log(testBool);
        // int类型
        int testInt = data.DIC("testData").DIC<int>("num");
        Debug.Log(testInt);
        // float类型
        float testFloat = data.DIC("testData").DIC<float>("num2");
        Debug.Log(testFloat);
        // double类型
        double testDouble = data.DIC("testData").DIC<double>("num3");
        Debug.Log(testDouble);
    }
}