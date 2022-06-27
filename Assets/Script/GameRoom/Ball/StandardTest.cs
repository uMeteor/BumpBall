using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardTest : MonoBehaviour
{

    public Collision collider1;


    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(1);

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log(1);
            collider1 = collision;
            GameControl.intance.standard.isCollWall = true;
        }
        else
        {
            collider1 = null;
            GameControl.intance.standard.isCollWall = false;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Wall"))
    //    {
    //        collider1 = other;
    //        GameControl.intance.standard.isCollWall = true;
    //    }
    //    else
    //    {
    //        collider1 = null;
    //        GameControl.intance.standard.isCollWall = false;
    //    }
    //}

}
