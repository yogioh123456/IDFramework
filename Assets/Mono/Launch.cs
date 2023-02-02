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
        
        //加载dll
        LoadDll(Define.BuildOutputDir);
    }

    public static void ReloadDll() {
        Debug.Log("热重载");
        
        //调用退出方法
        Type type = hotfixAssembly.GetType("Main");
        MethodInfo methodInfo = type.GetMethod("Quit");
        methodInfo.Invoke(null, null);
        
        //重新加载程序集
        LoadDll(Define.BuildOutputDir);
    }
    
    private static void LoadDll(string BuildOutputDir) {
        //Unity版本对于Load Dll的影响
        //2019 就算在运行时修改了dll，也是无效的，拿的还是上一次的dll
        //2020 unity认为相同路径为上一次的dll
        //2021及以上 没问题
        //存在另外一个bug，必须使用新命名的dll，不然的话断点没有数据
        
        //读取相对路径文件夹下的某种名称的 dll
        string[] logicFiles = Directory.GetFiles(BuildOutputDir, "Code*.dll");

        if (logicFiles.Length != 1)
        {
            throw new Exception("Logic dll count != 1");
        }

        string logicName = Path.GetFileNameWithoutExtension(logicFiles[0]);
        byte[] assBytes = File.ReadAllBytes(Path.Combine(BuildOutputDir, $"{logicName}.dll"));
        byte[] pdbBytes = File.ReadAllBytes(Path.Combine(BuildOutputDir, $"{logicName}.pdb"));
        //通过 dll 和 pdb 加载程序集
        hotfixAssembly = Assembly.Load(assBytes, pdbBytes);

        //实例化调用 dll 方法
        Type type = hotfixAssembly.GetType("Main");
        MethodInfo methodInfo = type.GetMethod("Init");
        methodInfo.Invoke(null, null);
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