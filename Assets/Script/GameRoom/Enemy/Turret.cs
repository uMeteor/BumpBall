using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    private ObjectPool mObjectPool;
    public Image attackCdUI;
    public GameObject bullet;
    private GameObject turretMouth;
    private GameObject bulletLaunchDot;
    private GameControl gameControl;
    private UIFacade mUIFacade;
    private BallPosTest ballPosTest;

//    private Vector3 toBallPos;//朝向
    private Vector3 firstToBallPos=Vector3.zero;//球进入时的位置
   // private bool isBallInRange;
    private bool isFirstBallInRange;
    public bool isFire;

    public float launchCD;
    private float curentCD;
    private float attackUIValue;
    public int moneyValue;
    private void Awake()
    {
        mUIFacade = GameManager.intance.mUIFacade;
        gameControl = GameControl.intance;
        bulletLaunchDot = transform.Find("Mouth/LaunchPos").gameObject;
        turretMouth = transform.Find("Mouth").gameObject;
        attackCdUI = transform.Find("CdShow_Canvas/TurretCD/CD").GetComponent<Image>();
        mObjectPool = GameManager.intance.mUIFacade.mObjectPool;
        ballPosTest = transform.Find("BallTest").GetComponent<BallPosTest>();
    }

    private void OnEnable()
    {
        isFire = false;
    }
    private void Update()
    {
        SpinTurret();
        TimeClock();
    }

    //攻击CD
    private void TimeClock()
    {
        if (!isFire)
        {
            curentCD += Time.deltaTime;
            attackUIValue = curentCD / launchCD;//设置值
            if (attackUIValue >= 1)
            {
                isFire = true;
                attackUIValue = 1;
            }
            attackCdUI.fillAmount = attackUIValue;
        }
    }

    //旋转塔
    private void SpinTurret()
    {
    
        if (ballPosTest.IsBallInRange)
         {
            //旋转炮塔
            Vector3 temp = ballPosTest.ToBallPos - turretMouth.transform.position;          
            float angle = Mathf.Atan2(-temp.x, -temp.z) * Mathf.Rad2Deg + turretMouth.transform.eulerAngles.z + 90;
            turretMouth.transform.eulerAngles = new Vector3(-90, 0, angle);
       
            LauncBullet();
        }  
    }
    private void LauncBullet()
    {
        if (isFire)
        {           
            GameObject g= mObjectPool.GetObjectInPool(ObjectName.Bullet);
            g.transform.position = bulletLaunchDot.transform.position;
            g.transform.rotation = bulletLaunchDot.transform.rotation;
            g.GetComponent<Bullet>().Init();
            g.SetActive(true);
            isFire = false;
            curentCD = 0;
        }
    }
    
    //被撞
    private void OnCollisionEnter(Collision collision)
    {    
        if (collision.gameObject.CompareTag("Player"))
        {          
            for (int i = 0; i < moneyValue; i++)
            {   //产钱    
                GameObject g= mObjectPool.GetObjectInPool(ObjectName.MoneyTrail);
                g.transform.position = transform.position;
                g.SetActive(true); 
            }                    
            DieTurret();
        }
    }
    private void DieTurret()
    {
        //爆炸
        GameObject g1 = mObjectPool.GetObjectInPool(ObjectName.Explosion);
        g1.transform.position = transform.position;
        g1.SetActive(true);
        GameControl.intance.CutTurrenlist();
        mUIFacade.PlayEffectMusic(mUIFacade.keyValueVoice["TurrentBomb"]);//音效
        GameControl.intance.AddMoney(Random.Range(moneyValue,moneyValue+3));//加钱
        mObjectPool.PutObjectInPool(ObjectName.Turret, gameObject);

    }

    //球进入时先转到锁定位置（待定）
    private void FirstSpinTurret()
    {
        if (isFirstBallInRange)
        {
            Vector3 temp = firstToBallPos - turretMouth.transform.position;
            float angle = Mathf.Atan2(-temp.x, -temp.z) * Mathf.Rad2Deg + turretMouth.transform.eulerAngles.z + 90;
            float t = Mathf.Lerp(turretMouth.transform.eulerAngles.z, angle, Time.deltaTime * 10);
            //Debug.Log(t+"|"+ angle);
            turretMouth.transform.eulerAngles = new Vector3(-90, 0, t);
            if (turretMouth.transform.eulerAngles == new Vector3(-90, 0, angle))
            {
                Debug.Log(1);
                isFirstBallInRange = false;
            }
        }
    }
}
