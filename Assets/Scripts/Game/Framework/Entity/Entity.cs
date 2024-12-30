using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public Dictionary<Type, object> compDic = new Dictionary<Type, object>();
    public List<IUpdate> updateList = new List<IUpdate>();
    public List<IFixedUpdate> fixedUpdateList = new List<IFixedUpdate>();
    public List<ILateUpdate> lateUpdateList = new List<ILateUpdate>();
    public List<IApplicationQuit> applicationList = new List<IApplicationQuit>();
    
    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddComp<T>(params object[] datas)
    {
        Type type = typeof (T);
        T t = (T)Activator.CreateInstance(type, datas);
        
        if (!compDic.ContainsKey(type))
        {
            compDic.Add(type, t);
            
            if (t is IUpdate update)
            {
                updateList.Add(update);
            }
            if (t is IFixedUpdate fixedUpdate)
            {
                fixedUpdateList.Add(fixedUpdate);
            }
            if (t is ILateUpdate lateUpdate)
            {
                lateUpdateList.Add(lateUpdate);
            }
            if (t is IApplicationQuit applicationQuit)
            {
                applicationList.Add(applicationQuit);
            }
        }
        else
        {
            Debug.LogError("不能重复添加组件");
        }
        
        return t;
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

    public void RemoveComp<T>(T t)
    {
        Type type = typeof (T);
        if (compDic.ContainsKey(type))
        {
            if (compDic[type] is Entity entity)
            {
                if (entity.compDic.Count > 0)
                {
                    foreach (var one in entity.compDic)
                    {
                        Type c = one.Key;
                        RemoveComp(c);
                    }
                }
                else
                {
                    if (compDic[type] is IUpdate update)
                    {
                        updateList.Remove(update);
                    }
                    if (compDic[type] is IFixedUpdate fixedUpdate)
                    {
                        fixedUpdateList.Remove(fixedUpdate);
                    }
                    if (compDic[type] is ILateUpdate lateUpdate)
                    {
                        lateUpdateList.Remove(lateUpdate);
                    }
                    if (compDic[type] is IApplicationQuit applicationQuit)
                    {
                        applicationList.Remove(applicationQuit);
                    }
                }
            }
            compDic.Remove(type);
        }
    }

    public virtual void Dispose() {
        foreach (var one in compDic) {
            if (one.Value is Entity entity) {
                entity.Dispose();
            }
        }
    }
}
