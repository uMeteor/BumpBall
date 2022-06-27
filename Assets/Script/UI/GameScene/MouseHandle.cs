using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHandle : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    private Standard standard;
    private GameObject rectControl;
    private GameObject rKey;
    private Vector3 beginDragPos;
    public Vector3 nowHandlePosNormal;//传递给球的方向
    private float handleLong;
    private float rectWidth;
    private float timeScaleValue=0.1f;
    public bool isControlHandle;
    private int isReverKey;//翻转操作杆 0.true, 1false;
    private int isShowHandle;//同上 

   public void init()
    {
        standard = GameControl.intance.standard;
        rectControl = GameObject.Find("Rect");
        rKey = rectControl.transform.Find("RKey").gameObject;
        rectWidth = rectControl.GetComponent<RectTransform>().sizeDelta.x;

        isReverKey = PlayerPrefs.GetInt(DataName.isReverseHandlep);
        isShowHandle = PlayerPrefs.GetInt(DataName.isShowRocker);
     
        standard.gameObject.SetActive(false);
        rectControl.SetActive(false);

    }
    //显示与隐藏摇杆
    private void IsShouHand(int s)
    {
        Image r = rKey.GetComponent<Image>(),
            k = rectControl.GetComponent<Image>();
        if (s == 0)//显示
        {
            r.enabled=true;
            k.enabled = true;
        }
        else//隐藏
        {
            r.enabled = false;
            k.enabled = false;
        }
    }  

    //摇杆处理
    public void OnBeginDrag(PointerEventData eventData)
    {      
        beginDragPos = Input.mousePosition;
        rectControl.transform.position = Input.mousePosition;
        rectControl.gameObject.SetActive(true);
        isControlHandle = true;
        Time.timeScale = timeScaleValue;
        IsShouHand(isShowHandle);
        standard.gameObject.SetActive(true);
    }
    public void OnDrag(PointerEventData eventData)
    {        
        Vector3 differ = Input.mousePosition - beginDragPos;        
        handleLong = differ.magnitude*0.6f;
        float angle = Mathf.Atan2(differ.x, differ.y) * Mathf.Rad2Deg + transform.eulerAngles.x;
        if (isReverKey == 1)
        {           
            nowHandlePosNormal = differ.normalized;
            rKey.transform.position = beginDragPos + nowHandlePosNormal * Mathf.Clamp(handleLong, 0, rectWidth);
        }
        else if (isReverKey == 0)//反方向
        {
            angle += 180;
            nowHandlePosNormal = -differ.normalized;
            rKey.transform.position = beginDragPos -nowHandlePosNormal * Mathf.Clamp(handleLong, 0, rectWidth);
        }             
        float longs = differ.magnitude/rectWidth<1 ? differ.magnitude / rectWidth : 1;
        EventCenter.Broadcast<float,float>(EventType.StandardRotation, angle,longs);//预计线的角度与长度      
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isControlHandle = false;
        rectControl.gameObject.SetActive(false);
        EventCenter.Broadcast(EventType.BallDir);
        Time.timeScale = 1;
        standard.gameObject.SetActive(false);
    }
}

