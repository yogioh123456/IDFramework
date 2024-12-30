using UnityEngine;
using UnityEditor;
using System.IO;

public class NetHelper : UnityEditor.Editor {

    //源文件夹
    private static string netFilePath = "../Server/IDServer/IDServer/Data/";
    //拷贝的文件夹
    private static string targetPath = Application.dataPath + "/Scripts/Network/Data/";
    
    [MenuItem("网络工具/同步网络协议")]
    private static void CopyNetFile()
    {
        DirectoryInfo root = new DirectoryInfo(netFilePath);
        FileInfo[] fileDic = root.GetFiles();
        foreach (var file in fileDic)
        {
            if (file.Name.StartsWith("Net") && file.Name.EndsWith(".cs")) {
                File.Copy(file.FullName, targetPath + file.Name, true);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("协议同步成功!");
    }
}
