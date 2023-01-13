using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private TimerManager _timerManager;
    public float duration;
    public int loopTimes;
    public float delayTime;
    public Action completeFunc;
    public Action excuteFunc;
    public Action cancelFunc;
    private float _startTime;

    public Timer(TimerManager tm)
    {
        _timerManager = tm;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="loopTimes">循环次数</param>
    /// <param name="delayTime">第一次执行的延时</param>
    /// <param name="excuteFunc">执行方法</param>
    /// <param name="completeFunc">完成方法用于循环多次最终完成执行</param>
    /// <param name="cancelFunc">取消方法</param>
    public void Create(float duration, int loopTimes, float delayTime, Action excuteFunc, Action completeFunc, Action cancelFunc)
    {
        this.duration = duration;
        this.loopTimes = loopTimes;
        this.delayTime = delayTime;
        this.completeFunc = completeFunc;
        this.excuteFunc = excuteFunc;
        this.cancelFunc = cancelFunc;

        float curTime = GetTime();
        _startTime = curTime + duration + delayTime;
    }

    // Update is called once per frame
    public void Update()
    {
        float curTime = GetTime();
        if (curTime >= _startTime)
        {
            excuteFunc();
            --loopTimes;
            if (loopTimes > 0)
            {
                _startTime = curTime + duration;
            }
            else
            {
                _timerManager.CompleteTimer(this);
            }
        }
    }
    
    private float GetTime()
    {
        return Time.time;
    }
}
