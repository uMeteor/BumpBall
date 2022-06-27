using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ãæ°å»ùÀà
/// </summary>
public class BasePanel : MonoBehaviour, IBasePanel
{
    [HideInInspector]

    public  UIFacade mUIFacede;
  
    public virtual void EnterPanel()
    {
    }

    public virtual void ExitPanel()
    {
    }

    public virtual void InitPanel()
    {
        mUIFacede = GameManager.intance.mUIFacade;
       
    }
    public virtual void UpdatePanel()
    {
    }
}
