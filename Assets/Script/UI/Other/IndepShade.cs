using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//≥°æ∞«–ªª’⁄’÷
public class IndepShade : MonoBehaviour
{
    private Image Image;
    void Start()
    {
        Image = GetComponent<Image>();
        Image.DOColor(new Color(77/255f, 20/255f, 20/255f,1), 1f);
        StartCoroutine(DaleyDisplayer());
    }

    IEnumerator  DaleyDisplayer()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
    }
}
