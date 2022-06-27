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
    public MainBall mMainBall;//���е��Ƿ��ǵ�ǰ����
    private UIFacade mUIFacade;
    public Standard standard;
    private ObjectPool mObjectPool;
    public CameraM cameraM;

    private string LevelID;
    private int moneyNum;
    private int turrentList = 0;
    public bool isOver;

    //���浱ǰ�ؿ��ĵ�ͼ����
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

    //��ҡ����ʧ��ʱ����ֵ����
    public void BallDirect()
    {
        mMainBall.ballNormalDir = mMouseHandle.nowHandlePosNormal;
        mMainBall.BallDirSwitch();
    }

    //��ȡ��ͼ�ļ�
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
        Debug.Log("�ļ��ӳ�ʧ�ܣ�ʧ��·��" + filePath);
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
            Debug.Log("�ļ��ӳ�ʧ�ܣ�ʧ��·��" + s);
        }
        Debug.Log("�ļ����سɹ�,·����" + s);

    }
#endif
    //����ȡ�ĵ�ͼ�ļ���������
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
    //���浱ǰ�ؿ�����
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

    //��Ǯ�������ʾ
    public void AddMoney(int money)
    {
        moneyNum += money;
    }
    public int DisPlayCurrMoney()
    {
        return moneyNum;
    }

    //��ʼʱ������˵�����
    public void AddTurrentList()
    {
        turrentList++;
    }
    //����ȫ�����˽���
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
    //ʤ��ʱ������
    IEnumerator DaleyScale()
    {
        yield return new WaitForSecondsRealtime(1f);
        mUIFacade.ShowGamePanel(GameShowPauseMode.Win);//��ʾʤ������
        //������һ��
        int s = PlayerPrefs.GetInt(DataName.LeveLUnLockIndex);
        if (s < 3)
        {
            PlayerPrefs.SetInt(DataName.LeveLUnLockIndex, s + 1);
        }
        Time.timeScale = 1;
        mObjectPool.PutObjectInPool(ObjectName.Ball, mMainBall.gameObject);
    }
    //������ȫ���ͻء������
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
