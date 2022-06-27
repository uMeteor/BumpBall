using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateScene : BaseStateScene
{
    public override void EnterScene()
    {
        base.EnterScene();
    }
    public override void ExitScene()
    {
        SceneManager.LoadScene(1);
        GameManager.intance.AsyncLoadScene(1);
    }
}
