﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List
/// </summary>
[Serializable]
public class JsonSerialization<T>
{
    /// <summary>
    /// 实际序列化的字段
    /// </summary>
    [SerializeField]
    private List<T> data;

    /// <summary>
    /// 构造方法
    /// </summary>
    public JsonSerialization(List<T> data)
    {
        this.data = data;
    }

    /// <summary>
    /// 转换为字典返回
    /// </summary>
    /// <returns></returns>
    public List<T> ToList()
    {
        return data;
    }
}

/// <summary>
/// Dictionary
/// </summary>
[Serializable]
public class JsonSerialization<TKey, TValue> : ISerializationCallbackReceiver
{
    /// <summary>
    /// 实际序列化的字段
    /// </summary>
    [SerializeField]
    private List<TKey> keys;

    /// <summary>
    /// 实际序列化的字段
    /// </summary>
    [SerializeField]
    private List<TValue> values;

    /// <summary>
    /// 待序列化的数据
    /// </summary>
    private Dictionary<TKey, TValue> data;

    /// <summary>
    /// 构造方法
    /// </summary>
    public JsonSerialization(Dictionary<TKey, TValue> data)
    {
        this.data = data;
    }

    /// <summary>
    /// 序列化前调用
    /// </summary>
    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(data.Keys);
        values = new List<TValue>(data.Values);
    }

    /// <summary>
    /// 反序列化后调用
    /// </summary>
    public void OnAfterDeserialize()
    {
        if (keys == null || values == null ||
            keys.Count <= 0 || values.Count <= 0)
        {
            Debug.LogError("反序列化失败!");
        }
        else
        {
            if (keys.Count == values.Count)
            {
                data = new Dictionary<TKey, TValue>(keys.Count);
                for (var index = 0; index < keys.Count; ++index)
                {
                    data.Add(keys[index], values[index]);
                }
            }
        }
    }

    /// <summary>
    /// 转换为字典返回
    /// </summary>
    /// <returns></returns>
    public Dictionary<TKey, TValue> ToDictionary()
    {
        return data;
    }
}
