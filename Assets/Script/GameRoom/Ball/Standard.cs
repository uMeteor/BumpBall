using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���˶��˶�Ԥ����
public class Standard : MonoBehaviour
{
    private GameObject largStandard, litStandard;
    private Transform largRect, litRect;
    private GameControl gameControl;
    private Vector3 piv;//Larg�Ķ���λ��
    private StandardTest standardTest;

    private float standardDic;//�Ƕ�ֵ
    private float  longs;//��������
    private float maxLong=10;//��󳤶�
   // private float transLong = 3.76f;//�������곤��
    private Vector3 largeRot;
    public bool isCollWall;

    private void Awake()
    {
        gameControl = GameControl.intance;
        largStandard = GameObject.Find("Sta1");
        litStandard = GameObject.Find("Sta2");
        largRect = largStandard.GetComponent<Transform>();
        litRect = litStandard.GetComponent<Transform>();
        EventCenter.AddListener<float,float>(EventType.StandardRotation, GetStaRot);
        litStandard.SetActive(false);   
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<float, float>(EventType.StandardRotation, GetStaRot);
    }

    //��
    private void Update()
    {
        transform.position= GameControl.intance.mMainBall.transform.position;
        if (standardDic !=0)
        {
            largeRot= new Vector3(90, standardDic + 90,0);
            largRect.eulerAngles = largeRot;
            // RaycastHit hit = IsStand2();
            largRect.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(longs * maxLong), 2);
            //if (isCollWall)
            //{
            //    //    //��ǽ����
            //    //    //   Vector3 litStandPos = piv.position;
            //    //    litStandard.transform.position = hit.point;
            //    //    litRect.eulerAngles = Vector3.Reflect(largeRot, hit.normal);

            //    //   // litRect.eulerAngles = Vector3.Reflect(largeRot, hit.normal);
            //    //    //  float dis = Mathf.Abs(Vector3.Distance(standardTest.collider1.GetContact(0).point, transform.position));

            //    //    //   float longs1 = dis / transLong;
            //    //    //   Debug.Log("dis" + dis+" longs"+longs);
            //    //    //Ҫȡ�����ߵ�ǽ�ľ���
            //    //    float dis = Mathf.Abs(Vector3.Distance(transform.position, hit.point));
            //    //    largRect.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs((dis*4)), 2);
            //    //    litRect.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(maxLong- dis * 4), 2);

            //    //    litStandard.SetActive(true);
            //  //  largRect.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(Vector3.Distance(hit.transform.position ,largStandard.transform.position))*4, 2);
            //}
            //else
            //{
            //    largRect.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(longs * maxLong), 2);
            //    litStandard.SetActive(false);
            //}
        }
    }   

    /// һ�μ��(����)
    private RaycastHit IsStand2( )
    {
        Ray ray = new Ray(largStandard.transform.position, largStandard.transform.right);
        RaycastHit hit;        
        Debug.DrawRay(largStandard.transform.position, largStandard.transform.position+ 
            (-largStandard.transform.right)* longs * maxLong/4);//��������
        if (Physics.Raycast(ray, out hit, longs *maxLong, 1 << 7))
        {
            isCollWall = true;
       //     Debug.Log("��⵽ǽ");
          
        }
        else
        {
            isCollWall = false;
        }
        return hit;

    }
  
    private void GetStaRot(float rot,float lon)
    {
        longs = lon;
        standardDic = rot;
    }
}
