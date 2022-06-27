using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// (计时器,基于客户端)改接口
/// </summary>
/// 目标；支持时间定时，帧定时 
/// 定时任务可循环，可取消，可替换
/// 使用简单，定时方便
public class TimerSys : MonoBehaviour
{
    private PETimer pe;
    public void Init()
    {

        pe = new PETimer();
        pe.SetLog((string info ) =>
        {
            Debug.Log("PETimerLog:" + info);
        });

    }
    private void Update()
    {
        pe.Update();
    }
    #region TimeTask
    /// <summary>
    /// 添加进入数组
    /// </summary>                                              //单位默认毫秒
    public int AddTimerTake(double intervalTime, Action callback,int counts = 1, TimeUnti timeUnit=TimeUnti.MillSecond)
    {
      return pe.AddTimerTake(intervalTime, callback, counts, timeUnit);
    }
    /// <summary>
    /// 删除任务
    /// </summary>
    public bool DesTimerTake(int id)
    {
        return pe.DesTimerTake(id);
    }
    /// <summary>
    /// 替换任务
    /// </summary>
    public bool ReplaceTimerTake(int id, double intervalTime, Action callback, TimeUnti timeUnit = TimeUnti.MillSecond, int counts = 1)
    {
        return pe.ReplaceTimerTake(id, intervalTime, callback, timeUnit, counts);
    }
    #endregion

    #region FrameTimeTask

    /// <summary>
    /// 添加进入数组
    /// </summary>                                              //单位默认毫秒
    public int AddFrameTake(int intervalTime, Action callback, int counts = 1)
    {
        return pe.AddFrameTake(intervalTime, callback, counts);
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    public bool DesFrameTake(int id)
    {
        return pe.DesFrameTake(id);
    }

    /// <summary>
    /// 替换任务
    /// </summary>
    public bool ReplaceFrameTake(int id, int intervalTime, Action callback, int counts = 1)
    {
        return pe.ReplaceFrameTake(id, intervalTime, callback, counts); 
    }
    #endregion


}
