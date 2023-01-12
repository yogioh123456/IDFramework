using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

public class BuildAssemblieEditor {
    public const string BuildOutputDir = "./Temp/Bin/Debug";
    private const string CodeDir = "Assets/Bundles/Code/";
    
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

    private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences,
        CodeOptimization codeOptimization) {
        List<string> scripts = new List<string>();
        for (int i = 0; i < CodeDirectorys.Length; i++) {
            DirectoryInfo dti = new DirectoryInfo(CodeDirectorys[i]);
            FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
            for (int j = 0; j < fileInfos.Length; j++) {
                scripts.Add(fileInfos[j].FullName);
            }
        }

        if (!Directory.Exists(BuildOutputDir))
            Directory.CreateDirectory(BuildOutputDir);

        string dllPath = Path.Combine(BuildOutputDir, $"{assemblyName}.dll");
        string pdbPath = Path.Combine(BuildOutputDir, $"{assemblyName}.pdb");
        //删除老的
        File.Delete(dllPath);
        File.Delete(pdbPath);

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

            Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

            if (warningCount > 0) {
                Debug.LogFormat("有{0}个Warning!!!", warningCount);
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
                Debug.Log("compile success!");
            }
        };

        //开始构建
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