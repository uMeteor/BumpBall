using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SetPanel : BasePanel
{
    private Slider setVoiceValue;//音乐
    private Slider setEffectValue;//音效        
    private Toggle setReverse;
    private Toggle setShowRocker;
    private Button but_clearDate, but_onDate, but_offDate;
    private GameObject clearTipPanel;
    public override void InitPanel()
    {
        base.InitPanel();

        setVoiceValue = GameObject.Find("sil_Mousic").GetComponent<Slider>();
        setEffectValue = GameObject.Find("sil_Effect").GetComponent<Slider>();
        setReverse = GameObject.Find("tog_Reverse").GetComponent<Toggle>();
        setShowRocker = GameObject.Find("tog_Rocker").GetComponent<Toggle>();
        but_clearDate = GameObject.Find("but_ClearDate").GetComponent<Button>();
        but_onDate = GameObject.Find("ClearTipPanel/Button").GetComponent<Button>();
        but_offDate = GameObject.Find("ClearTipPanel/Button1").GetComponent<Button>();
        clearTipPanel = GameObject.Find("ClearTipPanel");

        setVoiceValue.onValueChanged.AddListener(AdjustVoice);
        setEffectValue.onValueChanged.AddListener(AdjustEffecSound);
        InitUI();

        setReverse.onValueChanged.AddListener((bool isRever) =>
        {
            if (isRever) PlayerPrefs.SetInt(DataName.isReverseHandlep, 0);
            else PlayerPrefs.SetInt(DataName.isReverseHandlep, 1);
        });
        setShowRocker.onValueChanged.AddListener((bool isRever) =>
        {
            if (isRever) PlayerPrefs.SetInt(DataName.isShowRocker, 0);
            else PlayerPrefs.SetInt(DataName.isShowRocker, 1);
        });

        but_clearDate.onClick.AddListener(() =>
        {
            clearTipPanel.SetActive(true);
        });
        but_onDate.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(DataName.IsFristEnter, 0);
            SceneManager.LoadScene(0);
        });
        but_offDate.onClick.AddListener(() =>
        {
            clearTipPanel.SetActive(false);
        });
        clearTipPanel.SetActive(false);
    }
    private void InitUI()
    {
        setVoiceValue.value = PlayerPrefs.GetFloat(DataName.AudioValue);
        setEffectValue.value= PlayerPrefs.GetFloat(DataName.EffectSoundValue);

        if (PlayerPrefs.GetInt(DataName.isReverseHandlep) == 0) setReverse.isOn = true;
        else setReverse.isOn = false;
        if (PlayerPrefs.GetInt(DataName.isShowRocker) == 0) setShowRocker.isOn = true;
        else setShowRocker.isOn = false;
    }

    private void AdjustVoice(float value)
    {
        mUIFacede.AdjustVoice(value);
    }
    private void AdjustEffecSound(float value)
    {
        mUIFacede.AdjustEffecSound(value);
    }
    
    //进出界面
    public void MovePanel()
    {
       transform.DOLocalMoveX(0, 0.5f);      
    }
    public void BackPanel()
    {
       transform.DOLocalMoveX(480, 0.5f);
    }
}
