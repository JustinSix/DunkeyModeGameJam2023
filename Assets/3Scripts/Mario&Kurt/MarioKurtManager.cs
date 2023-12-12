using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarioKurtManager : MonoBehaviour
{
    [SerializeField] TMP_Text modeText;

    [SerializeField] private GameObject xqcObject;
    [SerializeField] private GameObject ethanObject;
    [SerializeField] private GameObject amouranthObject;
    [SerializeField] private GameObject destinyObject;
    [SerializeField] private int activityPointsValue;
    private string chosenStreamer;
    private void Start()
    {
        chosenStreamer = PlayerPrefs.GetString("ChosenStreamer", "XQC");
        switch (chosenStreamer)
        {
            case "Amouranth":
                amouranthObject.SetActive(true);
                break;
            case "EthanH3H3":
                ethanObject.SetActive(true);
                break;
            case "XQC":
                xqcObject.SetActive(true);
                break;
            case "Destiny":
                destinyObject.SetActive(true);  
                break;
            default:
                xqcObject.SetActive(true);
                break;
        }

        SoundManager.Instance.SpawnSound(SoundManager.SoundName.GAMINGMODE);
        modeText.text = "GAMING MODE";
        RectTransform modeTextRectTransform = modeText.GetComponent<RectTransform>();
        modeTextRectTransform.DOScale(1, .5f);
        StartCoroutine(LerpAnchoredPosition(modeTextRectTransform, new Vector2(0, 435), .4f));
    }

    public void CalculateResults(bool won)
    {
        if(won)
        {
            PlayerPrefs.SetInt("ActivityResult", 1);
        }
        else
        {
           PlayerPrefs.SetInt("ActivityResult", 0);
        }
        PlayerPrefs.SetInt("CompletedActivityPoints", activityPointsValue);
    }
    IEnumerator LerpAnchoredPosition(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        rectTransform.anchoredPosition = targetPos;

        Debug.Log("Lerping anchoredPosition complete!");

        yield return new WaitForSeconds(2f);

        modeText.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        modeText.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
