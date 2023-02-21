using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

public class BuildAssemblieEditor {
    private const string BuildOutputDir = Define.BuildOutputDir;
    private const string CodeDir = "Assets/Bundles/Code/";
    private static DateTime compileStartTime;

    [MenuItem("Tools/CompileDll _F8")]
    public static void BuildLogic() {
        //编译外部Code文件夹代码，生成dll
        BuildMuteAssembly("Code", new []
        {
            "Code/",
        }, Array.Empty<string>(), CodeOptimization.Debug);

        //完成编译后进行Dll拷贝和设置ab包
        //AfterCompiling();

        AssetDatabase.Refresh();
    }

    private static void BuildMuteAssembly(string assemblyName, string[] codeDirectorys, string[] additionalReferences,
        CodeOptimization codeOptimization) {
        //让每次生成的Dll名字不同
        assemblyName += UnityEngine.Random.Range(100000000, 999999999).ToString();
        Debug.Log(assemblyName);
        
        List<string> scripts = new List<string>();
        for (int i = 0; i < codeDirectorys.Length; i++) {
            DirectoryInfo dti = new DirectoryInfo(codeDirectorys[i]);
            FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
            for (int j = 0; j < fileInfos.Length; j++) {
                scripts.Add(fileInfos[j].FullName);
            }
        }

        if (!Directory.Exists(BuildOutputDir))
            Directory.CreateDirectory(BuildOutputDir);

        //删除老文件
        DirectoryInfo directory = new DirectoryInfo(BuildOutputDir);
        FileInfo[] files = directory.GetFiles("Code*", System.IO.SearchOption.AllDirectories);
        foreach (var one in files) {
            File.Delete(one.FullName);
        }
        
        string dllPath = Path.Combine(BuildOutputDir, $"{assemblyName}.dll");
        AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());

        //启用UnSafe
        //assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

        BuildTargetGroup buildTargetGroup =
            BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

        assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
        assemblyBuilder.compilerOptions.ApiCompatibilityLevel =
            PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
        // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

        assemblyBuilder.additionalReferences = additionalReferences;

        assemblyBuilder.flags = AssemblyBuilderFlags.None;
        //AssemblyBuilderFlags.None                 正常发布
        //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
        //AssemblyBuilderFlags.EditorAssembly       编辑器状态
        assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

        assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

        assemblyBuilder.buildTargetGroup = buildTargetGroup;

        assemblyBuilder.buildStarted += delegate(string assemblyPath) {
            Debug.LogFormat("build start：" + assemblyPath);
        };

        assemblyBuilder.buildFinished += delegate(string assemblyPath, CompilerMessage[] compilerMessages) {
            int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
            int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

            if (warningCount == 0 && errorCount == 0)
            {
                Debug.LogFormat("<color=#00ff00>Warnings: {0} - Errors: {1}</color>", warningCount, errorCount);
            }
            else if (warningCount > 0)
            {
                Debug.LogFormat("<color=yellow>Warnings: {0} - Errors: {1}</color>", warningCount, errorCount);
            }
            else if (errorCount > 0)
            {
                Debug.LogFormat("<color=red>Warnings: {0} - Errors: {1}</color>", warningCount, errorCount);
            }

            if (warningCount > 0) {
                for (int i = 0; i < compilerMessages.Length; i++) {
                    if (compilerMessages[i].type == CompilerMessageType.Warning) {
                        Debug.LogWarning(compilerMessages[i].message);
                    }
                }
            }
            
            if (errorCount > 0) {
                if (PlayerPrefs.GetInt("AutoBuild") == 1) //如果开启了自动编译要Cancel掉，否则会死循环
                    CancelAutoBuildCode();
                for (int i = 0; i < compilerMessages.Length; i++) {
                    if (compilerMessages[i].type == CompilerMessageType.Error) {
                        Debug.LogError(compilerMessages[i].message);
                    }
                }
            } else {
                Debug.Log($"compile success!  time:{(DateTime.Now - compileStartTime).TotalSeconds}");
                //判断是否进入热重载
                if (Application.isPlaying) {
                    Launch.ReloadDll();
                }
            }
        };

        //开始构建
        compileStartTime = DateTime.Now;
        if (!assemblyBuilder.Build()) {
            Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
        }
    }

    private static void CancelAutoBuildCode() {
        PlayerPrefs.DeleteKey("AutoBuild");
        ShowNotification("AutoBuildCode Disabled");
    }
    
    private static void AfterCompiling()
    {
        while (EditorApplication.isCompiling)
        {
            Debug.Log("Compiling wait1");
            // 主线程sleep并不影响编译线程
            Thread.Sleep(1000);
            Debug.Log("Compiling wait2");
        }
            
        Debug.Log("Compiling finish");

        Directory.CreateDirectory(CodeDir);
        File.Copy(Path.Combine(BuildOutputDir, "Code.dll"), Path.Combine(CodeDir, "Code.dll.bytes"), true);
        File.Copy(Path.Combine(BuildOutputDir, "Code.pdb"), Path.Combine(CodeDir, "Code.pdb.bytes"), true);
        AssetDatabase.Refresh();
        Debug.Log("copy Code.dll to Bundles/Code success!");
            
        // 设置ab包
        AssetImporter assetImporter1 = AssetImporter.GetAtPath("Assets/Bundles/Code/Code.dll.bytes");
        assetImporter1.assetBundleName = "Code.unity3d";
        AssetImporter assetImporter2 = AssetImporter.GetAtPath("Assets/Bundles/Code/Code.pdb.bytes");
        assetImporter2.assetBundleName = "Code.unity3d";
        AssetDatabase.Refresh();
        Debug.Log("set assetbundle success!");
            
        Debug.Log("build success!");
        //反射获取当前Game视图，提示编译完成
        ShowNotification("Build Code Success");
    }
    
    private static void ShowNotification(string tips)
    {
        var game = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
        game?.ShowNotification(new GUIContent($"{tips}"));
    }
}