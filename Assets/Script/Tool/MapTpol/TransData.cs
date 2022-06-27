using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��json���ݵĴ��������
/// </summary>
public class TransData 
{
    public List<TranXY> wallTransArray = new List<TranXY>();
    public List<TranXY> floorTransArray = new List<TranXY>();
    public List<TranXY> otherTransArray = new List<TranXY>();
    public TranXY mainBall = new TranXY();
}
public struct TranXY
{
    public double x;
    public double z;
    public ItemName name;
    public TranXY(double x, double z,ItemName name)
    {
        this.x = x;
        this.z = z;
        this.name = name;
    }
}
public enum ItemName{
     wallIt,
     floorIt,
     turretIt,
     ballIt,
     moneyIt,
     swordIt
}