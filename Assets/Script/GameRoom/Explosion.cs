using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//±¬Õ¨ÌØÐ§
public class Explosion : MonoBehaviour
{
    private UIFacade uIFacade;
    void Start()
    {
        uIFacade = GameManager.intance.mUIFacade;
    }
    private void OnEnable()
    {
        StartCoroutine(Daley());
    }
    IEnumerator Daley()
    {
        yield return new WaitForSeconds(1.5f);
        uIFacade.mObjectPool.PutObjectInPool(ObjectName.Explosion, gameObject);      
    }
}
