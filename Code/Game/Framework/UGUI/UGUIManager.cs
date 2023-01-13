using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.主面板是栈结构
/// 2.弹出面板单独集合，可自行控制全部关闭
/// 3.一个界面可以由多个UI面板组成
/// 4.未能实现：1个返回按钮既可以关闭Window 又可以关闭 Panel，主要是这个功能和3号有冲突，如果需要实现的话，那么3号应该有1个单独的字典来维护
/// </summary>
public class UGUIManager
{
    //栈结构主面板
    public Stack<UGUICtrl> uiPanelStack = new Stack<UGUICtrl>();

    //入栈ui面板
    public Dictionary<string, UGUICtrl> uiPanelCtrl = new Dictionary<string, UGUICtrl>();
    //叠加面板
    public Dictionary<string, UGUICtrl> uiWindowCtrl = new Dictionary<string, UGUICtrl>();
    //当前面板
    private string curPanelName = "";
    //UI摄像机(Camera Canvas)
    public Camera uiCamera;
    public RectTransform canvasRectTransform;
    public Transform UIRoot;

    public UGUIManager() {
        var uiRoot = AssetManager.LoadPrefab("UI/Prefabs/UIRoot");
        uiRoot.SetZero();
        UIRoot = uiRoot.transform.GetChild(0);
        canvasRectTransform = UIRoot.GetComponent<RectTransform>();
        Canvas canvas = UIRoot.GetComponent<Canvas>();
        if (canvas != null)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                uiCamera = canvas.worldCamera;
            }
        }
    }
    
    /// <summary>
    /// 打开面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void OpenUIPanel<T>(object data = null) where T: UGUICtrl, new()
    {
        string panelName = typeof(T).ToString();
        //Debug.Log("打开" + panelName);
        if (curPanelName.Equals(panelName))
        {
            return;
        }
        curPanelName = panelName;
        if (!uiPanelCtrl.ContainsKey(panelName))
        {
            uiPanelCtrl.Add(panelName, new T());
        }
        if (uiPanelStack.Count > 0)
        {
            //关闭当前主面板
            UGUICtrl peakCtrl = uiPanelStack.Peek();
            peakCtrl.CloseSelfPanel();
        }
        uiPanelStack.Push(uiPanelCtrl[panelName]);//入栈
        uiPanelCtrl[panelName].OpenSelfPanel(data);//打开新面板
    }

    /// <summary>
    /// 打开UI Window
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    public UGUICtrl OpenUI<T>(object data = null) where T: UGUICtrl, new()
    {
        //Debug.Log("直接打开ui");
        string panelName = typeof(T).ToString();
        if (!uiWindowCtrl.ContainsKey(panelName))
        {
            uiWindowCtrl.Add(panelName, new T());
        }
        uiWindowCtrl[panelName].mainView.transform.SetAsLastSibling();
        uiWindowCtrl[panelName].OpenSelfPanel(data);
        return uiWindowCtrl[panelName];
    }

    /// <summary>
    /// 打开UI Window 并设置位置
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    public void OpenUI<T>(Vector3 v3,object data = null) where T: UGUICtrl, new()
    {
        string panelName = typeof(T).ToString();
        OpenUI<T>(data);
        uiWindowCtrl[panelName].mainView.transform.position = v3;
    }
    
    // 打开UI Window 通过 anchoredPosition 适用于UI Camera 方案
    public void OpenUIArchPos<T>(Vector3 v3,object data = null) where T: UGUICtrl, new()
    {
        string panelName = typeof(T).ToString();
        OpenUI<T>(data);
        uiWindowCtrl[panelName].mainView.transform.GetComponent<RectTransform>().anchoredPosition = v3;
    }
    
    /// <summary>
    /// 直接关闭UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void CloseUI<T>() where T: UGUICtrl, new()
    {
        string panelName = typeof(T).ToString();
        if (uiWindowCtrl.ContainsKey(panelName))
        {
            uiWindowCtrl[panelName].CloseSelfPanel();
        }
    }

    public void CloseAllWindowUI()
    {
        foreach (var window in uiWindowCtrl)
        {
            window.Value.CloseSelfPanel();
        }
    }
    
    public T GetUI<T>()
    {
        string panelName = typeof(T).ToString();
        if (uiPanelCtrl.ContainsKey(panelName))
        {
            object a = uiPanelCtrl[panelName];
            return (T) a;
        }

        if (uiWindowCtrl.ContainsKey(panelName))
        {
            object a = uiWindowCtrl[panelName];
            return (T) a;
        }

        return default;
    }
    
    /// <summary>
    /// 回退
    /// </summary>
    public void BackUIPanel()
    {
        //Debug.Log(uiCtrlStack.Count);
        if (uiPanelStack.Count > 1)
        {
            UGUICtrl peakCtrl = uiPanelStack.Peek();
            if (uiWindowCtrl.ContainsKey(peakCtrl.panelName))
            {
                uiWindowCtrl.Remove(peakCtrl.panelName);
            }
            peakCtrl.CloseSelfPanel();
            uiPanelStack.Pop();
            peakCtrl = uiPanelStack.Peek();
            curPanelName = peakCtrl.panelName;
            peakCtrl.OpenSelfPanel(null);
        }
    }
}
