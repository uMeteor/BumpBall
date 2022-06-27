using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneUI : MonoBehaviour, IBaseSceneUI
{
    protected UIManager mUIManager;
    protected UIFacade mUIFacade;
    public virtual void  Init()
    {
        mUIFacade = GameManager.intance.mUIFacade;
        mUIManager = mUIFacade.mUIManager;
      
    }
}
