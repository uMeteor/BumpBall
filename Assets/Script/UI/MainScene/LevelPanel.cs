using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelPanel : BasePanel
{
    private Button toRight;
    private Button toLeft;
    private Button enterGame;
    private GameObject unlock;//未解锁图片
    public SlideCanCoverScrollView slide;

    public int bigLevel=1;
    private int dataItem=-1;//解锁关卡索引

    private void Start()
    {
        slide = GameObject.Find("Scroll_View").GetComponent<SlideCanCoverScrollView>();
        toRight = GameObject.Find("but_ToRight").GetComponent<Button>();
        toLeft = GameObject.Find("but_ToLeft").GetComponent<Button>();
        enterGame = GameObject.Find("but_EnterGame").GetComponent<Button>();
        unlock = GameObject.Find("img_UnlockLevel");
        unlock.SetActive(false);
        dataItem = PlayerPrefs.GetInt(DataName.LeveLUnLockIndex);

        enterGame.onClick.AddListener(EnterButten);
        toRight.onClick.AddListener(ToRightButten);
        toLeft.onClick.AddListener(ToLeftButten);
        toLeft.gameObject.SetActive(false);
        toRight.gameObject.SetActive(false);
    }
    public override void InitPanel()
    {
        base.InitPanel();
    }
    private void Update()
    {
        if (dataItem == -1) return;
        IsDisplayeEnterBut();
    }
    //未解锁关卡的进入按钮显示
    private void IsDisplayeEnterBut() 
    {
        if (dataItem < slide.currentItem-1)
        {
            unlock.SetActive(true);
            enterGame.gameObject.SetActive(false);
        }
        else
        {
            unlock.SetActive(false);
            enterGame.gameObject.SetActive(true) ;
        }
    
    }
    private void EnterButten()
    {
        //减体力
        if (PlayerPrefs.GetInt(DataName.PowerNum) < 1) { mUIFacede.ShowTipPanel(); return; }
        else if(PlayerPrefs.GetInt(DataName.PowerNum)>=1)
        {
            PlayerPrefs.SetInt(DataName.PowerNum, PlayerPrefs.GetInt(DataName.PowerNum) - 1);
            //选择关卡名称
            PlayerPrefs.SetString(DataName.LeveSeletString, mUIFacede.GetLevelID());//存贮选择关卡
            mUIFacede.ExitScene();
        }
        if (PlayerPrefs.GetInt(DataName.PowerNum) < 0) PlayerPrefs.SetInt(DataName.PowerNum, 0);
    }

    //是否隐藏当前LevelPanel
    public void IsHideLevelPanel(bool on_off)
    {
        if (on_off == true) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    //大关切换（待定）
    private void ToRightButten()
    {
        if (bigLevel >= 3) bigLevel = 3;
        else bigLevel++;
        SwtichBg();
    }
    private void ToLeftButten()
    {
        if (bigLevel <= 1) bigLevel = 1;
        else bigLevel--;
        SwtichBg();
    }
    private void SwtichBg()
    {
        EventCenter.Broadcast<int>(EventType.BigLevelID, bigLevel);
    }
        

}
