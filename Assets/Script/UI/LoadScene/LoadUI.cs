using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
/// <summary>
/// 开始界面ui
/// </summary>
public class LoadUI : BaseSceneUI
{
    private Image img_StartCover;
    private Text txt_Piont;
    private Slider slider;
    private Button but_GO;//任意开始按钮

    private AsyncOperation AsyncOp = null;
    private float progress;

    public override void Init ()
    {
        base.Init();
        mUIFacade = GameManager.intance.mUIFacade;
        mUIManager = mUIFacade.mUIManager;
        mUIManager.mloadUI = this;
        img_StartCover = GameObject.Find("img_StartCover").GetComponent<Image>();
        txt_Piont = GameObject.Find("img_StartCover/txt/txt_Piont").GetComponent<Text>();
        slider = GameObject.Find("img_StartCover/Slider").GetComponent<Slider>();
        but_GO= GameObject.Find("but_GO").GetComponent<Button>();
        but_GO.onClick.AddListener(EnterMainScene);
   
        AsyncOp = SceneManager.LoadSceneAsync(1);//获得加载下一个场景的状态
        AsyncOp.allowSceneActivation = false;
        TxtAnim();
    }
    private void Update()
    {
        //进度条显示
        if (AsyncOp == null) return;
        else progress = AsyncOp.progress;      
        if (progress >= 0.899) progress = 1;
        if (slider.value > 0.99f) slider.value = 1;
        slider.value = Mathf.Lerp(slider.value, progress, Time.deltaTime * 10f);
        if (slider.value == 1)
        {
            img_StartCover.transform.position = new Vector3(10000, 100, 100);
        }
    }   
    public void EnterMainScene()
    {
        //这里为allowSceneActivation = true;时不会立刻跳转，需要将最后的10%加载完毕后才会跳转
        AsyncOp.allowSceneActivation = true;
        mUIFacade.EnterMainScene();
    }
    //文字点动画
    private void TxtAnim()
    {
        Tween txt = txt_Piont.DOText("......", 3);
        txt.SetAutoKill(false);
        txt.SetEase(Ease.Linear);
        txt.SetLoops(-1);
    }
 
}
