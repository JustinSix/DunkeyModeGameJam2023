using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FullGuysManager : MonoBehaviour
{
    [SerializeField] TMP_Text modeText;
    [Header("Timer Stuff")]
    public TMP_Text timerText;
    public float maxTime = 60f; // Set your desired maximum time in seconds
    private float currentTime;
    private bool gameEnded = false;
    int activityPointsValue = 10000;
    private void Start()
    {
        SoundManager.Instance.SpawnSound(SoundManager.SoundName.GAMINGMODE);
        modeText.text = "GAMING MODE";
        RectTransform modeTextRectTransform = modeText.GetComponent<RectTransform>();
        modeTextRectTransform.DOScale(1, .5f);
        StartCoroutine(LerpAnchoredPosition(modeTextRectTransform, new Vector2(0, 435), .4f));
        currentTime = maxTime;
        timerText.text = FormatTime(currentTime);
    }
    void Update()
    {
        if (!gameEnded)
        {
            currentTime -= Time.deltaTime;
            timerText.text = FormatTime(currentTime);

            // Check if the player has won (you can replace this condition)
            if (currentTime <= 0.0f)
            {
                EndGame();
            }
        }
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

    void EndGame()
    {
        CalculateResults(false);
        StartCoroutine(DelayedLoadScene());
    }

    string FormatTime(float time)
    {
        // Format time as minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
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
        modeText.GetComponent <RectTransform>().localScale = Vector3.zero;
    }
    IEnumerator DelayedLoadScene()
    {
        SoundManager.Instance.SpawnSound(SoundManager.SoundName.LOSING_SOUND);
        yield return new WaitForSeconds(1.4f);
        Loader.Load(Loader.Scene.StreamerScene);
    }
}
