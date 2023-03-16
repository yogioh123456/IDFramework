using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatic
{
    private static Dictionary<Type, object> compDic = new(16);
    protected static List<IUpdate> updateList = new(16);
    protected static List<IFixedUpdate> fixedUpdateList = new(16);
    protected static List<ILateUpdate> lateUpdateList = new(16);
    protected static List<IApplicationQuit> applicationList = new(16);

    public static void Clear()
    {
        compDic.Clear();
        updateList.Clear();
        fixedUpdateList.Clear();
        lateUpdateList.Clear();
        applicationList.Clear();
    }

    protected static void AddComp(Type type)
    {
        object t = Activator.CreateInstance(type);
        AddToComp(type, t);
    }

    private static void AddToComp(Type type, object t)
    {
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

            if (t is Entity entity)
            {
                foreach (var one in entity.updateList)
                {
                    updateList.Add(one);
                }

                foreach (var one in entity.fixedUpdateList)
                {
                    fixedUpdateList.Add(one);
                }

                foreach (var one in entity.lateUpdateList)
                {
                    lateUpdateList.Add(one);
                }

                foreach (var one in entity.applicationList)
                {
                    applicationList.Add(one);
                }
            }
        }
        else
        {
            Debug.LogError("不能重复添加组件" + type);
        }
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Add<T>() where T : new()
    {
        Type type = typeof(T);
        T t = (T) Activator.CreateInstance(type);
        AddToComp(type, t);
        return t;
    }

    /// <summary>
    /// 组件刷新
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T AddNew<T>() where T : new()
    {
        Type type = typeof(T);
        T t = (T) Activator.CreateInstance(type);

        if (!compDic.ContainsKey(type))
        {
            compDic.Add(type, t);
        }
        else
        {
            compDic[type] = t;
        }

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

        return t;
    }

    public static void RemoveComp<T>()
    {
        Type type = typeof(T);
        var t = compDic[type];
        if (t != null)
        {
            if (t is IUpdate update)
            {
                updateList.Remove(update);
            }

            if (t is IFixedUpdate fixedUpdate)
            {
                fixedUpdateList.Remove(fixedUpdate);
            }

            if (t is ILateUpdate lateUpdate)
            {
                lateUpdateList.Remove(lateUpdate);
            }

            if (t is IApplicationQuit applicationQuit)
            {
                applicationList.Remove(applicationQuit);
            }

            if (t is IDispose dispose)
            {
                dispose.Dispose();
            }

            if (t is Entity entity)
            {
                foreach (var one in entity.updateList)
                {
                    updateList.Remove(one);
                }

                foreach (var one in entity.fixedUpdateList)
                {
                    fixedUpdateList.Remove(one);
                }

                foreach (var one in entity.lateUpdateList)
                {
                    lateUpdateList.Remove(one);
                }

                foreach (var one in entity.applicationList)
                {
                    applicationList.Remove(one);
                }

                entity.Dispose();
            }
        }

        compDic.Remove(type);
    }

    /// <summary>
    /// 获取组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>()
    {
        Type type = typeof(T);
        if (compDic.ContainsKey(type))
        {
            return (T) compDic[type];
        }

        return default;
    }
}