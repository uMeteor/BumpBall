using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBall : MonoBehaviour
{

    private int startSpeed = 10;
    public Vector3 ballNormalDir = Vector3.zero;//Ϊ�������Ļ����
    protected Vector3 ballRightDir = Vector3.zero;
    private Vector3 pathPos;
    private bool isMoney;
    private bool isSpeed;
    [HideInInspector]
    public bool isWinOverGame;//��Ϸʤ��

    private UIFacade mUIFacade;
    private ObjectPool mObjectPool;
    private Rigidbody rig;
    protected MeshRenderer meshRenderer;

    public Material[] ballMatereial;
    private List<GameObject> trailList;
    private TrailRenderer currTrailRender;
    private GameObject ballExplosion;
    private void Awake()
    {
        EventCenter.AddListener<bool>(EventType.MoneyTrailTo, (bool isM) => isMoney = isM);
        mUIFacade = GameManager.intance.mUIFacade;
        mObjectPool = mUIFacade.mObjectPool;
        rig = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailList = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            trailList.Add(GameObject.Find("Trail" + i.ToString()));
        }
    
    }
    private void OnEnable()
    {
        meshRenderer.material = ballMatereial[PlayerPrefs.GetInt(DataName.BallSkinIndex)];
        SetTrack(PlayerPrefs.GetInt(DataName.BallTrackIndex));
        ballNormalDir = Vector3.zero;//Ϊ�������Ļ����
        ballRightDir = Vector3.zero;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        currTrailRender.Clear();
        isWinOverGame = false;
       
    }
    void Update()
    {
        if (isMoney)
        {
            //ΪǮָ��λ��
            EventCenter.Broadcast<Vector3>(EventType.MoneyTrailBack, transform.position);
        }
        //��������
        if (GameManager.intance.currentStateScene != GameManager.intance.sceneLists[2])
        {
            mObjectPool.PutObjectInPool(ObjectName.Ball, gameObject);
        }
    }
    private void FixedUpdate()
    {    
        //���˶�
        if (ballRightDir != Vector3.zero && !isSpeed)
        {
            rig.velocity = ballRightDir * startSpeed;
        }
        else if (isSpeed)
        {
            rig.velocity = pathPos * startSpeed * 2;
        }
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.MoneyTrailTo, (bool isM) => isMoney = isM);
    }

    //������β
    protected void SetTrack(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            trailList[i].SetActive(false);
        }
        currTrailRender = trailList[index].GetComponent<TrailRenderer>();
        trailList[index].SetActive(true); ;
    }

    //�Է������ݽ��д���
    public void BallDirSwitch()
    {
        if (ballNormalDir != Vector3.zero)
        {
            ballRightDir.x = ballNormalDir.x;
            ballRightDir.y = 0;
            ballRightDir.z = ballNormalDir.y;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ballRightDir = Vector3.Reflect(ballRightDir, collision.contacts[0].normal);
            mUIFacade.PlayEffectMusic(mUIFacade.keyValueVoice["BallBomp"]);
        }
        if (collision.gameObject.CompareTag("Die"))//�ж���
        {
            GameOver();
        }
    }

    //��ը
    private void GameOver()
    {
        if (isWinOverGame) return;
        ballExplosion = mObjectPool.GetObjectInPool(ObjectName.Explosion);
        ballExplosion.transform.position = transform.position;
        ballExplosion.SetActive(true);

        mUIFacade.PlayEffectMusic(mUIFacade.keyValueVoice["BallExplosion"]);//boom����
        mUIFacade.ShowGamePanel(GameShowPauseMode.Loser);//ʧ�ܽ���
        mObjectPool.PutObjectInPool(ObjectName.Ball, gameObject);

    }

    private void OnTriggerStay(Collider other)
    {
        //���ٴ�
        if (other.CompareTag("SpeedPath"))
        {
            isSpeed = true;
            Vector3 temo = (other.transform.position - transform.position).normalized;
            temo.y = 0;
            pathPos = temo;
            if (Vector3.Distance(other.transform.position, transform.position) < 0.6f)
            {
                isSpeed = false;
            }
        }
        if (other.CompareTag("Die"))//�ж��ӵ�
        {
            GameOver();
        }
    }


}
