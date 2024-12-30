using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType
{
    Chinese,
    English,
    Japanese
}

public class LanguageManager
{
    private LanguageType language;
    public LanguageType LanguageCur
    {
        get
        {
            return language;
        }
        set
        {
            language = value;
            Game.Save.Save("language", (int) language);
        }
    }
    public Dictionary<string, object> languageDic = new Dictionary<string, object>();
    
    public LanguageManager()
    {
        languageDic.Clear();
        foreach (var one in ExcelManager.allExcelData)
        {
            if (one.Key.Contains("language_"))
            {
                foreach (var t in (Dictionary<string, object>)one.Value)
                {
                    languageDic.Add(t.Key,t.Value);
                }
            }
        }
        if (Game.Save.Check("language"))
        {
            language = (LanguageType)Game.Save.Load<int>("language");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="readyStr">备份文本</param>
    /// <returns></returns>
    public string Get(string key, string readyStr = "")
    {
        LanguageType languageType = Game.Language.LanguageCur;
        if (!languageDic.ContainsKey(key))
        {
            Debug.LogWarning("本地化不存在key值" + key);
            return key;
        }
        Dictionary<string, object> data = (Dictionary<string, object>) languageDic[key];
        string lt = data.DIC<string>(languageType.ToString());
        if (!string.IsNullOrEmpty(lt))
        {
            return lt;
        }

        //默认中文查询
        string ch = data.DIC<string>(LanguageType.Chinese.ToString());
        if (!string.IsNullOrEmpty(ch))
        {
            return ch;
        }

        if (!string.IsNullOrEmpty(readyStr))
        {
            return readyStr;
        }
        return key;
    }
}

public static class LanguageManagerUtil
{
    public static string Localization(this string str, string readyStr = "")
    {
        return Game.Language.Get(str, readyStr);
    }
}