using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectName
{
    Ball,
    Bullet,
    MoneyTrail,
    Wall,
    Floor,
    Turret,
    Sword,
    Explosion
}

[System.Serializable]//需要实例化才面板才有序列化
public class ObjectType
{
    public ObjectName Name;
    public GameObject PreFab;
}
public class ObjectPool : MonoBehaviour
{
    public List<ObjectType> prefabList = new List<ObjectType>();//存储预制体
    private Dictionary<ObjectName, List<GameObject>> objectDic = 
        new Dictionary<ObjectName, List<GameObject>>();

    //取得对应的预制体
    private GameObject GetPreFab(ObjectName name)
    {
        foreach (var item in prefabList)
        {
            if (item.Name == name)
            {
                GameObject ob= Instantiate(item.PreFab, transform);
                ob.SetActive(false);
                return ob;
            }
        }
        Debug.Log("Name不存在");
        return null;
    }

    public GameObject GetObjectInPool(ObjectName name)
    {
        List<GameObject> oList;
        if (!objectDic.ContainsKey(name))
        {
            objectDic.Add(name, new List<GameObject>());
        }    
        oList = objectDic[name];
        if (oList.Count <= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                oList.Add(GetPreFab(name));
            }
        }
        int index = oList.Count - 1;
        GameObject g = oList[index];
        oList.RemoveAt(index);
        return g;
    }
    public void PutObjectInPool(ObjectName name,GameObject gameObject)
    {
        if (objectDic.ContainsKey(name))
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = transform;
            if(!objectDic[name].Contains(gameObject)) objectDic[name].Add(gameObject);

        }
        else Debug.Log(name + "不存在");
    }
}
