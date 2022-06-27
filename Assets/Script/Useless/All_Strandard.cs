using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Strandard : MonoBehaviour
{
    private GameObject largStandard, litStandard;
    private RectTransform largRect, litRect;
  //private GameControl gameControl;
    private Transform piv;//Larg的顶点位置
    private StandardTest standardTest;

    private float standardDic;//角度值
    private float longs;//比例长度
    private float maxLong = 1500;
    private float transLong = 3.76f;//世界坐标长度
    private Vector3 largeRot;
    public bool isCollWall;

    private void Awake()
    {
    //  gameControl = GameControl.intance;
        largStandard = GameObject.Find("Standard");
        litStandard = GameObject.Find("standard");
        piv = largStandard.transform.Find("piv");
        standardTest = piv.GetComponent<StandardTest>();
        largRect = largStandard.GetComponent<RectTransform>();
        litRect = litStandard.GetComponent<RectTransform>();
        EventCenter.AddListener<float, float>(EventType.StandardRotation, GetStaRot);
        litStandard.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<float, float>(EventType.StandardRotation, GetStaRot);
    }
    private void Update()
    {
        transform.position = GameControl.intance.mMainBall.transform.position;
        if (standardDic != 0)
        {
            largeRot = new Vector3(90, standardDic + 90, 0);
            largRect.eulerAngles = largeRot;
            // RaycastHit hit = IsStand2();
            //largRect.sizeDelta = new Vector2(Mathf.Abs(longs * maxLong*300), 200);

            if (isCollWall)
            {
                //碰墙二段
                Vector3 litStandPos = piv.position;
                litStandard.transform.position = litStandPos;

                litRect.eulerAngles = Vector3.Reflect(largeRot, standardTest.collider1.GetContact(0).normal);

                //litRect.eulerAngles = Vector3.Reflect(largeRot, hit.normal);
                float dis = Mathf.Abs(Vector3.Distance(standardTest.collider1.GetContact(0).point, transform.position));

                float longs1 = dis / transLong;
                Debug.Log("dis" + dis + " longs" + longs);
                //要取得射线到墙的距离
                litRect.sizeDelta = new Vector2(Mathf.Abs(longs1 * maxLong), 200);
                largRect.sizeDelta = new Vector2(Mathf.Abs((longs - longs1) * maxLong), 200);
                litStandard.SetActive(true);
            }
            else
            {
                largRect.sizeDelta = new Vector2(Mathf.Abs(longs * maxLong), 200);
                litStandard.SetActive(false);
            }
        }
    }

    /// 一段检测(待定)
    private RaycastHit IsStand2()
    {
        Ray ray = new Ray(largStandard.transform.position, piv.transform.position);
        Debug.Log(largStandard.transform.position + "||" + piv.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, longs * transLong, 1 << 7))
        {
            isCollWall = true;
            Debug.Log("检测到墙");
        }
        else
        {
            isCollWall = false;
        }
        return hit;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, piv.transform.position * longs);
    }

    private void GetStaRot(float rot, float lon)
    {
        longs = lon;
        standardDic = rot;
    }
}
