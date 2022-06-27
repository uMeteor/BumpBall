using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteAlways]
public class MapCube : MonoBehaviour
{
    private MeshRenderer render;
    private MapCreat mMeveTesrt;
    private bool isLucency;
    private bool isHaveMode;  
    public MouseModes mode;
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        render.material.color = Color.green;
        mMeveTesrt = MapCreat.intance;
    }

    //����ƶ�����ǰ�������ʾ
    private void OnMouseEnter()
    {     
        render.material.color = Color.red;
        mMeveTesrt.nowMapCube = gameObject;
    }
    private void OnMouseExit()
    {
        if (isLucency)
        {
            render.material.color = Color.green;
        }
        else if (isHaveMode)
        {
            switch (mode)
            {
                case MouseModes.floor:
                    render.material.color = Color.blue;
                    break;
                case MouseModes.wall:
                    render.material.color = Color.black;
                    break;
                default:
                    break;
            }
        }      
        else render.material.color = Color.green;
        mMeveTesrt.nowMapCube = null;
    }

    //�л�ģʽ
    public void ChangeMode(MouseMode mode)
    {
        isLucency = false;
        isHaveMode = true;
        this.mode = (MouseModes)mode;
    }
    public void IsLucency()
    {
        isLucency = !isLucency;
        isHaveMode = false;
        mode = MouseModes.dafualt;
    }

}

public enum MouseModes
{
    dafualt,//Ĭ�ϲ������κζ���
    floor,
    wall
}