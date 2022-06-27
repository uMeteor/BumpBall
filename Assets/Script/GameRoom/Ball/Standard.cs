using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//球运动运动预计线
public class Standard : MonoBehaviour
{
    private GameObject largStandard, litStandard;
    private Transform largRect, litRect;
    private GameControl gameControl;
    private Vector3 piv;//Larg的顶点位置
    private StandardTest standardTest;

    private float standardDic;//角度值
    private float  longs;//比例长度
    private float maxLong=10;//最大长度
   // private float transLong = 3.76f;//世界坐标长度
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

    //寄
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
            //    //    //碰墙二段
            //    //    //   Vector3 litStandPos = piv.position;
            //    //    litStandard.transform.position = hit.point;
            //    //    litRect.eulerAngles = Vector3.Reflect(largeRot, hit.normal);

            //    //   // litRect.eulerAngles = Vector3.Reflect(largeRot, hit.normal);
            //    //    //  float dis = Mathf.Abs(Vector3.Distance(standardTest.collider1.GetContact(0).point, transform.position));

            //    //    //   float longs1 = dis / transLong;
            //    //    //   Debug.Log("dis" + dis+" longs"+longs);
            //    //    //要取得射线到墙的距离
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

    /// 一段检测(待定)
    private RaycastHit IsStand2( )
    {
        Ray ray = new Ray(largStandard.transform.position, largStandard.transform.right);
        RaycastHit hit;        
        Debug.DrawRay(largStandard.transform.position, largStandard.transform.position+ 
            (-largStandard.transform.right)* longs * maxLong/4);//绘制射线
        if (Physics.Raycast(ray, out hit, longs *maxLong, 1 << 7))
        {
            isCollWall = true;
       //     Debug.Log("检测到墙");
          
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
