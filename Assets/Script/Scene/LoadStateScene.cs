using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStateScene : BaseStateScene
{
    public override void EnterScene()
    {
        base.EnterScene();
        mUIFacade.SetScene(0);  
    }

    public override void ExitScene()
    {
        mUIFacade.AsyncLoadScene(1);      
    }
}
