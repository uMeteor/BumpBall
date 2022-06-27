using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;
    private ObjectPool mObjectPool;
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        mObjectPool = GameManager.intance.mUIFacade.mObjectPool;
    }
    public void Init()
    {
        rig.velocity = transform.right*speed;
       // GameManager.intance.mUIFacade.DaleyPushPoop(ObjectName.Bullet, gameObject);
    }
    private void Update()
    {
        if (GameControl.intance.isOver)
        {
            mObjectPool.PutObjectInPool(ObjectName.Bullet, gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            mObjectPool.PutObjectInPool(ObjectName.Bullet, gameObject);
        }
    }  
}
