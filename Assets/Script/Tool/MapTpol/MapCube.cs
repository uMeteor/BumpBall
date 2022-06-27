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

    //鼠标移动到当前方块的显示
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

    //切换模式
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
    dafualt,//默认不代表任何东西
    floor,
    wall
}