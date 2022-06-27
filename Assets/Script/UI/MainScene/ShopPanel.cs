using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;

public class ShopPanel : BasePanel
{
    //引用
    private SlideCanCoverScrollView slideViewSkin;
    private SlideCanCoverScrollView slideViewTrack;
    private Toggle tog_skin;
    private Button but_seletEquip;
    private Button but_bugEquip;
    private Text txt_PriceEquip;
    private GameObject txt_seletEquip, txt_seletingEquip;
    private GameObject contentSkin, contentTrack;
    public GameObject smallMap;
    private SmallBall smallBall;

    //数据
    private int currentEquipSkinIndex,currentEquipTrackIndex,tempTrack=-1;//当前选择球的索引；
    private string bugSkin, bugTrack,bugCurrent;
    private bool isSkin=true;
    private bool isNull;//用于Upata判空
 
    List<GameObject> equipListsSkin, equipListsTrack;
    List<int> priceSkinList = new List<int>() { 0, 5, 10, 15, 20 },//皮肤与轨迹的价格
              priceTrackLList = new List<int>() { 0, 4, 8, 12 };
    Vector3 scale = new Vector3(1, 1, 1);
    Vector3 recoverScale = new Vector3(0.5f, 0.5f, 1);
    Color colorChange;//用于改变透明度
    private int tIndex=-1;
    private void Awake()
    {
        slideViewSkin = transform.Find("Scroll_Skin").GetComponent<SlideCanCoverScrollView>();
        slideViewTrack = transform.Find("Scroll_Track").GetComponent<SlideCanCoverScrollView>();
        slideViewSkin.gameObject.SetActive(false);
        slideViewTrack.gameObject.SetActive(false);
    }

    public override void InitPanel()
    {
        base.InitPanel();
        bugSkin = PlayerPrefs.GetString(DataName.BugBallSkinIndex);
        bugTrack = PlayerPrefs.GetString(DataName.BugBallTrackIndex);
            
        GetSildeContent(slideViewSkin, ref equipListsSkin, ref contentSkin);
        GetSildeContent(slideViewTrack, ref equipListsTrack, ref contentTrack);        

        tog_skin = GameObject.Find("TwoToggle/tog_ball").GetComponent<Toggle>();
        tog_skin.onValueChanged.AddListener(ShowBallShinOrTrack);
        tog_skin.isOn = true;

        but_seletEquip = GameObject.Find("but_SeletEquip").GetComponent<Button>();
        but_bugEquip = GameObject.Find("but_BugEquip").GetComponent<Button>();
        but_seletEquip.onClick.AddListener(SeletCurrentEquip);
        but_bugEquip.onClick.AddListener(BugEquip);

        txt_PriceEquip = but_bugEquip.transform.Find("txt_PriceEquip").GetComponent<Text>();        
        txt_seletEquip =  but_seletEquip.transform.Find("txt_SeletEquip").gameObject;
        txt_seletingEquip= but_seletEquip.transform.Find("txt_SeletingEquip").gameObject;
       
        smallMap = GameObject.Find("SmallMap").gameObject;
        smallBall =smallMap.transform.Find("SmallBall").GetComponent<SmallBall>();
        isNull=true;
    }

    private void Update()
    {
        if (!isNull) return;
        if (slideViewSkin != null&&isSkin)
        {         
            ChangSildeEquipScale(slideViewSkin, equipListsSkin,ref currentEquipSkinIndex);
            smallBall.SetSkin(currentEquipSkinIndex);

        }
        if (slideViewTrack != null&&!isSkin)
        {
            ChangSildeEquipScale(slideViewTrack,equipListsTrack,ref currentEquipTrackIndex);
            if (currentEquipTrackIndex != tempTrack)
            {
                smallBall.SetTrack(currentEquipTrackIndex);
                tempTrack = currentEquipTrackIndex;
            }
           
        }
        ShowPrice();
        ShowBugEquipOrSelet();
        SetSeletButText();
    }

    //变化演示球的皮肤与轨迹
    private void SeletCurrentEquip()
    {
        if (isSkin)
        {
            PlayerPrefs.SetInt(DataName.BallSkinIndex, currentEquipSkinIndex);
        }
        else
        {
            PlayerPrefs.SetInt(DataName.BallTrackIndex, currentEquipTrackIndex);
        }   
        
    }
    private void ShowPrice()
    {
        if (!but_bugEquip.IsActive()) return;
        if (isSkin)
        {
            txt_PriceEquip.text = "-"+priceSkinList[currentEquipSkinIndex].ToString();
        }
        else txt_PriceEquip.text = "-" + priceTrackLList[currentEquipTrackIndex].ToString();

    }
    private void BugEquip()
    {
        if (isSkin)
        {
            StringBuilder s =new  StringBuilder(PlayerPrefs.GetString(DataName.BugBallSkinIndex));
            s[currentEquipSkinIndex] = '0';
            if (PlayerPrefs.GetInt(DataName.MoneyNum) > priceSkinList[currentEquipSkinIndex])
            {
                PlayerPrefs.SetInt(DataName.MoneyNum, PlayerPrefs.GetInt(DataName.MoneyNum) - priceSkinList[currentEquipSkinIndex]);
                mUIFacede.UpdatePowerAndMoney();
                bugSkin = s.ToString();
                PlayerPrefs.SetString(DataName.BugBallSkinIndex, bugSkin);
                ChangRecoverSAndC(currentEquipSkinIndex, equipListsSkin);
            }         
        }
        else
        {
            StringBuilder s = new StringBuilder(PlayerPrefs.GetString(DataName.BugBallTrackIndex));
            s[currentEquipTrackIndex] = '0';
            if (PlayerPrefs.GetInt(DataName.MoneyNum) > priceSkinList[currentEquipSkinIndex])
            {
                PlayerPrefs.SetInt(DataName.MoneyNum, PlayerPrefs.GetInt(DataName.MoneyNum) - priceTrackLList[currentEquipTrackIndex]);
                mUIFacede.UpdatePowerAndMoney();
                bugTrack = s.ToString();
                PlayerPrefs.SetString(DataName.BugBallTrackIndex, bugTrack);
                ChangRecoverSAndC(currentEquipTrackIndex, equipListsTrack);
            }        
        }
    }

