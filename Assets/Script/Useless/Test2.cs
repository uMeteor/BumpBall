using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Test2: MonoBehaviour
{

    Hashtable hashtable = new Hashtable();
    public delegate void Deleg(int a);

    public void Da1(int a) { Debug.Log(1); }
    public void Da2(int a) { Debug.Log(2); }
    public void Da3(int a) { Debug.Log(3); }

    private void Start()
    {
        Deleg de = new Deleg(Da1);
       // de += Da2;
        de += Da3;
       // de(1);
        int[] array = new int[] { };
        Array.Sort(array);
        int a;
        int b=1;
        For1(out a);
        For2(ref b);

    }
    public void For()
    {
        hashtable.Add(1, 1);
        hashtable.Remove(1);
    }
    public void For1(out int a)
    {
        a = 0;
    }
    public void For2(ref int b)
    {

    }
}

