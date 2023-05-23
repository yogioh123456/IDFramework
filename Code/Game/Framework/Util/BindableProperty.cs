using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

/// <summary>
/// 属性绑定
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindableProperty<T>
{
    private List<Action> changeList = new List<Action>();
    private T _value;

    public BindableProperty(T value)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            DispatchAll();
        }
    }

    public void AddListener(Action ac)
    {
        if (!changeList.Contains(ac))
        {
            changeList.Add(ac);
            ac();//绑定时候执行下
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }

    public void RemoveListener(Action ac)
    {
        if (changeList.Contains(ac))
        {
            changeList.Remove(ac);
        }
        else
        {
            Debug.LogWarning("移除的消息不存在");
        }
    }

    public void ClearListener()
    {
        changeList.Clear();
        //Debug.Log("清理成功");
    }

    private void DispatchAll()
    {
        foreach (var item in changeList)
        {
            item();
        }
    }
}

/// <summary>
/// List
/// 使用了ReadOnlyCollection作为保护，使用IReadOnlyList也是可以
/// 通过Add Remove  Update  Clear 对数据进行操作
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindableList<T>
{
    //数据发生改变时，监听列表
    private List<Action> changeList = new List<Action>();
    //数据添加时，监听列表
    private List<Action<T>> addEventList = new List<Action<T>>();
    //数据移除时，监听列表
    private List<Action<T>> removeEventList = new List<Action<T>>();
    //单个数据修改时，监听列表
    private List<Action<int,T>> changeEventList = new List<Action<int,T>>();
    
    //添加移除多数据
    private List<Action<List<T>>> addMultiEventList = new List<Action<List<T>>>();
    private List<Action<List<T>>> removeMultiEventList = new List<Action<List<T>>>();
    
    private List<T> _data;

    public BindableList()
    {
        _data = new List<T>();
    }

    public ReadOnlyCollection<T> data
    {
        get {
            return _data.AsReadOnly();
        }
        private set {
            
        }
    }

    public int Count => _data.Count;

    public T IndexOf(int index)
    {
        return _data[index];
    }
    
    public void Add(T t)
    {
        _data.Add(t);
        DispatchAll();
        DispatchAddEvent(t);
    }
    
    public void Insert(int index, T t)
    {
        _data.Insert(index, t);
        DispatchAll();
        DispatchAddEvent(t);
    }
    
    public void Add(List<T> _list)
    {
        _data = _data.Union(_list).ToList();
        DispatchAll();
        foreach (var item in addMultiEventList)
        {
            item(_list);
        }
    }

    public void Remove(T t)
    {
        if (_data.Contains(t))
        {
            _data.Remove(t);
            DispatchAll();
            DispatchRemoveEvent(t);
        }
    }
    
    public void Remove(List<T> _list)
    {
        bool isChange = false;
        foreach (var t in _list)
        {
            if (_data.Contains(t))
            {
                isChange = true;
                _data.Remove(t);
            }
        }
        if (isChange)
        {
            DispatchAll();
            foreach (var item in removeMultiEventList)
            {
                item(_list);
            }
        }
    }

    public void RemoveAt(int index)
    {
        if (_data[index] != null)
        {
            _data.RemoveAt(index);
            DispatchAll();
        }
    }
    
    public void Clear()
    {
        _data.Clear();
        DispatchAll();
    }

    public void AddListener_Change(Action<int, T> ac)
    {
        if (!changeEventList.Contains(ac))
        {
            changeEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void DispatchChange(int index, T t)
    {
        foreach (var item in changeEventList)
        {
            item(index, t);
        }
    }
    
    public void Change(int index, T t)
    {
        if (index >= _data.Count)
        {
            return;
        }
        _data[index] = t;
        DispatchChangeListener(index, t);
    }

    public void DispatchChangeListener(int index, T t)
    {
        DispatchChange(index, t);
        DispatchAll();
    }
    
    //全部替换
    public void UpdateAll(List<T> _list)
    {
        _data = _list;
        DispatchAll();
    }

    //替换
    public void ReplaceByIndex(int index, T one)
    {
        _data[index] = one;
        DispatchAll();
    }

    //交换
    public void Exchange(int a, int b)
    {
        var temp = _data[a].Clone();
        _data[a] = _data[b];
        _data[b] = temp;
        DispatchAll();
    }

    public void AddListener(Action ac)
    {
        if (!changeList.Contains(ac))
        {
            changeList.Add(ac);
            ac();//绑定时候执行下
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }

    public T this[int index]
    {
        get { return _data[index]; }
        set
        {
            DispatchAll();
            _data[index] = value;
        }
    }
    
    //数组打乱
    public List<T> Shuffle()
    {
        System.Random randomNum = new System.Random();
        int index = 0;
        T temp;
        for (int i = 0; i < _data.Count; i++)
        {
            index = randomNum.Next(0, _data.Count - 1);
            if (index != i)
            {
                temp = _data[i];
                _data[i] = _data[index];
                _data[index] = temp;
            }
        }
        return _data;
    }

    public void AddListener_Add(Action<T> ac)
    {
        if (!addEventList.Contains(ac))
        {
            addEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void AddListener_AddMulti(Action<List<T>> ac)
    {
        if (!addMultiEventList.Contains(ac))
        {
            addMultiEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void AddListener_Remove(Action<T> ac)
    {
        if (!removeEventList.Contains(ac))
        {
            removeEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }

    public void AddListener_RemoveMulti(Action<List<T>> ac)
    {
        if (!removeMultiEventList.Contains(ac))
        {
            removeMultiEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void RemoveListener(Action ac)
    {
        if (changeList.Contains(ac))
        {
            changeList.Remove(ac);
        }
        else
        {
            Debug.LogWarning("移除的消息不存在");
        }
    }

    public void RemoveListener(Action<T> ac)
    {
        if (addEventList.Contains(ac))
        {
            addEventList.Remove(ac);
        }
        else if (removeEventList.Contains(ac))
        {
            removeEventList.Remove(ac);
        }
    }

    public void RemoveListener(Action<List<T>> ac)
    {
        if (addMultiEventList.Contains(ac))
        {
            addMultiEventList.Remove(ac);
        }
        else if (removeMultiEventList.Contains(ac))
        {
            removeMultiEventList.Remove(ac);
        }
    }
    
    public void ClearListener()
    {
        changeList.Clear();
        addEventList.Clear();
        removeEventList.Clear();
        addMultiEventList.Clear();
        removeMultiEventList.Clear();
        changeEventList.Clear();
        //Debug.Log("清理成功");
    }

    public void DispatchAll()
    {
        foreach (var item in changeList)
        {
            item();
        }
    }
    
    public void DispatchAddEvent(T t)
    {
        foreach (var item in addEventList)
        {
            item(t);
        }
    }
    
    public void DispatchRemoveEvent(T t)
    {
        foreach (var item in removeEventList)
        {
            item(t);
        }
    }
}

/// <summary>
/// Dic
/// 使用IReadOnlyDictionary作为保护
/// 通过Add RemoveByKey  Update  Clear 对数据进行操作
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindableDic<T,V>
{
    private List<Action> changeList = new List<Action>();
    //数据添加时，监听列表
    private List<Action<T,V>> addEventList = new List<Action<T,V>>();
    //数据移除时，监听列表
    private List<Action<T>> removeEventList = new List<Action<T>>();
    //单个数据修改时，监听列表
    private List<Action<T,V,V>> changeEventList = new List<Action<T,V,V>>();
    
    private Dictionary<T, V> _data;
    public IReadOnlyDictionary<T, V> data
    { get { return _data; } }

    public BindableDic()
    {
        _data = new Dictionary<T, V>();
    }
    
    public KeyValuePair<T,V> IndexOf(int index)
    {
        return _data.ElementAt(index);
    }
    
    public int Count()
    {
        return _data.Count;
    }
    
    public void Add(T t, V v)
    {
        _data.Add(t,v);
        DispatchAddEvent(t, v);
        DispatchAll();
    }

    public bool TryGetValue(T key, out V value)
    {
        return _data.TryGetValue(key, out value);
    }
    
    public V this[T key]
    {
        get { return _data[key]; }
        set
        {
            DispatchChangeListener(key,_data[key],value);
            _data[key] = value;
        }
    }
    
    public void Change(T t, V v)
    {
        if (!_data.ContainsKey(t))
        {
            Debug.LogError("键值不存在");
            return;
        }

        DispatchChangeListener(t, _data[t], v);
        _data[t] = v;
    }

    public void DispatchChangeListener(T t, V v, V v2)
    {
        DispatchChange(t, v, v2);
        DispatchAll();
    }
    
    public void RemoveByKey(T _key)
    {
        if (_data.ContainsKey(_key))
        {
            _data.Remove(_key);
            DispatchRemoveEvent(_key);
            DispatchAll();
        }
    }

    public void Clear()
    {
        _data.Clear();
        DispatchAll();
    }

    public void RemoveAll()
    {
        foreach (var one in _data.ToList())
        {
            RemoveByKey(one.Key);
        }
        DispatchAll();
    }
    
    public void Update(Dictionary<T,V> _dic)
    {
        _data = _dic;
        DispatchAll();
    }

    public void AddListener(Action ac)
    {
        if (!changeList.Contains(ac))
        {
            changeList.Add(ac);
            ac();//绑定时候执行下
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }

    public void AddListener_Add(Action<T,V> ac)
    {
        if (!addEventList.Contains(ac))
        {
            addEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void AddListener_Remove(Action<T> ac)
    {
        if (!removeEventList.Contains(ac))
        {
            removeEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void AddListener_Change(Action<T,V,V> ac)
    {
        if (!changeEventList.Contains(ac))
        {
            changeEventList.Add(ac);
        }
        else
        {
            Debug.LogWarning("重复监听");
        }
    }
    
    public void RemoveListener(Action ac)
    {
        if (changeList.Contains(ac))
        {
            changeList.Remove(ac);
        }
        else
        {
            Debug.LogWarning("移除的消息不存在");
        }
    }

    public void ClearListener()
    {
        changeList.Clear();
        addEventList.Clear();
        removeEventList.Clear();
        changeEventList.Clear();
        //Debug.Log("清理成功");
    }

    public void DispatchChange(T t, V oldv, V newv)
    {
        foreach (var item in changeEventList)
        {
            item(t,oldv,newv);
        }
    }
    
    public void DispatchAll()
    {
        foreach (var item in changeList)
        {
            item();
        }
    }
    
    public void DispatchAddEvent(T t, V v)
    {
        foreach (var item in addEventList)
        {
            item(t,v);
        }
    }
    
    public void DispatchRemoveEvent(T t)
    {
        foreach (var item in removeEventList)
        {
            item(t);
        }
    }
}
