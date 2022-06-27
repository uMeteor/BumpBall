using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public LoadUI mloadUI;
    public MainUI mMainUI;
    public GameUI mGameUI;
    public IBaseSceneUI currentSceneUI;
    public void InitUIManager()
    {    
       // DontDestroyOnLoad(gameObject)
    }
    /// <summary>
    /// ��õ�ǰ������Ui
    /// </summary>
    public void SetSceneUI()
    {
        currentSceneUI = GameObject.Find("Canvas").GetComponent<IBaseSceneUI>();
        string currentName = currentSceneUI.GetType().ToString();
        if (currentName == "LoadUI")
        {
            mloadUI = currentSceneUI as LoadUI;
        }          
        else if (currentName == "MainUI")
        {
            mMainUI = currentSceneUI as MainUI;         
        }
        else if (currentName == "GameUI")
        {
            mGameUI = currentSceneUI as GameUI;
        }
        else
        {
            Debug.Log("����UIδ�ҵ�" + currentSceneUI);
        }
                
        currentSceneUI.Init();
    }
 
}
