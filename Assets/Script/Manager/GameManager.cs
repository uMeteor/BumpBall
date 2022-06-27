using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _intance;
    public static GameManager intance
    {
        get { return _intance; }
    }

    public UIManager mUIManager;
    public UIFacade mUIFacade;
    public GameControl mGameControl;
    public ObjectPool mObjectPool;
    public VoiceManeger mVoiceManeger;

    public List<IBaseStateScene> sceneLists;//场景List
    public IBaseStateScene currentStateScene;//当前场景
    private int startDateTime,endDateTime;//用于程序关闭后计算体力的自动回复
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        _intance = this;      
        mUIFacade = new UIFacade();
        mVoiceManeger = new VoiceManeger();
        mUIManager = GetComponent<UIManager>();
        mObjectPool = GetComponent<ObjectPool>();

        mUIManager.InitUIManager();
        mVoiceManeger.Init();
        mUIFacade.InitFacade();

        //场景处理
        AddStateScene();
        currentStateScene = sceneLists[0];
        EnterScene();
        //数据处理
        DateInit();
        PlayerPrefs.SetInt(DataName.IsFristEnter, 1);//第一次进入游戏
        startDateTime = ConversionTime(DateTime.Now.ToLocalTime());
        endDateTime = PlayerPrefs.GetInt(DataName.EndDateNum);
     //   Debug.Log(endDateTime + "endDateTime     " + startDateTime + "startDateTime");
        AddPowerInClose();

    }

    float currentTime=0;
    float maxTime=30;//300 5分钟
    int curPowerNum = 0;
    private void Update()
    {
        currentTime += Time.deltaTime*1;
        //时间流逝时添加体力
        if (currentTime > maxTime)
        {
            currentTime = 0;
            curPowerNum = PlayerPrefs.GetInt(DataName.PowerNum);
            PlayerPrefs.SetInt(DataName.PowerNum, curPowerNum + 1 < PlayerPrefs.GetInt(DataName.MaxPowerNum)
                ? curPowerNum + 1 : PlayerPrefs.GetInt(DataName.MaxPowerNum));
           
            endDateTime = ConversionTime(DateTime.Now.ToLocalTime());//自动记录当前时间，用于下次开启游戏增加体力
            PlayerPrefs.SetInt(DataName.EndDateNum, endDateTime);
        }
    }

    //上线时按时间流逝添加体力
    private void AddPowerInClose()
    {
        if (endDateTime > startDateTime) return;
        else
        {
             
            int powerNum = PlayerPrefs.GetInt(DataName.PowerNum)+ startDateTime - endDateTime;
            if(powerNum> PlayerPrefs.GetInt(DataName.MaxPowerNum))
            {
                 powerNum = PlayerPrefs.GetInt(DataName.MaxPowerNum);
            }
            PlayerPrefs.SetInt(DataName.PowerNum, powerNum);
        }
    }
    //转化时间
    private int ConversionTime(DateTime dateTime)
    {
        int timeCode = dateTime.Minute / 5 + dateTime.Hour * 12 + dateTime.Day * 12 * 24;//以5分钟为一个单位
        return timeCode;
    }

    //数据初始化
    public void DateInit()
    {
        if (PlayerPrefs.GetInt(DataName.IsFristEnter) == 0)
        {
            PlayerPrefs.SetInt(DataName.IsFristEnter, 0);
            PlayerPrefs.SetInt(DataName.isReverseHandlep, 1);
            PlayerPrefs.SetInt(DataName.isShowRocker, 0);
            PlayerPrefs.SetInt(DataName.SoundValue, 1);

            PlayerPrefs.SetInt(DataName.MaxPowerNum, 5);
            PlayerPrefs.SetInt(DataName.MoneyNum, 50);
            PlayerPrefs.SetInt(DataName.PowerNum, 5);

            PlayerPrefs.SetInt(DataName.BallSkinIndex, 0);//目前选择的
            PlayerPrefs.SetInt(DataName.BallTrackIndex, 0);
            PlayerPrefs.SetInt(DataName.LeveLSmallIndex,1);
            PlayerPrefs.SetInt(DataName.LevelBigIndex, 1);
            PlayerPrefs.SetInt(DataName.LeveLUnLockIndex, 0);//已解锁关卡索引
            PlayerPrefs.SetInt(DataName.LevelBigIndex, 1);
            PlayerPrefs.SetString(DataName.BugBallSkinIndex, "0111");//解锁的
            PlayerPrefs.SetString(DataName.BugBallTrackIndex, "0111");

            PlayerPrefs.SetFloat(DataName.AudioValue, 1);
            PlayerPrefs.SetFloat(DataName.EffectSoundValue, 1);
        }

    }

    //切换场景有关设置
    private void AddStateScene()
    {
        LoadStateScene loadState = new LoadStateScene();
        MainStateScene mainState = new MainStateScene();
        GameStateScene gameState = new GameStateScene();
        sceneLists = new List<IBaseStateScene> { loadState, mainState, gameState };           
    }
    public void EnterScene()
    {
        currentStateScene.EnterScene();
    }
    public void ExitScene() 
    {
        currentStateScene.ExitScene();
    }
    public void SetScene(int index)
    {
       currentStateScene =sceneLists[index];
    } 
    public void AsyncLoadScene(int index)
    {
        mUIFacade.SetScene(index);
        StartCoroutine(AsyncScene(index));
    }
    IEnumerator AsyncScene(int index)
    {
                         //WaitForSeconds //受timeScale影响
        yield return new WaitForSecondsRealtime(0.1f);//按显示时间运算
        mUIFacade.EnterScene();
    }
    /// 开始场景加载后的异步获得
    public void EnterMainScene() 
    {
        StartCoroutine(DaleyScene());    
    }
    IEnumerator DaleyScene()
    {    
        yield return new WaitForSeconds(0.5f);
        mUIFacade.ExitScene();
    }

    //GameScene结束后，将物体送入对象池
    public void DaleyPushPoop(ObjectName name, GameObject g)
    {
        StartCoroutine(DaleyDes(name,g));
    }
    IEnumerator DaleyDes(ObjectName name, GameObject g)
    {
        yield return new WaitForSeconds(10);
        mObjectPool.PutObjectInPool(name, g);
    }
}
