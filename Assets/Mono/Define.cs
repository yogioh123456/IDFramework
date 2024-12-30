using UnityEngine;

public class Define
{
    public const string BuildOutputDir = "./Code/Bin";
    //public const string UIScriptsPath = "./Code/Logic/UI/";
    public static string UIScriptsPath => Application.dataPath + "/Scripts/Logic/UI/";

    public string GetPath()
    {
        string a = Application.dataPath + "/Scripts/Logic/UI/";
        return "";
    }
}
