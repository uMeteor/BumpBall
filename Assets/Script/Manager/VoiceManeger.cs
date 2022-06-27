using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceClip
{
    public string
    ballBomp = "BallBomp",
    ballExplosion = "BallExplosion",
    turrentBomb = "TurrentBomb",
    backgroundMusic = "BackgroundMusic",
    fastMove = "FastMove";

}
public class VoiceManeger
{
    //背景音乐
    //按钮点击
    //球撞击声音
    //

    private VoiceClip voiceClip;
    private AudioSource[] audioSources;//0 音乐 1音效
    public Dictionary<string, AudioClip> keyValueVoice;
    public void Init()
    {
        voiceClip = new VoiceClip();
        keyValueVoice = new Dictionary<string, AudioClip>();
        audioSources= GameManager.intance.GetComponents<AudioSource>();
        //获得clip\
        AddClip();
        AdjustEffecSound(PlayerPrefs.GetFloat(DataName.EffectSoundValue));
        AdjustVoice(PlayerPrefs.GetFloat(DataName.AudioValue));
    }

    //从文件中加载AudioClip
    private void AddClip()
    {

        AudioClip audio = Resources.Load<AudioClip>("AudioCilp/" + voiceClip.backgroundMusic);
        keyValueVoice.Add(voiceClip.backgroundMusic, audio);
        audio = Resources.Load<AudioClip>("AudioCilp/" + voiceClip.ballExplosion);
        keyValueVoice.Add(voiceClip.ballExplosion, audio);
        audio = Resources.Load<AudioClip>("AudioCilp/" + voiceClip.turrentBomb);
        keyValueVoice.Add(voiceClip.turrentBomb, audio);
        audio = Resources.Load<AudioClip>("AudioCilp/" + voiceClip.ballBomp);
        keyValueVoice.Add(voiceClip.ballBomp, audio);
        audio = Resources.Load<AudioClip>("AudioCilp/" + voiceClip.fastMove);
        keyValueVoice.Add(voiceClip.fastMove, audio);
    }

    /// 播放背景音乐
    public void PlayBGMusic()
    {    
        if (!audioSources[0].isPlaying)
        {
            audioSources[0].clip = keyValueVoice[voiceClip.backgroundMusic];
            audioSources[0].Play();
        }
    }
   /// 播放音效
    public void PlayEffectMusic(AudioClip audioClip)
    {     
        audioSources[1].PlayOneShot(audioClip);        
    }   
    //
    //设置音量
    public void AdjustVoice(float value)
    {
        audioSources[0].volume = value;
        PlayerPrefs.SetFloat(DataName.AudioValue,value);     
    }
    public void AdjustEffecSound(float value)
    {     
        audioSources[1].volume = value;
        PlayerPrefs.SetFloat(DataName.EffectSoundValue, value);
    }


    /// <summary>
    /// 方便外部调用的按钮音
    /// </summary>
    //public void PlayButtonAudioClip()
    //{
    //  //  PlayEffectMusic(GameManager.Instance.GetAudioClip("Main/Button"));
    //}
    //public void PlayPagingAudioClip()
    //{
    //   // PlayEffectMusic(GameManager.Instance.GetAudioClip("Main/Paging"));
    //}
}
