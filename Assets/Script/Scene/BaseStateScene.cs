using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateScene : IBaseStateScene
{
    protected UIFacade mUIFacade;
    protected UIManager mUImanager;
    public virtual void EnterScene()
    {
        mUIFacade = GameManager.intance.mUIFacade;
        mUImanager = mUIFacade.mUIManager;
        mUIFacade.SetSceneUI();
    }
    public virtual void ExitScene()
    {
       
    }
}
