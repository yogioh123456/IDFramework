using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class EventSystemUtil
{
    //双层嵌套字典,第一个string是类名，第二个消息名，第三个委托方法
    private static Dictionary<string, List<MethodInfo>> allEventDic = new Dictionary<string, List<MethodInfo>>();
    private static Dictionary<string, List<Delegate>> allDelegateDic = new Dictionary<string, List<Delegate>>();

    private static void Init() {
        allEventDic.Clear();
        allDelegateDic.Clear();
    }
    
    //游戏第一次运行，反射
    public static void GetAllEventAuto(this object instance) {
        Init();
        
        //获得当前类的程序集
        Assembly x = instance.GetType().Assembly;
        //获取此程序集中的所有类
        Type[] allClass = x.GetTypes();
        foreach (Type oneClass in allClass)
        {
            List<MethodInfo> list = new List<MethodInfo>();
            MethodInfo[] _method = oneClass.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                                       BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            for (int i = 0; i < _method.Length; i++)
            {
                System.Object[] _attrs = _method[i].GetCustomAttributes(typeof(EventMsg), false); //反射获得用户自定义属性
                for (int j = 0; j < _attrs.Length; j++)
                {
                    if (_attrs[j] is EventMsg)
                    {
                        list.Add(_method[i]);
                    }
                }
            }

            //用fullName是防止 内部类 重复问题
            if (!allEventDic.ContainsKey(oneClass.FullName))
            {
                allEventDic.Add(oneClass.FullName, list);
            }
            else
            {
                Debug.LogError("重复的类" + oneClass.FullName);
            }
        }
    }

    //注意打标签的方法名如果是重复的话，那就相当于多播委托
    //自动注册
    public static void RegisterEvent(this object instance)
    {
        string className = instance.GetType().ToString();
        List<Delegate> delegateList = new List<Delegate>();
        foreach (MethodInfo methodInfo in allEventDic[className])
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            Type type = null;
            if (parameters.Length == 0)
            {
                type = typeof(EventSystemManager.ListenerDelegate);
            }
            else if (parameters.Length == 1)
            {
                type = typeof(EventSystemManager.ListenerDelegate<>).MakeGenericType(parameters[0].ParameterType);
            }
            else if (parameters.Length == 2)
            {
                type = typeof(EventSystemManager.ListenerDelegate<,>).MakeGenericType(parameters[0].ParameterType,
                    parameters[1].ParameterType);
            }
            else if (parameters.Length == 3)
            {
                type = typeof(EventSystemManager.ListenerDelegate<,,>).MakeGenericType(parameters[0].ParameterType,
                    parameters[1].ParameterType, parameters[2].ParameterType);
            }
            else if (parameters.Length == 4)
            {
                type = typeof(EventSystemManager.ListenerDelegate<,,,>).MakeGenericType(parameters[0].ParameterType,
                    parameters[1].ParameterType, parameters[2].ParameterType, parameters[3].ParameterType);
            }

            if (type != null)
            {
                Delegate dDelegate = Delegate.CreateDelegate(type, instance, methodInfo);
                Game.Event.AddListener(methodInfo.Name, dDelegate);
                delegateList.Add(dDelegate);
            }
            else
            {
                Debug.LogError("注册方法参数已超标");
            }
        }

        if (allDelegateDic.ContainsKey(className))
        {
            for (int i = 0; i < delegateList.Count; i++)
            {
                allDelegateDic[className].Add(delegateList[i]);
            }
        }
        else
        {
            allDelegateDic.Add(className, delegateList);
        }
    }

    //自动取消注册
    public static void UnregisterEvent(this object instance)
    {
        string className = instance.GetType().ToString();
        if (!allDelegateDic.ContainsKey(className))
        {
            return;
        }
        foreach (var dDelegate in allDelegateDic[className])
        {
            Game.Event.RemoveListener(dDelegate.Method.Name, dDelegate);
        }

        allDelegateDic.Remove(className);
    }
}