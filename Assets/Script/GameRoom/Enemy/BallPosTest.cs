using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������λ�õļ��
/// </summary>
public class BallPosTest : MonoBehaviour
{

   // private bool isFirstBallInRange;
    private bool isBallInRange;
    public bool IsBallInRange { get { return isBallInRange; } }
    private Vector3 firstToBallPos;
    private Vector3 toBallPos;
    public Vector3 ToBallPos { get { return toBallPos; } }
    
    private bool TestWall(Collider other)
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = other.transform.position;
        pos1.y = 0.5f;
        pos2.y = 0.5f;
        if (Physics.Raycast(transform.position, other.transform.position-transform.position,
            10, 1<<7)) return false;
        else return true;
    }
    private void OnEnable()
    {
        isBallInRange = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
       //  if(!TestWall(other)) return ;
            firstToBallPos = other.transform.position;
        //    isFirstBallInRange = true;   
            isBallInRange = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        { //�м���ǽ
       //     if (!TestWall(other)) return;
             toBallPos = other.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBallInRange = false;     
        }
    } 
}
