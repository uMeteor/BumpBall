using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 主场景UI
/// </summary>
public class MainUI : BaseSceneUI
{
    //数据
    [HideInInspector]
    public int minID;
    private int bigID;
         
    //不变Ui
    private Button but_Level;
    private Button but_Shop;
    private Button but_Set;
    private Button but_addEnergy,but_closeEnergy;
    private Text txt_Money;
    private Text txt_Power;
    private GameObject img_Selet;
    private GameObject tipPanel, tipSmall;

    //引用
    [HideInInspector]
    public  LevelPanel mLevelPanel;
    private ShopPanel mShopPanel;
    private SetPanel mSetPanel;

    private void Awake()
    {
        EventCenter.AddListener<int>(EventType.MinLevelID, (int tenp) => { minID = tenp; });
        EventCenter.AddListener<int>(EventType.BigLevelID, (int tenp) => { bigID = tenp; });
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventType.MinLevelID, (int tenp) => { minID = tenp; });
        EventCenter.RemoveListener<int>(EventType.BigLevelID, (int tenp) => { bigID = tenp; });
    }

    //放这里加载显示慢
    public override void Init()
    {     
        base.Init();
        mLevelPanel = GameObject.Find("LevelPanel").GetComponent<LevelPanel>();
        mShopPanel = GameObject.Find("ShopPanel").GetComponent<ShopPanel>();
        mSetPanel = GameObject.Find("SetPanel").GetComponent<SetPanel>();
        mLevelPanel.InitPanel();
        mShopPanel.InitPanel();
        mSetPanel.InitPanel();
        minID = PlayerPrefs.GetInt(DataName.LeveLUnLockIndex)+1;//索引变关卡值要加一
        bigID = PlayerPrefs.GetInt(DataName.LevelBigIndex);
        FixedUIAssign();
        UpdatePowerAndMoney();
        mUIFacade.mVoiceManeger.PlayBGMusic();
    }

    //共用Ui赋值
    private void FixedUIAssign()
    {
        tipPanel = transform.Find("TipPanel").gameObject;
        tipSmall = transform.Find("TipPanel/TipSmall").gameObject;
        but_addEnergy = tipPanel.transform.Find("Button").GetComponent<Button>();
        but_closeEnergy = tipPanel.transform.Find("Button1").GetComponent<Button>();
        but_Level = transform.Find("ComUI/Menu/but_Level").GetComponent<Button>();
        but_Shop = transform.Find("ComUI/Menu/but_Shop").GetComponent<Button>();
        but_Set = transform.Find("ComUI/Menu/but_Set").GetComponent<Button>();
        txt_Money = transform.Find("ComUI/Money/txt_MoneyNum").GetComponent<Text>();
        txt_Power = transform.Find("ComUI/Power/txt_PowerNum").GetComponent<Text>();
        img_Selet = GameObject.Find("ComUI/Menu/img_Selet");

        tipPanel.SetActive(false);
        tipSmall.SetActive(false);
        but_Level.onClick.AddListener(() => SeletButten(but_Level));
        but_Shop.onClick.AddListener(() => SeletButten(but_Shop));
        but_Set.onClick.AddListener(() => SeletButten(but_Set));
        but_addEnergy.onClick.AddListener(() => { TipPanelButten(true); });
        but_closeEnergy.onClick.AddListener(() => { TipPanelButten(false); });
    }

    /// 更新能量金钱数据
    public void UpdatePowerAndMoney()
    {
        txt_Money.text = PlayerPrefs.GetInt(DataName.MoneyNum).ToString();
        txt_Power.text = PlayerPrefs.GetInt(DataName.MaxPowerNum).ToString() + "/" + PlayerPrefs.GetInt(DataName.PowerNum).ToString();
    }
    //返回关卡名称，用于加载
    public string GetLevelID()
    {
        PlayerPrefs.SetInt(DataName.LeveLSmallIndex, minID);
      return bigID.ToString() + "." + minID.ToString();
    }

    //选择界面时的按钮滑块移动
    private void SeletButten(Button button)
    {
        Tween tween = img_Selet.transform.DOMoveX(button.transform.position.x, 0.2f);
        tween.SetEase(Ease.Linear);
        tween.OnComplete(() =>
        {
            InitButton();
            button.GetComponent<Image>().color = new Vector4(76.0f / 255, 214.0f / 255, 1, 1);
            button.GetComponentInChildren<Text>().color = new Vector4(76.0f / 255, 214.0f / 255, 1, 1);
        });
        if (button==but_Level)
        {
            mShopPanel.BackPanel();
            mSetPanel.BackPanel();
            mUIFacade.IsHideLevelPanel(false);
        }
        else if (button == but_Shop)
        {           
            mShopPanel.MovePanel();  
            mSetPanel.BackPanel();    
            mUIFacade.IsHideLevelPanel(false);

        }
        else if (button == but_Set)
        {
            mSetPanel.MovePanel();
            mShopPanel.BackPanel();
            mUIFacade.IsHideLevelPanel(true);
        }

    }
    //面板选择按钮初始化
    private void InitButton()
    {
        Vector4 iColor=new Vector4(67.0f / 255, 67.0f / 255, 67.0f / 255, 1);
        Vector4 tColor = new Vector4(71.0f / 255, 71.0f / 255, 71.0f / 255, 1);
        but_Set.GetComponent<Image>().color = iColor;
        but_Shop.GetComponent<Image>().color = iColor;
        but_Level.GetComponent<Image>().color = iColor;

        but_Set.GetComponentInChildren<Text>().color =tColor;
        but_Shop.GetComponentInChildren<Text>().color = tColor;
        but_Level.GetComponentInChildren<Text>().color = tColor;
    }
 
    //体力消耗完时的提醒
    public void ShowTipPanel()
    { 
        tipPanel.SetActive(true);
    }
    //钱换体力
    private void TipPanelButten(bool isAdd)
    {
        if (isAdd)
        {
            int s = PlayerPrefs.GetInt(DataName.MoneyNum);
            if (PlayerPrefs.GetInt(DataName.PowerNum) >= PlayerPrefs.GetInt(DataName.MaxPowerNum)) { return; }
            if (s >= 3)
            {
                PlayerPrefs.SetInt(DataName.MoneyNum, s-3);
                PlayerPrefs.SetInt(DataName.PowerNum, PlayerPrefs.GetInt(DataName.PowerNum) + 1);               
                UpdatePowerAndMoney();
            }
            else
            {
                tipPanel.SetActive(true);
                StartCoroutine(DaleyTipSmall());
            }
        }
        else tipPanel.SetActive(false);

    }        
    IEnumerator  DaleyTipSmall()
    {
       yield return new WaitForSeconds(2);
       tipPanel.SetActive(false);
    }

}
