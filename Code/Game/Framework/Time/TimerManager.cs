using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : IUpdate , IApplicationQuit
{
    private List<Timer> timerList = new(32);

    public TimerManager()
    {
        timerList.Clear();
    }
    
    public void Update()
    {
        for (int i = timerList.Count-1; i >= 0; i--)
        {
            timerList[i].Update();
        }
    }

    public Timer AddTimer(float duration ,Action excuteFunc)
    {
        return AddTimer(duration, 1, 0, excuteFunc, null, null);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">间隔时间</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="excuteFunc">执行方法</param>
    /// <returns></returns>
    public Timer AddTimer(float duration, int loopTimes,Action excuteFunc)
    {
        return AddTimer(duration, loopTimes, 0, excuteFunc, null, null);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">间隔时间</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="delayTime">延迟时间</param>
    /// <param name="excuteFunc">执行方法</param>
    /// <param name="completeFunc">完成回调方法</param>
    /// <param name="cancelFunc">取消回调方法</param>
    /// <returns></returns>
    public Timer AddTimer(float duration, int loopTimes, float delayTime, Action excuteFunc, Action completeFunc, Action cancelFunc)
    {
        //Debug.Log("添加计时器" + duration);
        Timer timer = new Timer(this);
        timer.Create(duration, loopTimes, delayTime, excuteFunc, completeFunc, cancelFunc);
        timerList.Add(timer);
        return timer;
    }

    public void ExcuteTimer(Timer timer)
    {
        if (timer == null)
        {
            return;
        }
        timerList.Remove(timer);
        if (timer.excuteFunc != null)
        {
            timer.excuteFunc();
        }
    }
    
    public void CompleteTimer(Timer timer)
    {
        if (timer == null)
        {
            return;
        }
        timerList.Remove(timer);
        if (timer.completeFunc != null)
        {
            timer.completeFunc();
        }
    }
    
    public void CancelTimer(Timer timer)
    {
        if (timer == null)
        {
            return;
        }
        timerList.Remove(timer);
        if (timer.cancelFunc != null)
        {
            timer.cancelFunc();
        }
    }

    public void CreateTimer(Dictionary<string, Timer> timerDic, string timerName, float time, int loopNum, Action ac)
    {
        bool has = false;
        if (timerDic.ContainsKey(timerName))
        {
            has = true;
            CancelTimer(timerDic[timerName]);
        }
        Timer t = AddTimer(time, loopNum, ac);
        if (!has)
        {
            timerDic.Add(timerName,t);
        }
        else
        {
            timerDic[timerName] = t;
        }
    }
    
    public void CreateTimer(Dictionary<string,Timer> timerDic,string timerName,float time,Action ac)
    {
        CreateTimer(timerDic, timerName, time, 1, ac);
    }
    
    public void CancelAllTime(Dictionary<string,Timer> timerDic)
    {
        foreach (var one in timerDic)
        {
            CancelTimer(one.Value);
        }
    }

    public void OnApplicationQuit()
    {
        timerList.Clear();
    }
}

/// <summary>
/// 使用说明
/// 1.首先定义                  Dictionary<string,Timer> timerDic = new Dictionary<string,Timer>();
/// 2.创建：                    timerDic.CreateTimer(timerDic, "timer1", 1, () => { Debug.Log("哈哈哈1"); });
/// 3.取消此字典所有计时器：     timerDic.CancelAllTime();
/// 4.取消单个计时器：           timerDic.CancelOneTime("timer1")
/// </summary>
public static class TimeHelp
{ 
    public static void CreateTimer(this Dictionary<string,Timer> timerDic ,string timeName ,float time, Action ac)
    {
        Game.TimerManager.CreateTimer(timerDic, timeName, time,1, ac);
    }
    
    public static void CreateTimer(this Dictionary<string,Timer> timerDic ,string timeName ,float time,int loopNum, Action ac)
    {
        Game.TimerManager.CreateTimer(timerDic, timeName, time, loopNum, ac);
    }
    
    public static void CancelAllTime(this Dictionary<string,Timer> timerDic)
    {
        Game.TimerManager.CancelAllTime(timerDic);
    }
    
    public static void CancelOneTime(this Dictionary<string,Timer> timerDic, string timerName)
    {
        if (timerDic.ContainsKey(timerName))
        {
            Game.TimerManager.CancelTimer(timerDic[timerName]);
        }
    }
}