using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainStateScene : BaseStateScene
{
    public override void EnterScene()
    {
        base.EnterScene();
    }
    public override void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(2);
        mUIFacade.AsyncLoadScene(2);
    }
}
