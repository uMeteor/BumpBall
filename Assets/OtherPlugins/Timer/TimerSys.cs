using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// (��ʱ��,���ڿͻ���)�Ľӿ�
/// </summary>
/// Ŀ�ꣻ֧��ʱ�䶨ʱ��֡��ʱ 
/// ��ʱ�����ѭ������ȡ�������滻
/// ʹ�ü򵥣���ʱ����
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
    /// ��ӽ�������
    /// </summary>                                              //��λĬ�Ϻ���
    public int AddTimerTake(double intervalTime, Action callback,int counts = 1, TimeUnti timeUnit=TimeUnti.MillSecond)
    {
      return pe.AddTimerTake(intervalTime, callback, counts, timeUnit);
    }
    /// <summary>
    /// ɾ������
    /// </summary>
    public bool DesTimerTake(int id)
    {
        return pe.DesTimerTake(id);
    }
    /// <summary>
    /// �滻����
    /// </summary>
    public bool ReplaceTimerTake(int id, double intervalTime, Action callback, TimeUnti timeUnit = TimeUnti.MillSecond, int counts = 1)
    {
        return pe.ReplaceTimerTake(id, intervalTime, callback, timeUnit, counts);
    }
    #endregion

    #region FrameTimeTask

    /// <summary>
    /// ��ӽ�������
    /// </summary>                                              //��λĬ�Ϻ���
    public int AddFrameTake(int intervalTime, Action callback, int counts = 1)
    {
        return pe.AddFrameTake(intervalTime, callback, counts);
    }

    /// <summary>
    /// ɾ������
    /// </summary>
    public bool DesFrameTake(int id)
    {
        return pe.DesFrameTake(id);
    }

    /// <summary>
    /// �滻����
    /// </summary>
    public bool ReplaceFrameTake(int id, int intervalTime, Action callback, int counts = 1)
    {
        return pe.ReplaceFrameTake(id, intervalTime, callback, counts); 
    }
    #endregion


}
