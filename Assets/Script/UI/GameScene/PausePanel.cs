using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//游戏结束界面
public class PausePanel : MonoBehaviour
{
    private Button but_ReturnMainScene;
    private Button but_Again;
    private Button but_Next; 

    private Text txt_moneyNum;
    private GameObject moneyTip;
    private UIFacade mUIFacade;
    public void Start()
    {
        mUIFacade = GameManager.intance.mUIFacade;
        moneyTip = GameObject.Find("Money");
        but_Next = transform.Find("but_Next").GetComponent<Button>();
        but_Again = transform.Find("but_Again").GetComponent<Button>();
        but_ReturnMainScene = transform.Find("but_ToMIneScene").GetComponent<Button>();
        txt_moneyNum = transform.Find("Money/txt_MoneyNum").GetComponent<Text>();

        but_ReturnMainScene.onClick.AddListener(BackMainScene);
        but_Again.onClick.AddListener(AgainLevel);
        but_Next.onClick.AddListener(NextLevel);
        gameObject.SetActive(false);
    }
    //选择下一关
    private void NextLevel()
    {
        int s= PlayerPrefs.GetInt(DataName.LeveLSmallIndex);
        if (s < 4&&PlayerPrefs.GetInt(DataName.PowerNum)>0)
        {
            if(s > PlayerPrefs.GetInt(DataName.LeveLUnLockIndex))
            {
                PlayerPrefs.SetInt(DataName.LeveLUnLockIndex, s);
            }
            PlayerPrefs.SetInt(DataName.LeveLSmallIndex, s + 1);
            PlayerPrefs.SetString(DataName.LeveSeletString,
                PlayerPrefs.GetInt(DataName.LevelBigIndex).ToString() + "." 
                + PlayerPrefs.GetInt(DataName.LeveLSmallIndex).ToString());

               PlayerPrefs.SetInt(DataName.PowerNum, PlayerPrefs.GetInt(DataName.PowerNum) - 1);
            
         
            AgainLevel();
        }
        else
        {
            BackMainScene();
        }
    }
    //重来
    private void AgainLevel()
    {
        mUIFacade.mObjectPool.PutObjectInPool(ObjectName.Ball, GameControl.intance.mMainBall.gameObject);
        SceneManager.LoadScene(2);
        mUIFacade.AsyncLoadScene(2);
        GameControl.intance.isOver = false;
    }
    //回主场景
    private void BackMainScene()
    {
        GameControl.intance.isOver = false;
        Time.timeScale = 1;
        mUIFacade.ExitScene();
  
    }
    //显示结束界面
    public void ShowGamePanel(GameShowPauseMode mode)
    {
        switch (mode)
        {
            case GameShowPauseMode.Loser:
                but_Next.gameObject.SetActive(false);
                but_Again.gameObject.SetActive(true);
                moneyTip.SetActive(false);
                EventCenter.Broadcast(EventType.ToMoneyTrailPool);//回收钱
                GameControl.intance.isOver = true;
                break;
            case GameShowPauseMode.Win:
                but_Next.gameObject.SetActive(true);
                but_Again.gameObject.SetActive(false);
                txt_moneyNum.text = "+"+GameControl.intance.DisPlayCurrMoney().ToString();//显示获得金额
                moneyTip.SetActive(true);
                EventCenter.Broadcast(EventType.ToMoneyTrailPool);
                GameControl.intance.isOver = true;
                break;
            case GameShowPauseMode.Pause:
                but_Next.gameObject.SetActive(false);
                but_Again.gameObject.SetActive(true);
                moneyTip.SetActive(false);
                break;
            default:
                break;
        }
        gameObject.SetActive(true);
    }
}
public  enum GameShowPauseMode
{
    Loser,
    Win,
    Pause
}
