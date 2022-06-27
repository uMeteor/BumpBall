using System;
using System.Collections.Generic;



/// <summary>
/// 全栈使用定时器
/// </summary>
public class PETimer
{
    private Action<string> taskLog;
    private DateTime startDateTime = new DateTime(1997, 1, 1, 0, 0, 0, 0);//计算机元年
    private static readonly string obj = "lock";//readonly是一个修饰字段的关键字。被它修饰的字段只有在初始化或者构造函数中才能够赋值。
    private int tid = 0;//id索引

    private List<int> tidList = new List<int>(); //贮存任务id
    private List<int> tempTidlist = new List<int>();//用于删除tid的缓存

    private List<PETimeTask> timerTaskList = new List<PETimeTask>();//计时任务列表
    private List<PETimeTask> tempTaskList = new List<PETimeTask>();//缓存计时任务列表


    private int frameNum = 1;//帧数
    private List<PEFrameTask> frameTaskList = new List<PEFrameTask>();//计时任务列表
    private List<PEFrameTask> tempFrameTaskList = new List<PEFrameTask>();//缓存计时任务列表


    //初始化
    public PETimer()
    {
        tidList.Clear();
        tempTidlist.Clear();

        timerTaskList.Clear();
        tempTaskList.Clear();

        frameTaskList.Clear();
        tempFrameTaskList.Clear();

    }
    public void Update()
    {
        ChackTimeTask();
        ChackFrameTask();
        //在主逻辑后删除tid
        if (tempTidlist.Count > 0)
        {
            for (int i = 0; i < tempTidlist.Count; i++)
            {
                tidList.Remove(tempTidlist[i]);
            }
            tempTidlist.Clear();
        }
        if (frameNum == int.MaxValue) frameNum = 0;
        frameNum++;//帧加
    }

    #region TimeTask
    /// <summary>
    /// 普通时间检测逻辑
    /// </summary>
    private void ChackTimeTask()
    {
        //将缓存列表加入执行列表
        for (int i = 0; i < tempTaskList.Count; i++)
        {
            timerTaskList.Add(tempTaskList[i]);
        }
        tempTaskList.Clear();
        //判断列表中任务是否到时间，并执行
        for (int i = 0; i < timerTaskList.Count; i++)
        {
            PETimeTask pETime = timerTaskList[i];
            if (pETime.dealTime <= GetUTCMilliSeconds())
            {
                //异常检测
                Action ab = pETime.callback;
                try
                {
                    if (ab != null) ab();
                }
                catch (Exception e)
                {
                    SetLogInfo(e.ToString());
                }

                if (pETime.count == 0)//为零的时候无限循环
                {
                    pETime.dealTime += pETime.intervalTime;
                    return;
                }
                if (pETime.count == 1)
                {
                    tempTidlist.Add(pETime.id);
                    timerTaskList.RemoveAt(i);
                    i--; //防止后面的索引偏离
                }
                else
                {
                    pETime.dealTime += pETime.intervalTime;
                    pETime.count--;
                }
            }

        }

    }

