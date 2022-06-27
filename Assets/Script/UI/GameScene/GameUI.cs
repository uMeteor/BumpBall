using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameUI : BaseSceneUI
{
    [HideInInspector]
    public MouseHandle mMouseHandle;
    [HideInInspector]
    public PausePanel pausePanel;
    private Button but_Pause;
    private Button but_Continue;
    void Awake()
    {
        mMouseHandle = transform.Find("MouseHandle").GetComponent<MouseHandle>();
        pausePanel = transform.Find("PausePanel").GetComponent<PausePanel>();
        but_Pause = transform.Find("but_Pause").GetComponent<Button>();
        but_Continue = transform.Find("but_Continue").GetComponent<Button>();
        but_Continue.gameObject.SetActive(false);

        but_Continue.onClick.AddListener(() =>
        {
            but_Pause.gameObject.SetActive(true);
            but_Continue.gameObject.SetActive(false);
            pausePanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        });
        but_Pause.onClick.AddListener(() =>
        {
            but_Pause.gameObject.SetActive(false);
            but_Continue.gameObject.SetActive(true);
            mUIFacade.ShowGamePanel(GameShowPauseMode.Pause);
            Time.timeScale = 0;
        });
    }
    public override void Init()
    {
        base.Init();
    }

}