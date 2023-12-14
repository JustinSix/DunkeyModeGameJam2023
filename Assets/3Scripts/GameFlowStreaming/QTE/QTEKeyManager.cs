using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class QTEKeyManager : MonoBehaviour
{
    public TMP_Text qteKeyText;
    public Image fillTimerImage;
    public Image outerKeyImage;
    public GameObject keyObject;
    [SerializeField] Sprite defaultOuterKeySprite;
    public void FadeInKey()
    {
        outerKeyImage.sprite = defaultOuterKeySprite;
        fillTimerImage.DOKill();
        qteKeyText.DOKill();
        outerKeyImage.DOKill();

        fillTimerImage.DOFade(255f, .3f);
        qteKeyText.DOFade(255f, .3f);
        outerKeyImage.DOFade(255f, .3f);
        
    }
    public void FadeKeyOut()
    {
        fillTimerImage.fillAmount = 0;

        fillTimerImage.DOKill();
        qteKeyText.DOKill();
        outerKeyImage.DOKill();

        fillTimerImage.DOFade(0f, .3f);
        qteKeyText.DOFade(0f, .3f);
        outerKeyImage.DOFade(0f, .3f);

        StartCoroutine(DelayedResetKeyAndDisable());

    }
    IEnumerator DelayedResetKeyAndDisable()
    {
        yield return new WaitForSeconds(.3f);
        outerKeyImage.sprite = defaultOuterKeySprite;
        keyObject.SetActive(false);
    }
}
