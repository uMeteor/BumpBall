
using UnityEngine;


//实体钱
public class MoneyTrail : MonoBehaviour
{
    private Vector3 ballPos;//球的实时位置
    private Vector3 force;   
    private ObjectPool mObjectPool;
    private Rigidbody rig;
    public float speed;
    private float delayTime;
    private float time;
    public bool gameOver;
    private void Awake()
    {
        mObjectPool = GameManager.intance.mUIFacade.mObjectPool;
        rig = GetComponent<Rigidbody>();
        delayTime = 1;
        time = 0;
        EventCenter.AddListener<Vector3>(EventType.MoneyTrailBack, (Vector3 Pos) => ballPos = Pos);
        EventCenter.Broadcast<bool>(EventType.MoneyTrailTo, true);
        EventCenter.AddListener(EventType.ToMoneyTrailPool, ToObjbectPool); //游戏结束回收钱OBject

    }
    private void OnDestroy()
    {
        EventCenter.Broadcast<bool>(EventType.MoneyTrailTo, false);
        EventCenter.RemoveListener<Vector3>(EventType.MoneyTrailBack, (Vector3 Pos) => ballPos = Pos);
        EventCenter.AddListener(EventType.ToMoneyTrailPool, ToObjbectPool);
    }
    public void OnEnable()
    {
        time = 0;
    }
    private void Update()
    {
        if (time < delayTime)
        {
            time += Time.deltaTime;
            force = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)).normalized;
            rig.AddForce(force * speed);
        }
        else
        {
            force = (ballPos - transform.position).normalized;
           // Debug.Log(ballPos);
            rig.velocity = force * speed;
        }
     
    }
    private void ToObjbectPool()
    {
        mObjectPool.PutObjectInPool(ObjectName.MoneyTrail, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {             
            ToObjbectPool();                      
        }
    }
}
