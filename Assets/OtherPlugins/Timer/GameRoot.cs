using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 启动入口
/// </summary>
public class GameRoot : MonoBehaviour
{
    private TimerSys timerSys;
    private int id;
    void Awake()
    {
        timerSys =GetComponent<TimerSys>();
        if (timerSys == null) Debug.Log(1234);
        timerSys.Init();
    }
    /// <summary>
    /// 添加任务
    /// </summary>
    public void OnClickSys()
    {
        id= timerSys.AddTimerTake(50, FuncA, 0);
    }
    /// <summary>
    /// 删除任务
    /// </summary>
    public void DesClickSys()
    {
       bool isExist=  timerSys.DesTimerTake(id);
        Debug.Log("编号"+id+"任务是否存在删除" + isExist);
    }
    /// <summary>
    /// 替换任务
    /// </summary>
    public void RaplaceClickSys()
    {
        bool isExist = timerSys.ReplaceTimerTake(id, 180, FuncB, 0) ;
        Debug.Log("编号" + id + "任务是否存在替换" + isExist);
    }
    void FuncA()
    {
        Debug.Log(22);
    }
    void FuncB()
    {
        Debug.Log(33);
    }
}
