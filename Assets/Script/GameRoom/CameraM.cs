using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ö÷ÉãÏñ»ú¸úËæ
public class CameraM : MonoBehaviour
{
    public GameObject mMainBall;
    private Vector3 dic; 

    public void Init()
    {
        mMainBall = GameControl.intance.mMainBall.gameObject;
        dic = new Vector3(2f, 15, -6.3f - 1);
    }
    private void LateUpdate()
    {
        if (mMainBall == null) return;
        transform.position = mMainBall.transform.position+dic;
    }
}
