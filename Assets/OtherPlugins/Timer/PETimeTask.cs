using System;
/// <summary>
///  定时任务数据类.普通时间类定时
/// </summary>
public class PETimeTask
{
    public int id;
    public double dealTime;//执行时间 ，单位：毫秒
    public double intervalTime;//间隔时间
    public Action callback;//执行任务
    public int count;//执行次数，0就是一直循环
    public PETimeTask(int id, double dealTime, double intervalTime, Action callback, int count)
    {
        this.id = id;
        this.dealTime = dealTime;
        this.intervalTime = intervalTime;
        this.callback = callback;
        this.count =count;
    }
}

/// <summary>
/// 帧时间类定时
/// </summary>
public class PEFrameTask
{
    public int id;
    public int frameTime;//执行时间 ，单位：帧
    public int intervalTime;//间隔帧数
    public Action callback;//执行任务
    public int count;//执行次数，0就是一直循环
    public PEFrameTask(int id, int frameTime, int intervalTime, Action callback, int count)
    {
        this.id = id;
        this.frameTime = frameTime;
        this.intervalTime = intervalTime;
        this.callback = callback;
        this.count = count;
    }
}

/// <summary>
/// 时间单位
/// </summary>
public enum TimeUnti
{
    MillSecond,
    Second,
    Minuta,
    Hour,
    Day
}

