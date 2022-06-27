using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
/// <summary>
/// ��ʼ����ui
/// </summary>
public class LoadUI : BaseSceneUI
{
    private Image img_StartCover;
    private Text txt_Piont;
    private Slider slider;
    private Button but_GO;//���⿪ʼ��ť

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
   
        AsyncOp = SceneManager.LoadSceneAsync(1);//��ü�����һ��������״̬
        AsyncOp.allowSceneActivation = false;
        TxtAnim();
    }
    private void Update()
    {
        //��������ʾ
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
        //����ΪallowSceneActivation = true;ʱ����������ת����Ҫ������10%������Ϻ�Ż���ת
        AsyncOp.allowSceneActivation = true;
        mUIFacade.EnterMainScene();
    }
    //���ֵ㶯��
    private void TxtAnim()
    {
        Tween txt = txt_Piont.DOText("......", 3);
        txt.SetAutoKill(false);
        txt.SetEase(Ease.Linear);
        txt.SetLoops(-1);
    }
 
}
