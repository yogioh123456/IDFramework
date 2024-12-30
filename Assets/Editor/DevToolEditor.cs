using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DevToolEditor {
    [MenuItem("Tools/暂停游戏 _F5")]
    static void PauseGame() {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    [MenuItem("Tools/拷贝Excel文件")]
    static void CopyExcel()
    {
        //拷贝Excel文件
        string excelPath = Path.Combine(Application.dataPath, ".Excel");
        DirectoryInfo root = new DirectoryInfo(excelPath);
        FileInfo[] fileDic = root.GetFiles();
        List<string> excelList = new List<string>();
        foreach (var file in fileDic)
        {
            //查找xlsx，并且开头不是~(打开的文件)
            if (file.FullName.EndsWith("xlsx") && !file.Name.StartsWith("~"))
            {
                excelList.Add("Excel/" + file.Name);
                string desName = Path.Combine(Application.dataPath, "Resources/Bundles/Excel", file.Name + ".bytes");
                File.Copy(file.FullName, desName, true);
            }
        }
        
        //获取 SOExcelConfig 并且赋值
        var data = AssetDatabase.LoadAssetAtPath<SOExcelConfig>("Assets/Resources/Bundles/SO/SOExcelConfig.asset");
        data.configs = excelList;
        EditorUtility.SetDirty(data);
        AssetDatabase.Refresh();
        Debug.Log("文件拷贝完毕");
    }
}
