using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Strandard : MonoBehaviour
{
    private GameObject largStandard, litStandard;
    private RectTransform largRect, litRect;
  //private GameControl gameControl;
    private Transform piv;//Larg�Ķ���λ��
    private StandardTest standardTest;

    private float standardDic;//�Ƕ�ֵ
    private float longs;//��������
    private float maxLong = 1500;
    private float transLong = 3.76f;//�������곤��
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
                //��ǽ����
                Vector3 litStandPos = piv.position;
                litStandard.transform.position = litStandPos;

                litRect.eulerAngles = Vector3.Reflect(largeRot, standardTest.collider1.GetContact(0).normal);

                //litRect.eulerAngles = Vector3.Reflect(largeRot, hit.normal);
                float dis = Mathf.Abs(Vector3.Distance(standardTest.collider1.GetContact(0).point, transform.position));

                float longs1 = dis / transLong;
                Debug.Log("dis" + dis + " longs" + longs);
                //Ҫȡ�����ߵ�ǽ�ľ���
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

    /// һ�μ��(����)
    private RaycastHit IsStand2()
    {
        Ray ray = new Ray(largStandard.transform.position, piv.transform.position);
        Debug.Log(largStandard.transform.position + "||" + piv.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, longs * transLong, 1 << 7))
        {
            isCollWall = true;
            Debug.Log("��⵽ǽ");
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
