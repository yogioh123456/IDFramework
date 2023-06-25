using UnityEngine;

public class SaveLoadManager 
{
    /// <summary>
    /// 存储结构体数据
    /// </summary>
    /// <param name="saveKey"></param>
    /// <param name="saveData"></param>
    public void Save(string saveKey, object saveData) 
    {
        if (saveData == null) 
        {
            return;
        }
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 存储string数据
    /// </summary>
    /// <param name="saveKey"></param>
    /// <param name="saveData"></param>
    public void Save(string saveKey, string saveData) 
    {
        if (string.IsNullOrEmpty(saveData))
        {
            return;
        }
        PlayerPrefs.SetString(saveKey, saveData);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 存储int类型
    /// </summary>
    /// <param name="saveKey"></param>
    /// <param name="saveData"></param>
    public void SaveInt(string saveKey, int saveData) 
    {
        PlayerPrefs.SetInt(saveKey, saveData);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 存储float类型
    /// </summary>
    /// <param name="saveKey"></param>
    /// <param name="saveData"></param>
    public void SaveFloat(string saveKey, float saveData) 
    {
        PlayerPrefs.SetFloat(saveKey, saveData);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 读取字符串或者结构体数据
    /// </summary>
    /// <param name="saveKey"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Load<T>(string saveKey) 
    {
        // 检查是否存在存档数据
        if (PlayerPrefs.HasKey(saveKey)) 
        {
            if (typeof(T) == typeof(string)) 
            {
                string str = PlayerPrefs.GetString(saveKey);
                return (T)(object)str;
            }

            // 从PlayerPrefs中获取存档数据的JSON字符串
            string json = PlayerPrefs.GetString(saveKey);
            // 将JSON字符串转换为存档数据对象
            T saveData = JsonUtility.FromJson<T>(json);
            return saveData;
        }
        
        Debug.LogError("没有找到存档数据");
        return default;
    }

    /// <summary>
    /// 读取int数据
    /// </summary>
    /// <param name="saveKey"></param>
    /// <returns></returns>
    public int LoadInt(string saveKey) 
    {
        // 检查是否存在存档数据
        if (PlayerPrefs.HasKey(saveKey)) 
        {
            int data = PlayerPrefs.GetInt(saveKey);
            return data;
        }
        
        Debug.LogError("没有找到存档数据");
        return default;
    }
    
    /// <summary>
    /// 读取float数据
    /// </summary>
    /// <param name="saveKey"></param>
    /// <returns></returns>
    public float LoadFloat(string saveKey) 
    {
        // 检查是否存在存档数据
        if (PlayerPrefs.HasKey(saveKey)) 
        {
            float data = PlayerPrefs.GetFloat(saveKey);
            return data;
        }
        
        Debug.LogError("没有找到存档数据");
        return default;
    }
    
    public bool Check(string saveKey)
    {
        return PlayerPrefs.HasKey(saveKey);
    }

    public void ClearAll() {
        PlayerPrefs.DeleteAll();
    }
}
