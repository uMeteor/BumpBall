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

    public List<IBaseStateScene> sceneLists;//����List
    public IBaseStateScene currentStateScene;//��ǰ����
    private int startDateTime,endDateTime;//���ڳ���رպ�����������Զ��ظ�
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

        //��������
        AddStateScene();
        currentStateScene = sceneLists[0];
        EnterScene();
        //���ݴ���
        DateInit();
        PlayerPrefs.SetInt(DataName.IsFristEnter, 1);//��һ�ν�����Ϸ
        startDateTime = ConversionTime(DateTime.Now.ToLocalTime());
        endDateTime = PlayerPrefs.GetInt(DataName.EndDateNum);
     //   Debug.Log(endDateTime + "endDateTime     " + startDateTime + "startDateTime");
        AddPowerInClose();

    }

    float currentTime=0;
    float maxTime=30;//300 5����
    int curPowerNum = 0;
    private void Update()
    {
        currentTime += Time.deltaTime*1;
        //ʱ������ʱ�������
        if (currentTime > maxTime)
        {
            currentTime = 0;
            curPowerNum = PlayerPrefs.GetInt(DataName.PowerNum);
            PlayerPrefs.SetInt(DataName.PowerNum, curPowerNum + 1 < PlayerPrefs.GetInt(DataName.MaxPowerNum)
                ? curPowerNum + 1 : PlayerPrefs.GetInt(DataName.MaxPowerNum));
           
            endDateTime = ConversionTime(DateTime.Now.ToLocalTime());//�Զ���¼��ǰʱ�䣬�����´ο�����Ϸ��������
            PlayerPrefs.SetInt(DataName.EndDateNum, endDateTime);
        }
    }

    //����ʱ��ʱ�������������
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
    //ת��ʱ��
    private int ConversionTime(DateTime dateTime)
    {
        int timeCode = dateTime.Minute / 5 + dateTime.Hour * 12 + dateTime.Day * 12 * 24;//��5����Ϊһ����λ
        return timeCode;
    }

    //���ݳ�ʼ��
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

            PlayerPrefs.SetInt(DataName.BallSkinIndex, 0);//Ŀǰѡ���
            PlayerPrefs.SetInt(DataName.BallTrackIndex, 0);
            PlayerPrefs.SetInt(DataName.LeveLSmallIndex,1);
            PlayerPrefs.SetInt(DataName.LevelBigIndex, 1);
            PlayerPrefs.SetInt(DataName.LeveLUnLockIndex, 0);//�ѽ����ؿ�����
            PlayerPrefs.SetInt(DataName.LevelBigIndex, 1);
            PlayerPrefs.SetString(DataName.BugBallSkinIndex, "0111");//������
            PlayerPrefs.SetString(DataName.BugBallTrackIndex, "0111");

            PlayerPrefs.SetFloat(DataName.AudioValue, 1);
            PlayerPrefs.SetFloat(DataName.EffectSoundValue, 1);
        }

    }

    //�л������й�����
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
                         //WaitForSeconds //��timeScaleӰ��
        yield return new WaitForSecondsRealtime(0.1f);//����ʾʱ������
        mUIFacade.EnterScene();
    }
    /// ��ʼ�������غ���첽���
    public void EnterMainScene() 
    {
        StartCoroutine(DaleyScene());    
    }
    IEnumerator DaleyScene()
    {    
        yield return new WaitForSeconds(0.5f);
        mUIFacade.ExitScene();
    }

    //GameScene�����󣬽�������������
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
