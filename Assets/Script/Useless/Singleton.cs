using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    static Singleton MySingleton;
   public static Singleton Instance()
    {
        return MySingleton;
    }
    void Awake()
    {
        MySingleton = this;
    }
}
