using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUICtrl
{
    public Type panelName;
    public UGUIView mainView;
    private CanvasGroup canvasGroup;

    protected void OnCreate(UGUIView t,string path,Type _panelName) 
    {
        GameObject go = AssetManager.LoadPrefab(path, Game.UI.UIRoot);
        //go.transform.parent = Game.UI.UIRoot;
        go.SetZero();
        canvasGroup = go.GetComponent<CanvasGroup>();
        if (canvasGroup == null) {
            Debug.LogError(_panelName + " 无CanvasGroup");
        }
        t.gameObject = go;
        t.transform = go.transform;
        t.Init(t.transform);
        mainView = t;
        
        OnRegisterEvent();
        ButtonAddClick();
        panelName = _panelName;
    }

    protected virtual void Init()
    {
        
    }
    
    protected virtual void ButtonAddClick()
    {
        
    }

    protected virtual void OnRegisterEvent()
    {
        
    }
    
    protected void Back()
    {
        Game.UI.BackUIPanel();
    }
    
    public void OpenSelfPanel(object data)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        OpenPanel(data);
    }
    
    protected virtual void OpenPanel(object data) {}

    public void CloseSelfPanel()
    {
        ClosePanel();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    protected virtual void ClosePanel() {}

    //用于一些情况的简单 显示和隐藏
    public void ViewActive(bool _b)
    {
        mainView.gameObject.SetActive(_b);
    }
    
    public virtual void Dispose()
    {
        GameObject.Destroy(mainView.gameObject);
    }

    public virtual void Update()
    {
        
    }
}
