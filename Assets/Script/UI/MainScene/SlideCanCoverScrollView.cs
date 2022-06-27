using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 使用比例进行多滑
/// </summary>
public class SlideCanCoverScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private float contentLengrh;//容器长度
    private float beginMousePostionX;
    private float endMousePostionX;
    [HideInInspector]
    public ScrollRect scrollRect;
    private UIFacade mUIFacade;

    private float lastProportiong;//下一个位置点

    public int cellLength;//每个单元格长度
    public int spacing;//间隙
    public int leftOffest;//左偏于量
    private float upperLimit;//滑动上限值
    private float lowerLimit;//下限
    private float firstItemMoveLength;//移动第一个单元格需要的距离
    private float oneItemMoveLength;//移动一个单元格所需要的距离
    private float oneItemProportion;//滑动一个单元格距离所占的比例

    public int totalItemNum;//单元格数量
    [HideInInspector]
    public int currentItem;//当前单元格索引
    [HideInInspector]
    public bool isSiler;
    [HideInInspector]
    public int slideClass;//0 皮肤滑块 1轨迹滑块 2 关卡滑块 
    public bool isTransmit;
    private void Awake()
    {
        mUIFacade = GameManager.intance.mUIFacade;
        scrollRect = GetComponent<ScrollRect>();
        contentLengrh = scrollRect.content.rect.xMax - 2 * leftOffest - cellLength;
        firstItemMoveLength = cellLength / 2 + leftOffest;
        oneItemMoveLength = cellLength + spacing;
        oneItemProportion = oneItemMoveLength / contentLengrh;
        upperLimit = 1 - firstItemMoveLength / contentLengrh;
        lowerLimit = firstItemMoveLength / contentLengrh;
        currentItem = 1;
        scrollRect.horizontalNormalizedPosition = 0;

    }
    private void OnEnable()
    {
        if (slideClass == 0)
        {
            MoveAppiontItem(PlayerPrefs.GetInt(DataName.BallSkinIndex) + 1);
        }
        else if (slideClass == 1)
        {
            MoveAppiontItem(PlayerPrefs.GetInt(DataName.BallTrackIndex) + 1);
        }
        else if (slideClass == 2)
        {
            MoveAppiontItem(PlayerPrefs.GetInt(DataName.LeveLUnLockIndex) + 1);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePostionX = Input.mousePosition.x;
        isSiler = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        endMousePostionX = Input.mousePosition.x;

#if UNITY_STANDALONE_WIN
        float offSetX = (beginMousePostionX - endMousePostionX)*2;
#elif UNITY_ANDROID
        float offSetX = (beginMousePostionX - endMousePostionX);
#endif
        if (Mathf.Abs(offSetX) >= firstItemMoveLength)
        {
            if (offSetX > 0)//右滑
            {
                if (currentItem >= totalItemNum) return;
                int moveCount = (int)((offSetX - firstItemMoveLength) / oneItemMoveLength) + 1;
                currentItem += moveCount;
                if (currentItem > totalItemNum) { currentItem = totalItemNum; }
                lastProportiong += oneItemProportion * moveCount;
                if (lastProportiong >= upperLimit) { lastProportiong = 1; }
            }
            else
            {
                if (currentItem <= 1) return;
                int moveCount = (int)((offSetX + firstItemMoveLength) / oneItemMoveLength) - 1;
                currentItem += moveCount;
                if (currentItem <= 1) { currentItem = 1; }
                lastProportiong += oneItemProportion * moveCount;
                if (lastProportiong <= lowerLimit) { lastProportiong = 0; }
            }
        }
        if (isTransmit)
        {
            EventCenter.Broadcast<int>(EventType.MinLevelID, currentItem);
        }
        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
            moveDis => scrollRect.horizontalNormalizedPosition = moveDis,
            lastProportiong, 0.5f).SetEase(Ease.OutQuint);
    }
    //赋值移动
    public void MoveAppiontItem(int index)
    {
        lastProportiong += oneItemProportion * (index - currentItem);
        if (lastProportiong <= lowerLimit) { lastProportiong = 0; }
        else if (lastProportiong >= upperLimit) { lastProportiong = 1; }

        currentItem = index;
        if (currentItem <= 1) { currentItem = 1; }
        else if (currentItem > totalItemNum) { currentItem = totalItemNum; }

        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
         moveDis => scrollRect.horizontalNormalizedPosition = moveDis,
         lastProportiong, 0.5f).SetEase(Ease.OutQuint);
    }
}
