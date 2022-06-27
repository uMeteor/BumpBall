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
    /// 获得当前场景主Ui
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
            Debug.Log("场景UI未找到" + currentSceneUI);
        }
                
        currentSceneUI.Init();
    }
 
}