    /// <summary>
    /// 添加进入数组
    /// </summary>                                              //单位默认毫秒
    public int AddTimerTake(double intervalTime, Action callback, int counts = 1, TimeUnti timeUnit = TimeUnti.MillSecond)
    {
        //单位换算
        intervalTime = SwitchTimeType(intervalTime, timeUnit);
        int id = GetId();
        //列表添加
        double delay = GetUTCMilliSeconds() + intervalTime;//一次计时到达时间
        tempTaskList.Add(new PETimeTask(id, delay, intervalTime, callback, counts));//先添加到缓存列表，然后在下一帧执行前再添加执行列表
        tidList.Add(id);
        return id;
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    public bool DesTimerTake(int id)
    {
        bool isDelete = false;
        bool isStay = false;
        if (tidList.Contains(id))
        {
            isStay = true;
        }
        if (isStay)
        {
            if (tempTaskList.Count > 0)
            {
                for (int i = 0; i < tempTaskList.Count; i++)
                {
                    if (tempTaskList[i].id == id)
                    {
                        tempTaskList.RemoveAt(i);
                        tidList.Remove(id);
                        isDelete = true;
                        break;
                    }
                }
            }
            for (int i = 0; i < timerTaskList.Count; i++)
            {
                if (timerTaskList[i].id == id)
                {
                    timerTaskList.RemoveAt(i);
                    tidList.Remove(id);
                    isDelete = true;
                    break;
                }
            }
        }
        return isDelete;
    }

    /// <summary>
    /// 替换任务
    /// </summary>
    public bool ReplaceTimerTake(int id, double intervalTime, Action callback, TimeUnti timeUnit = TimeUnti.MillSecond, int counts = 1)
    {
        SetLogInfo("shizhu");
        if (!tidList.Contains(id))
        {
            return false;
        }
        DesTimerTake(id);
        intervalTime = SwitchTimeType(intervalTime, timeUnit);
        //列表添加
        double delay = GetUTCMilliSeconds() + intervalTime; //一次计时到达时间
        tempTaskList.Add(new PETimeTask(id, delay, intervalTime, callback, counts));//先添加到缓存列表，然后在下一帧执行前再添加执行列表
        tidList.Add(id);
        return true;

    }
    #endregion

    #region FrameTimeTask
    /// <summary>
    /// 帧时间检测
    /// </summary>
    private void ChackFrameTask()
    {
        //将缓存列表加入执行列表
        for (int i = 0; i < tempFrameTaskList.Count; i++)
        {
            frameTaskList.Add(tempFrameTaskList[i]);
        }
        tempFrameTaskList.Clear();

        //判断列表中任务是否到时间，并执行
        for (int i = 0; i < frameTaskList.Count; i++)
        {
            PEFrameTask pEFrame = frameTaskList[i];
            if (pEFrame.frameTime <= frameNum)
            {
                //异常检测
                Action ab = pEFrame.callback;
                try
                {
                    if (ab != null) { ab(); }
                }
                catch (Exception e)
                {
                    SetLogInfo(e.ToString());
                }
                if (pEFrame.count == 0)//为零的时候无限循环
                {
                    pEFrame.frameTime += pEFrame.intervalTime;
                    return;
                }
                if (pEFrame.count == 1)
                {
                    tempTidlist.Add(pEFrame.id);
                    frameTaskList.RemoveAt(i);
                    i--; //防止后面的索引偏离
                }
                else
                {
                    pEFrame.frameTime += pEFrame.intervalTime;
                    pEFrame.count--;
                }
            }

        }

        //   Debug.Log( frameNum);

    }
    /// <summary>
    /// 添加进入数组
    /// </summary>                                              //单位默认毫秒
    public int AddFrameTake(int intervalTime, Action callback, int counts = 1)
    {
   
        int id = GetId();
        //列表添加
        int num = frameNum + intervalTime;
        tempFrameTaskList.Add(new PEFrameTask(id, num, intervalTime, callback, counts));//先添加到缓存列表，然后在下一帧执行前再添加执行列表
        tidList.Add(id);
        return id;
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    public bool DesFrameTake(int id)
    {
        bool isDelete = false;
        bool isStay = false;
        if (tidList.Contains(id))
        {
            isStay = true;
        }
        if (isStay)
        {
            if (tempFrameTaskList.Count > 0)
            {
                for (int i = 0; i < tempFrameTaskList.Count; i++)
                {
                    if (tempFrameTaskList[i].id == id)
                    {
                        tempFrameTaskList.RemoveAt(i);
                        tidList.Remove(id);
                        isDelete = true;
                        break;
                    }
                }
            }
            for (int i = 0; i < frameTaskList.Count; i++)
            {
                if (frameTaskList[i].id == id)
                {
                    frameTaskList.RemoveAt(i);
                    tidList.Remove(id);
                    isDelete = true;
                    break;
                }
            }
        }
        return isDelete;
    }

    /// <summary>
    /// 替换任务
    /// </summary>
    public bool ReplaceFrameTake(int id, int intervalTime, Action callback, int counts = 1)
    {
        if (!tidList.Contains(id))
        {
            return false;
        }
        DesFrameTake(id);
        //列表添加
        tempFrameTaskList.Add(new PEFrameTask(id, frameNum + intervalTime, intervalTime, callback, counts));//先添加到缓存列表，然后在下一帧执行前再添加执行列表
        tidList.Add(id);
        return true;

    }
    #endregion


    #region Tool
    /// <summary>
    /// 转换时间单位
    /// </summary>
    private double SwitchTimeType(double intervalTime, TimeUnti timeUnit)
    {
        if (timeUnit != TimeUnti.MillSecond)
        {
            switch (timeUnit)
            {
                case TimeUnti.Second:
                    intervalTime = intervalTime * 1000;
                    break;
                case TimeUnti.Minuta:
                    intervalTime = intervalTime * 1000 * 60;
                    break;
                case TimeUnti.Hour:
                    intervalTime = intervalTime * 1000 * 60 * 60;
                    break;
                case TimeUnti.Day:
                    intervalTime = intervalTime * 1000 * 60 * 60 * 24;
                    break;
                default:
                    break;
            }
        }
        return intervalTime;
    }

    /// <summary>
    /// 给任务生成一个全局全线程唯一id
    /// </summary>
    /// <returns></returns>
    private int GetId()
    {
        lock (obj)
        {
            tid++;
            if (tid == int.MaxValue)//安全保护
            {
                tid = 0;
            }
            while (tidList.Contains(tid))
            {
                tid++;
            }
        }
        return tid;
    }
    /// <summary>
    /// 不同环境设置输出格式
    /// </summary>
    /// <param name="log"></param>
    public void SetLog(Action<string> log)
    {
        taskLog = log;
    }

    /// <summary>
    /// 输出日志
    /// </summary>
    /// <param name="info"></param>
    public void SetLogInfo(string info)
    {
        if (taskLog != null)
        {
            taskLog(info);
        }
    }

    /// <summary>
    /// 获得世界时间差
    /// </summary>
    /// <returns></returns>
    private double GetUTCMilliSeconds()
    {                //世界标准时间
        TimeSpan ts = DateTime.UtcNow - startDateTime;
        return ts.TotalMilliseconds;
    }
    #endregion
}
