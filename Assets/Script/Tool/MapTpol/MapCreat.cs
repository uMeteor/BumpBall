using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

[System.Serializable]
public class MapItem
{
    public ObjectItem name;
    public GameObject prafab;
}

//创建地图工具
public class MapCreat : MonoBehaviour
{
    public static MapCreat intance;
    public string xPx;
    public bool isCrectItem;//是否建立道具模式
    public GameObject wall;
    public GameObject floor;
    public GameObject mapCube;
    [HideInInspector]
    public GameObject nowMapCube;

    public List<MapItem> mapItim = new List<MapItem>();//存放Item预制体
    List<List<Vector3>> itemsTranList = new List<List<Vector3>>();
    List<GameObject> allItem = new List<GameObject>();//mapCube

    private MouseMode mode;//默认赋值枚举的第一个
    private ObjectItem Item;

    private int plateNun = 15;

    private void Awake()
    {
        intance = this;
    }
    void Start()
    {
        int ge = plateNun;
        mode = MouseMode.floor;
        Debug.Log("编辑器下的执行");
        for (int x = -ge; x < ge; x++)
        {
            for (int y = -ge; y < ge; y++)
            {
              allItem.Add(Instantiate(mapCube, new Vector3(x, -0.5f, y), Quaternion.identity));
            }
        }
    }
    void Update()
    {
        ChangeMode();
        if (isCrectItem)
        {
            if (Input.GetMouseButtonDown(0) && nowMapCube != null)
            {
                GetPreFab(Item, nowMapCube.transform);
            }
            else if (Input.GetMouseButtonDown(1) && nowMapCube != null)
            {
                DestoryPrafab();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && nowMapCube != null)
            {
                nowMapCube.GetComponent<MapCube>().ChangeMode(mode);
            }
            else if (Input.GetMouseButton(1) && nowMapCube != null)
            {
                nowMapCube.GetComponent<MapCube>().IsLucency();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            RereveData();
        }

    }
    
    //储存数据
    private void RereveData()
    {
        List<Vector3> wallList = new List<Vector3>();
        List<Vector3> floorList = new List<Vector3>();
        TransData data = new TransData();
        foreach (var item in allItem)
        {
            if ((MouseMode)item.GetComponent<MapCube>().mode == MouseMode.wall)
            {
                wallList.Add(item.transform.position);
            }
            else if((MouseMode)item.GetComponent<MapCube>().mode == MouseMode.floor)
            {
                floorList.Add(item.transform.position);
            }
           
            if (!(item.GetComponentsInChildren<Transform>(true).Length <=1))
            {
               
                Vector3 itemChhildVec = item.transform.GetChild(0).position;
                TranXY tranXY;
                switch (item.transform.GetChild(0).name.ToString())
                {
                    case "Ball"+ "(Clone)":
                      TranXY balXY = new TranXY((double)itemChhildVec.x,
                           (double)itemChhildVec.z, ItemName.ballIt);
                        data.mainBall = balXY;
                        break;
                    case "Money"+ "(Clone)":
                        tranXY = new TranXY((double)itemChhildVec.x,
                           (double)itemChhildVec.z, ItemName.moneyIt);
                        data.otherTransArray.Add(tranXY);
                    
                        break;
                    case "Sword"+ "(Clone)":
                        tranXY = new TranXY((double)itemChhildVec.x,
                           (double)itemChhildVec.z, ItemName.swordIt);
                        data.otherTransArray.Add(tranXY);
                       
                        break;
                    case "Turret"+ "(Clone)":
                        tranXY = new TranXY((double)itemChhildVec.x,
                           (double)itemChhildVec.z, ItemName.turretIt);
                        data.otherTransArray.Add(tranXY);
                      
                        break;
                    default:                      
                        break;
                }                            
            }          
        }
        Debug.Log(wallList.Count + "||" + floorList.Count);

        //itemsTranList.Add(wallList);
      //  itemsTranList.Add(floorList);       
        foreach (var item in wallList)
        {
           TranXY tranXY = new TranXY((double)item.x,(double)item.z,ItemName.wallIt);
            data.wallTransArray.Add(tranXY);
        }
        foreach (var item in floorList)
        {
            TranXY tranXY = new TranXY((double)item.x, (double)item.z,ItemName.floorIt);
            data.floorTransArray.Add(tranXY);
        }
        string filePath = Application.dataPath + "/StreamingAssets/JsonFile/MapData" + xPx+".json";
        string saveJsonStr = JsonMapper.ToJson(data);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
    //改变建造物体
    public void ChangeMode()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            isCrectItem = !isCrectItem;
        }
        if (!isCrectItem)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                mode = MouseMode.floor;
                Debug.Log("地板");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                mode = MouseMode.wall;
                Debug.Log("墙");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Item = ObjectItem.Ball;
                Debug.Log("球");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Item = ObjectItem.Turrent;
                Debug.Log("塔");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Item = ObjectItem.Sword;
                Debug.Log("转刀");
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Item = ObjectItem.Money;
                Debug.Log("钱");
            }
        }
      
    }
    //销毁地图上指定的物体
    private void DestoryPrafab()
    {
        if (nowMapCube.transform.GetChild(0) == null) return;
        GameObject gameObject = nowMapCube.transform.GetChild(0).gameObject;
        Destroy(gameObject);
    }
    //得到预制体
    private GameObject GetPreFab(ObjectItem name, Transform vector3)
    {
        foreach (var item in mapItim)
        {
            if (item.name == name)
            {
                GameObject ob = Instantiate(item.prafab, vector3);
                ob.transform.localPosition = new Vector3(0, 1, 0);

                return ob;
            }
        }
        Debug.Log("Name不存在");
        return null;
    }

    private void OnDrawGizmos()
    {       
        Gizmos.color = Color.yellow;
        //x
        for (int i = -plateNun; i < plateNun; i++)
        {
            Gizmos.DrawLine(new Vector3(-plateNun, 0, i - 0.5f), new Vector3(plateNun, 0, i - 0.5f));
        }
        //y
        for (int i = -plateNun; i < plateNun; i++)
        {
            Gizmos.DrawLine(new Vector3(i - 0.5f, 0, -plateNun), new Vector3(i - 0.5f, 0, plateNun));
        }
    }
}
public enum MouseMode
{
    dafualt,//默认不代表任何东西
    floor,
    wall
}
public enum ObjectItem
{
    Ball,
    Turrent,
    Sword,
    Money
}