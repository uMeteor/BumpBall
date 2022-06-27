using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�н飬�ϲ�������������������²���UI�����н���
/// </summary>
public class UIFacade
{
    public UIManager mUIManager;
    public GameManager mGameManager;
    public ObjectPool mObjectPool;
    public Dictionary<string, AudioClip> keyValueVoice;
    public VoiceManeger mVoiceManeger;
    // public DataCentre mDataCentre;
    public void InitFacade()
    {
        mGameManager = GameManager.intance;
        mUIManager = mGameManager.mUIManager;
        mObjectPool = mGameManager.mObjectPool;
        mVoiceManeger = mGameManager.mVoiceManeger;
        keyValueVoice =mVoiceManeger.keyValueVoice;
    }
    public void IsHideLevelPanel(bool on_off)
    {
        mUIManager.mMainUI.mLevelPanel.IsHideLevelPanel(on_off);
    }
    public void BallDirect()
    {
        mGameManager.mGameControl.BallDirect();
    }
    /// <summary>
    /// ������һ������
    /// </summary>
    public void EnterScene()
    {
        mGameManager.EnterScene();
    }
    public void ExitScene()
    {
        mGameManager.ExitScene();
    }
    /// <summary>
    /// ������һ������
    /// </summary>
    /// <param name="index"></param>
    public void SetScene(int index)
    {
        mGameManager.SetScene(index);
    }
    public void SetSceneUI()
    {
        mUIManager.SetSceneUI();
    }
    public void EnterMainScene()
    {
        mGameManager.EnterMainScene();
    }
    public void AsyncLoadScene(int index)
    {
        mGameManager.AsyncLoadScene(index);
    }
    public string GetLevelID()
    {
        return mGameManager.mUIManager.mMainUI.GetLevelID(); 
    }
    public void ShowGamePanel(GameShowPauseMode mode)
    {
        mGameManager.mUIManager.mGameUI.pausePanel.ShowGamePanel(mode);
    }
    public void AdjustVoice(float value)
    {
        mVoiceManeger.AdjustVoice(value);
    }
    public void AdjustEffecSound(float value)
    {
       mVoiceManeger.AdjustEffecSound(value);
    }
    public void PlayEffectMusic(AudioClip audioClip)
    {
       mVoiceManeger.PlayEffectMusic(audioClip);
    }
    public void ShowTipPanel()
    {
        mUIManager.mMainUI.ShowTipPanel();
    }
    public void UpdatePowerAndMoney()
    {
        mUIManager.mMainUI.UpdatePowerAndMoney();
    }
    public void DaleyPushPoop(ObjectName name, GameObject g)
    {
        mGameManager.DaleyPushPoop(name, g);
    }

}
