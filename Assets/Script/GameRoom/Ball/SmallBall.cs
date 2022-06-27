using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///展示窗口的球
/// </summary>
public class SmallBall : MonoBehaviour
{
    private int startSpeed = 15;  
    protected Vector3 ballRightDir = Vector3.zero;
    public UIFacade mUIFacade;
    protected Rigidbody rig;
    protected MeshRenderer meshRenderer;
    public Material[] ballMatereial;
    public  List<GameObject> trailList;

    private void Start()
    {

        ballRightDir = new Vector3(3f, 0, 6f).normalized;
        rig = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailList.Add(transform.Find("Trail0").gameObject);
        trailList.Add(transform.Find("Trail1").gameObject);
        trailList.Add(transform.Find("Trail2").gameObject);
        trailList.Add(transform.Find("Trail3").gameObject);
        SetTrack(PlayerPrefs.GetInt(DataName.BallTrackIndex));
    } 
    private void FixedUpdate()
    {
        if (ballRightDir != null)
        {
            rig.velocity = ballRightDir * startSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ballRightDir = Vector3.Reflect(ballRightDir, collision.contacts[0].normal);        
        }
    }
    public void SetSkin(int index)
    {
        meshRenderer.material = ballMatereial[index];
    }
    public void SetTrack(int index)
    {       
        for (int i = 0; i < 4; i++)
        {
            trailList[i].SetActive(false);
        }
        trailList[index].SetActive(true); 
    }
}