    //切换购买和已购按钮
    private void ShowBugEquipOrSelet()
    {
        if (isSkin)
        {
            if (bugSkin[currentEquipSkinIndex] == '0')
            {
                but_bugEquip.gameObject.SetActive(false);
                but_seletEquip.gameObject.SetActive(true);
            }
            else
            {
                but_bugEquip.gameObject.SetActive(true);
                but_seletEquip.gameObject.SetActive(false);

            }
        }
        else
        {
            if (bugTrack[currentEquipTrackIndex] == '0')
            {
                but_bugEquip.gameObject.SetActive(false);
                but_seletEquip.gameObject.SetActive(true);
            }
            else
            {
                but_bugEquip.gameObject.SetActive(true);
                but_seletEquip.gameObject.SetActive(false);

            }

        }
    }
    //已购按钮的选择切换
    public void SetSeletButText()
    {
        if (!but_seletEquip.IsActive()) return;
        if (isSkin)
        {
            if (currentEquipSkinIndex != PlayerPrefs.GetInt(DataName.BallSkinIndex))
            {
               txt_seletEquip.SetActive(true);
                txt_seletingEquip. SetActive(false);
            }
            else
            {
                txt_seletEquip.SetActive(false);
                txt_seletingEquip.SetActive(true);
            }
        }
        else
        {
            if (currentEquipTrackIndex != PlayerPrefs.GetInt(DataName.BallTrackIndex))
            {
                txt_seletEquip.SetActive(true);
                txt_seletingEquip.SetActive(false);
            }
            else
            {
                txt_seletEquip.SetActive(false);
                txt_seletingEquip.SetActive(true);
            }
        }
       
    }

    //获得content里面的模块
    private void GetSildeContent(SlideCanCoverScrollView silde,ref List<GameObject> contentList,ref GameObject contentObject)
    {
        contentObject = silde.scrollRect.content.gameObject;
        contentList = new List<GameObject>();
        for (int i = 1; i <= silde.totalItemNum; i++)
        {
            GameObject temp = contentObject.transform.Find("SmellLevel/img_" + i.ToString()).gameObject;
            contentList.Add(temp);
        }
        ChangRecoverSAndC(0, contentList);
    }      
    //content大小收缩
    private void ChangSildeEquipScale(SlideCanCoverScrollView silde, List<GameObject> contentList,ref int currrentIndex)
    {
        float slideMove = silde.scrollRect.horizontalNormalizedPosition; 
        float monand = 1.0f / (silde.totalItemNum);  
        int index = Mathf.CeilToInt(slideMove / monand)-1;           
        if (tIndex != index)
        {         
            if (index < 0) index = 0;
            else if (index > silde.totalItemNum - 1) index = silde.totalItemNum - 1;         
            currrentIndex = index;
            ChangRecoverSAndC(index, contentList);
            tIndex = index;
        }
         
    }
    private void ChangRecoverSAndC(int index,List<GameObject> contentList)
    {   
        if (isSkin) bugCurrent = bugSkin;
        else bugCurrent = bugTrack;
        colorChange = contentList[index].GetComponent<Image>().color;
        if (bugCurrent[index] == '0')
        {
            contentList[index].transform.localScale = scale;
            colorChange.r = 1;
        }     
        for (int i = 0; i < contentList.Count; i++)
        {

            if (i != index)
            {
                contentList[i].transform.localScale = recoverScale;
                colorChange = contentList[i].GetComponent<Image>().color;
                colorChange.r = 0;
            }
        }
    }

    //两个滑块隐藏与显示
    private void ShowBallShinOrTrack(bool isOne)
    {
        isSkin = isOne;
        if (isOne)
        {
            slideViewSkin.gameObject.SetActive(true);
            slideViewTrack.gameObject.SetActive(false);
        }
        else
        {
            slideViewSkin.gameObject.SetActive(false);
            slideViewTrack.gameObject.SetActive(true);
        }
    }
    public void MovePanel()
    {
        transform.DOLocalMoveX(0,0.5f);
        slideViewSkin.gameObject.SetActive(true);
        slideViewTrack.gameObject.SetActive(false);
        smallMap.SetActive(true);
        tog_skin.isOn = true;
    }
    public void BackPanel()
    {
        transform.DOLocalMoveX(485, 0.5f);
        slideViewSkin.gameObject.SetActive(false);
        slideViewTrack.gameObject.SetActive(false);
        smallMap.SetActive(false);
    }
}
