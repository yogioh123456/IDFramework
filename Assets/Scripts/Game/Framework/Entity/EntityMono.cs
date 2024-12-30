using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityMono : MonoBehaviour
{
    public Dictionary<Type, object> compDic = new Dictionary<Type, object>();
    
    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddComp<T>() where T : new()
    {
        Type type = typeof (T);
        T t = (T)Activator.CreateInstance(type);
        
        if (!compDic.ContainsKey(type))
        {
            compDic.Add(type, t);
        }
        else
        {
            Debug.LogError("不能重复添加组件");
        }
        
        return t;
    }
    
    /// <summary>
    /// 添加挂在场景中的MonoBehaviour组件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddMonoComp<T>() where T : MonoBehaviour
    {
        Type type = typeof (T);
        T t = UnityEngine.Object.FindObjectOfType<T>();
        if (t == null)
        {
            GameObject go = new GameObject(typeof(T).ToString());
            t = go.AddComponent<T>();
        }
        if (!compDic.ContainsKey(type))
        {
            compDic.Add(type, t);
        }
        else
        {
            Debug.LogError("不能重复添加组件");
        }

        return t;
    }
    
    public T AddMonoComp<T>(GameObject go) where T : MonoBehaviour
    {
        Type type = typeof (T);
        T t = go.GetComponent<T>();
        if (!compDic.ContainsKey(type))
        {
            compDic.Add(type, t);
        }
        else
        {
            Debug.LogError("不能重复添加组件");
        }

        return t;
    }

    public void ClearMonoComp()
    {
        compDic.Clear();
    }

    /// <summary>
    /// 获取组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetComp<T>()
    {
        Type type = typeof (T);
        if (compDic.ContainsKey(type))
        {
            return (T)compDic[type];
        }
        return default;
    }
}
