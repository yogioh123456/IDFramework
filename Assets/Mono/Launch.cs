using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public enum GameMode
{
    Develop,
    Release
}

public class Launch : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;

    public static GameMode GameMode;
    public static AssetBundle assetBundle;
    private static Assembly hotfixAssembly;

    void Start()
    {
#if !UNITY_EDITOR
        gameMode = GameMode.Release;
#endif
        GameMode = gameMode;
        AssetManager.Init();

        Main.Init();
        //加载dll
        //LoadDll(Define.BuildOutputDir);
    }

    public static void ReloadDll() {
        Debug.Log("热重载");
        
        //调用退出方法
        //Type type = hotfixAssembly.GetType("Main");
        //MethodInfo methodInfo = type.GetMethod("Quit");
        //methodInfo.Invoke(null, null);
    }

    private void Update()
    {
        CodeLoader.Instance.Update();
    }

    private void FixedUpdate()
    {
        CodeLoader.Instance.FixedUpdate();
    }
    
    private void LateUpdate()
    {
        CodeLoader.Instance.LateUpdate();
    }

    private void OnApplicationQuit()
    {
        CodeLoader.Instance.OnApplicationQuit();
    }
}