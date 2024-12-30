using System;
using System.Collections;
using System.Collections.Generic;

public class EventSystemManager
{
    public delegate void ListenerDelegate();

    public delegate void ListenerDelegate<T>(T t);

    public delegate void ListenerDelegate<T1, T2>(T1 t1, T2 t2);

    public delegate void ListenerDelegate<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

    public delegate void ListenerDelegate<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

    private Dictionary<string, Delegate> listeners = new Dictionary<string, Delegate>();

    public EventSystemManager()
    {
        this.GetAllEventAuto();
    }

    #region Addlistener

    public void AddListener(string eventName, ListenerDelegate listener)
    {
        AddListener(eventName, listener as Delegate);
    }

    public void AddListener<T>(string eventName, ListenerDelegate<T> listener)
    {
        AddListener(eventName, listener as Delegate);
    }

    public void AddListener<T1, T2>(string eventName, ListenerDelegate<T1, T2> listener)
    {
        AddListener(eventName, listener as Delegate);
    }

    public void AddListener<T1, T2, T3>(string eventName, ListenerDelegate<T1, T2, T3> listener)
    {
        AddListener(eventName, listener as Delegate);
    }

    public void AddListener<T1, T2, T3, T4>(string eventName, ListenerDelegate<T1, T2, T3, T4> listener)
    {
        AddListener(eventName, listener as Delegate);
    }

    public void AddListener(string eventName, Delegate del)
    {
        Delegate listenerDelegate = listeners.ContainsKey(eventName) ? listeners[eventName] : null;
        listenerDelegate = Delegate.Combine(listenerDelegate, del);
        listeners[eventName] = listenerDelegate;
    }

    #endregion

    #region RemoveListener

    public void RemoveListener(string eventName, ListenerDelegate listener)
    {
        RemoveListener(eventName, listener as Delegate);
    }

    public void RemoveListener<T>(string eventName, ListenerDelegate<T> listener)
    {
        RemoveListener(eventName, listener as Delegate);
    }

    public void RemoveListener<T1, T2>(string eventName, ListenerDelegate<T1, T2> listener)
    {
        RemoveListener(eventName, listener as Delegate);
    }

    public void RemoveListener<T1, T2, T3>(string eventName, ListenerDelegate<T1, T2, T3> listener)
    {
        RemoveListener(eventName, listener as Delegate);
    }

    public void RemoveListener<T1, T2, T3, T4>(string eventName, ListenerDelegate<T1, T2, T3, T4> listener)
    {
        RemoveListener(eventName, listener as Delegate);
    }

    public void RemoveListener(string eventName, Delegate dDelegate)
    {
        Delegate remove = Delegate.Remove(listeners[eventName], dDelegate);
        SetRemoveValue(eventName, remove);
    }

    private void SetRemoveValue(string eventName, Delegate remove)
    {
        if (remove == null)
        {
            listeners.Remove(eventName);
        }
        else
        {
            listeners[eventName] = remove;
        }
    }

    #endregion

    public void RemoveAllListener(string eventName)
    {
        listeners.Remove(eventName);
    }

    public void Dispatch(string eventName)
    {
        if (listeners.ContainsKey(eventName))
        {
            ListenerDelegate listenerDelegate = listeners[eventName] as ListenerDelegate;
            listenerDelegate();
        }
    }

    public void Dispatch<T>(string eventName, T t)
    {
        if (listeners.ContainsKey(eventName))
        {
            ListenerDelegate<T> listenerDelegate = listeners[eventName] as ListenerDelegate<T>;
            listenerDelegate(t);
        }
    }

    public void Dispatch<T1, T2>(string eventName, T1 t1, T2 t2)
    {
        if (listeners.ContainsKey(eventName))
        {
            ListenerDelegate<T1, T2> listenerDelegate = listeners[eventName] as ListenerDelegate<T1, T2>;
            listenerDelegate(t1, t2);
        }
    }

    public void Dispatch<T1, T2, T3>(string eventName, T1 t1, T2 t2, T3 t3)
    {
        if (listeners.ContainsKey(eventName))
        {
            ListenerDelegate<T1, T2, T3> listenerDelegate = listeners[eventName] as ListenerDelegate<T1, T2, T3>;
            listenerDelegate(t1, t2, t3);
        }
    }

    public void Dispatch<T1, T2, T3, T4>(string eventName, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        if (listeners.ContainsKey(eventName))
        {
            ListenerDelegate<T1, T2, T3, T4> listenerDelegate = listeners[eventName] as ListenerDelegate<T1, T2, T3, T4>;
            listenerDelegate(t1, t2, t3, t4);
        }
    }
}