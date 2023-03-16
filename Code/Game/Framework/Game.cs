using System;
using UnityEngine;

public partial class Game : EntityStatic
{
    public static Action UpdateEvent;
    public static Action FixedUpdateEvent;
    
    public static void Init() {
        Clear();
        Add<ExcelManager>();
        Add<UGUIManager>();
        Add<TimerManager>();
        Add<ServerNetwork>();
        Add<ClientNetwork>();
        Add<ServerSyncManager>();
        Add<EventSystemManager>();
        Add<MainLogic>();
        Add<AudioManager>();
    }

    public static void Update() {
        for (int i = 0; i < updateList.Count; i++)
        {
            updateList[i].Update();
        }

        if (UpdateEvent != null)
        {
            UpdateEvent.Invoke();
        }
    }
    
    public static void FixedUpdate() {
        for (int i = 0; i < fixedUpdateList.Count; i++)
        {
            fixedUpdateList[i].FixedUpdate();
        }

        if (FixedUpdateEvent != null)
        {
            FixedUpdateEvent.Invoke();
        }
    }
    
    public static void LateUpdate() {
        for (int i = 0; i < lateUpdateList.Count; i++)
        {
            lateUpdateList[i].LateUpdate();
        }
    }
    
    public static void OnApplicationQuit() {
        for (int i = 0; i < applicationList.Count; i++)
        {
            applicationList[i].OnApplicationQuit();
        }
        Clear();
    }
}
