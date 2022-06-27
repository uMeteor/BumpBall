using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class GameControl : MonoBehaviour
{
    private static GameControl _intance;
    public static GameControl intance
    {
        get { return _intance; }
    }

    [HideInInspector]
    public MouseHandle mMouseHandle;
    [HideInInspector]
    public MainBall mMainBall;//持有的是否是当前的球
    private UIFacade mUIFacade;
    public Standard standard;
    private ObjectPool mObjectPool;
    public CameraM cameraM;

    private string LevelID;
    private int moneyNum;
    private int turrentList = 0;
    public bool isOver;

    //储存当前关卡的地图物体
    private Dictionary<ObjectName, List<GameObject>> allObjectDic = new Dictionary<ObjectName, List<GameObject>>();
    private void Awake()
    {
        _intance = this;
        mUIFacade = GameManager.intance.mUIFacade;
        mObjectPool = mUIFacade.mObjectPool;
        isOver = false;
        cameraM = Camera.main.GetComponent<CameraM>();
        standard = transform.Find("All_Standard").GetComponent<Standard>();
        mMouseHandle = GameObject.Find("Canvas/MouseHandle").GetComponent<MouseHandle>();
#if UNITY_STANDALONE_WIN
       MapInfoInCreat(LoadMapInfoFile());
#elif UNITY_ANDROID
        StartCoroutine(LoadMapInfoFile());
#endif
        mMouseHandle.init();


        EventCenter.AddListener(EventType.BallDir, BallDirect);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.BallDir, BallDirect);
        PlayerPrefs.SetInt(DataName.MoneyNum, PlayerPrefs.GetInt(DataName.MoneyNum) + moneyNum);
        OverAllPushPool();
    }

    //在摇杆消失的时给球赋值反向；
    public void BallDirect()
    {
        mMainBall.ballNormalDir = mMouseHandle.nowHandlePosNormal;
        mMainBall.BallDirSwitch();
    }

    //读取地图文件
#if UNITY_STANDALONE_WIN  
    public TransData LoadMapInfoFile()
    {
        LevelID = PlayerPrefs.GetString(DataName.LeveSeletString);
       
        string filePath = Application.dataPath + "/StreamingAssets/JsonFile/MapData" + LevelID+ ".json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();        
            TransData temp = JsonMapper.ToObject<TransData>(jsonStr);
            return temp;
        }
        Debug.Log("文件加成失败，失败路径" + filePath);
        return null;
    }
#elif UNITY_ANDROID
    IEnumerator  LoadMapInfoFile()
    {
        LevelID = PlayerPrefs.GetString(DataName.LeveSeletString);  
        string s = Application.streamingAssetsPath + "/JsonFile/MapData" + LevelID + ".json";

        //UnityWebRequest
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(s);
        yield return unityWebRequest.SendWebRequest();
        string jsonStr = unityWebRequest.downloadHandler.text;
        TransData trans = JsonMapper.ToObject<TransData>(jsonStr);
        MapInfoInCreat(trans);
        if (trans == null)
        {
            Debug.Log("文件加成失败，失败路径" + s);
        }
        Debug.Log("文件加载成功,路径：" + s);

    }
#endif
    //将读取的地图文件创建出来
    private void MapInfoInCreat(TransData transData)
    {
        for (int i = 0; i < transData.wallTransArray.Count; i++)
        {
            ToInfoGetObject(ObjectName.Wall, transData.wallTransArray, 0.5f, i);
        }
        for (int i = 0; i < transData.floorTransArray.Count; i++)
        {
            ToInfoGetObject(ObjectName.Floor, transData.floorTransArray, -0.5f, i);
        }
        for (int i = 0; i < transData.otherTransArray.Count; i++)
        {
            switch (transData.otherTransArray[i].name)
            {
                case ItemName.turretIt:
                    ToInfoGetObject(ObjectName.Turret, transData.otherTransArray, -0.25f, i);
                    break;
                case ItemName.moneyIt:
                    ToInfoGetObject(ObjectName.MoneyTrail, transData.otherTransArray, 0.5f, i);
                    break;
                case ItemName.swordIt:
                    ToInfoGetObject(ObjectName.Sword, transData.otherTransArray, 0.5f, i);
                    break;
                default:
                    break;
            }
        }
        GameObject ball = mObjectPool.GetObjectInPool(ObjectName.Ball);
        ball.transform.position = new Vector3((float)transData.mainBall.x, 0.5f, (float)transData.mainBall.z);
        ball.SetActive(true);
        mMainBall = ball.GetComponent<MainBall>();
        cameraM.Init();

    }
    //储存当前关卡物体
    private void ToInfoGetObject(ObjectName name, List<TranXY> xyList, float hight, int index)
    {
        GameObject t = mObjectPool.GetObjectInPool(name);
        if (!allObjectDic.ContainsKey(name))
        {
            allObjectDic.Add(name, new List<GameObject>());
        }
        allObjectDic[name].Add(t);
        if (name == ObjectName.Turret) { turrentList++; }
        t.transform.position = new Vector3((float)xyList[index].x, hight, (float)xyList[index].z);
        t.SetActive(true);
    }

    //金钱添加与显示
    public void AddMoney(int money)
    {
        moneyNum += money;
    }
    public int DisPlayCurrMoney()
    {
        return moneyNum;
    }

    //开始时计算敌人的数量
    public void AddTurrentList()
    {
        turrentList++;
    }
    //消灭全部敌人结束
    public void CutTurrenlist()
    {
        turrentList--;
        if (turrentList <= 0)
        {
            Time.timeScale = 0.3f;
            StartCoroutine(DaleyScale());
            if (mMainBall != null)
            {
                mMainBall.isWinOverGame = true;
            }
        }
    }
    //胜利时的慢放
    IEnumerator DaleyScale()
    {
        yield return new WaitForSecondsRealtime(1f);
        mUIFacade.ShowGamePanel(GameShowPauseMode.Win);//显示胜利界面
        //解锁下一关
        int s = PlayerPrefs.GetInt(DataName.LeveLUnLockIndex);
        if (s < 3)
        {
            PlayerPrefs.SetInt(DataName.LeveLUnLockIndex, s + 1);
        }
        Time.timeScale = 1;
        mObjectPool.PutObjectInPool(ObjectName.Ball, mMainBall.gameObject);
    }
    //结束·全部送回·对象池
    private void OverAllPushPool()
    {
        if (allObjectDic.Count < 1) return;
        foreach (var lis in allObjectDic)
        {
            for (int i = 0; i < lis.Value.Count; i++)
            {
                mObjectPool.PutObjectInPool(lis.Key, lis.Value[i]);
            }
            lis.Value.Clear();
        }
    }

}
