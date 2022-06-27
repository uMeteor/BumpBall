using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
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
    /// �������
    /// </summary>
    public void OnClickSys()
    {
        id= timerSys.AddTimerTake(50, FuncA, 0);
    }
    /// <summary>
    /// ɾ������
    /// </summary>
    public void DesClickSys()
    {
       bool isExist=  timerSys.DesTimerTake(id);
        Debug.Log("���"+id+"�����Ƿ����ɾ��" + isExist);
    }
    /// <summary>
    /// �滻����
    /// </summary>
    public void RaplaceClickSys()
    {
        bool isExist = timerSys.ReplaceTimerTake(id, 180, FuncB, 0) ;
        Debug.Log("���" + id + "�����Ƿ�����滻" + isExist);
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
