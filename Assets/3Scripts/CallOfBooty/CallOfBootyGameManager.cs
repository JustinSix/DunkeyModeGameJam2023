using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CallOfBootyGameManager : MonoBehaviour
{
    public static CallOfBootyGameManager Instance { get; private set; }
    [SerializeField] GameObject youLoseCanvasO;
    [SerializeField] TMP_Text modeText;
    int activityPointsValue = 10000;
    private bool gameEnded = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.SpawnSound(SoundManager.SoundName.GAMINGMODE);
        modeText.text = "GAMING MODE";
        RectTransform modeTextRectTransform = modeText.GetComponent<RectTransform>();
        modeTextRectTransform.DOScale(1, .5f);
        StartCoroutine(LerpAnchoredPosition(modeTextRectTransform, new Vector2(0, 435), .4f));
    }

    public void CalculateResults(bool won)
    {
        if (!gameEnded)
        {
            gameEnded = true;
            if (won)
            {
                PlayerPrefs.SetInt("ActivityResult", 1);

            }
            else
            {
                PlayerPrefs.SetInt("ActivityResult", 0);
            }
            PlayerPrefs.SetInt("CompletedActivityPoints", activityPointsValue);
        }
    }

    public void LoseGame()
    {
        SoundManager.Instance.SpawnSound(SoundManager.SoundName.LOSING_SOUND);
        CalculateResults(false);
        StartCoroutine(DelayedLoadScene());
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
    IEnumerator DelayedLoadScene()
    {
        youLoseCanvasO.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        Loader.Load(Loader.Scene.StreamerScene);
    }
}
