using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StreamManager : MonoBehaviour
{
    public static StreamManager Instance { get; private set; }
    [Header("Visual Feedback")]
    [SerializeField] private Image chatImage;
    [SerializeField] private Sprite[] positiveChatMessages;
    [SerializeField] private Sprite[] negativeChatMessages;
    [SerializeField] private TMP_Text feedbackTextNumber;
    [SerializeField] private TMP_Text feedbackTextType;
    [SerializeField] private Color negativeFColor;
    [SerializeField] private Color positiveFColor;

    [Header("Essential UI")]
    [SerializeField] private GameObject streamingWebVCameraO;
    [SerializeField] private GameObject streamViewCanvas;

    [SerializeField] private TMP_Text followersText;
    [SerializeField] private TMP_Text viewersText;


    [Header("Streamer SO's")]
    [SerializeField] private Streamer amouranthSO;
    [SerializeField] private Streamer destinySO;
    [SerializeField] private Streamer xqcSO;
    [SerializeField] private Streamer ethanSO;
    [SerializeField] private Streamer hasanSO;
    [SerializeField] private Streamer pirateSO;

    public string completedActivity;
    public int activityEarnedPoints;

    private int currentFollowers = 10000;
    private int currentViewers = 1000;
    private int startingFollowersValue;
    private int startingViewersValue;
    private bool feedbackPositive = false;
    private void Awake()
    {
        Instance = this;
        currentViewers = PlayerPrefs.GetInt("CurrentViewers", 1000);
        currentFollowers = PlayerPrefs.GetInt("CurrentFollowers", 10000);

    }
    private void Start()
    {
        completedActivity = PlayerPrefs.GetString("CompletedActivity", "");
        activityEarnedPoints = PlayerPrefs.GetInt("CompletedActivityPoints", 1);
    }
    public void ShiftOffStreamView()
    {
        streamingWebVCameraO.SetActive(false);
        streamViewCanvas.SetActive(false);
    }

    public void ShiftToStreamView()
    {
        StartCoroutine(WaitForCinemachineTransition());

        if (ActivityManager.Instance.GetActivityResult())
        {
            PositiveReaction();
            //change it from binary 
            //then based on final score change if its positive or negative
            // calculate exact score in polsitive/negative reeaction
        }
        else
        {
            NegativeReaction();
        }

        streamingWebVCameraO.SetActive(true);
    }

    //pop in random image of chat have array of negative and positive chats, animate it in 

    private void PositiveReaction()
    {
        feedbackPositive = true;
        startingFollowersValue = currentFollowers;
        startingViewersValue = currentViewers;

        int rNum = UnityEngine.Random.Range(0, positiveChatMessages.Length);

        chatImage.sprite = positiveChatMessages[rNum];

        currentViewers += GetViewerChanges(true);

        currentFollowers += GetFollowerChanges(true);

        PlayerPrefs.SetInt("CurrentViewers", currentViewers);
        PlayerPrefs.SetInt("CurrentFollowers", currentFollowers);
    }
    private void NegativeReaction()
    {
        feedbackPositive = false;
        startingFollowersValue = currentFollowers;
        startingViewersValue = currentViewers;

        int rNum = UnityEngine.Random.Range(0, negativeChatMessages.Length);

        chatImage.sprite = negativeChatMessages[rNum];

        currentViewers -= GetViewerChanges(false);

        currentFollowers -= GetFollowerChanges(false);

        if (currentFollowers < 0)
        {
            currentFollowers = 0;
        }
        if (currentViewers < 0)
        {
            currentViewers = 0;
        }
        PlayerPrefs.SetInt("CurrentViewers", currentViewers);
        PlayerPrefs.SetInt("CurrentFollowers", currentFollowers);
    }
    private IEnumerator LerpText(int startValue, int endValue, TMP_Text textElement)
    {
        float elapsedTime = 0f;
        float lerpTime = 1.5f; // Adjust the time as needed

        while (elapsedTime < lerpTime)
        {
            textElement.text = Mathf.Lerp(startValue, endValue, elapsedTime / lerpTime).ToString("F0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final value is set
        textElement.text = endValue.ToString("F0");
    }
    private int GetFollowerChanges(bool isPositive)
    {
        int totalFollowers = 0;
        //determine streamer x activity multiplier
        //if activity is synergistic with streamer more points
        //if activity isnt then less points
        float multiplier = 1f;
        switch (GameManager.Instance.GetCurrentStreamer())
        {
            case "Amouranth":
                multiplier = GetMultiplierByStreamer(amouranthSO);
                break;
            case "H3H3":
                multiplier = GetMultiplierByStreamer(ethanSO);
                break;
            case "XQC":
                multiplier = GetMultiplierByStreamer(xqcSO);
                break;
            case "Destiny":
                multiplier = GetMultiplierByStreamer(destinySO);
                break;
            case "Hasan":
                multiplier = GetMultiplierByStreamer(hasanSO);
                break;
            case "PirateSoftware":
                multiplier = GetMultiplierByStreamer(pirateSO);
                break;
        }

        if (isPositive)
        {
            totalFollowers = Mathf.RoundToInt(activityEarnedPoints * multiplier);
        }
        else
        {
            totalFollowers = Mathf.RoundToInt(activityEarnedPoints / multiplier);
        }
        totalFollowers = Mathf.RoundToInt(activityEarnedPoints / UnityEngine.Random.Range(1.1f, 1.9f));

        Debug.Log("total followers after calculation: " + totalFollowers + "\nIs positive: " + isPositive);

        return totalFollowers;
    }

    private int GetViewerChanges(bool isPositive)
    {
        int totalViewers = 0;
        //determine streamer x activity multiplier
        //if activity is synergistic with streamer more points
        //if activity isnt then less points
        float multiplier = 1f;
        switch (GameManager.Instance.GetCurrentStreamer())
        {
            case "Amouranth":
                multiplier = GetMultiplierByStreamer(amouranthSO);
                break;
            case "H3H3":
                multiplier = GetMultiplierByStreamer(ethanSO);
                break;
            case "XQC":
                multiplier = GetMultiplierByStreamer(xqcSO);
                break;
            case "Destiny":
                multiplier = GetMultiplierByStreamer(destinySO);
                break;
            case "Hasan":
                multiplier = GetMultiplierByStreamer(hasanSO);
                break;
            case "PirateSoftware":
                multiplier = GetMultiplierByStreamer(pirateSO);
                break;
        }

        if (isPositive)
        {
            totalViewers = Mathf.RoundToInt(activityEarnedPoints * multiplier);
        }
        else
        {
            totalViewers = Mathf.RoundToInt(activityEarnedPoints / multiplier);
        }
        Debug.Log("total viewers after calculation: " + totalViewers + "\nIs positive: " + isPositive);
        return totalViewers + UnityEngine.Random.Range(0, 10);
    }
    private float GetMultiplierByStreamer(Streamer streamer)
    {
        float multiplier = 0f;
        switch (completedActivity)
        {
            case "MarioKurt":
                multiplier = streamer.MarioKurtMultiplier;
                break;
            case "FullGuys":
                multiplier = streamer.FullGuysMultiplier;
                break;
            case "PlayPiano":
                multiplier = streamer.PlayPianoMultiplier;
                break;
            case "HotTub":
                multiplier = streamer.HotTubMultiplier;
                break;
            //case "Eat":

            //    break;

            case "Dance":
                multiplier = streamer.DanceMultiplier;
                break;
            //case "PoliticalRant":

            //    break;

            case "Vape":
                multiplier = streamer.VapeMultiplier;
                break;

            case "Gamble":
                multiplier = streamer.GambleMultiplier;
                break;
            case "FactoryOH":
                multiplier = streamer.FactoryOhMultiplier;
                break;
            case "AnimalWell":
                multiplier = streamer.AnimalWellMultiplier;
                break;
            case "CallOfBooty":
                multiplier = streamer.CallOfBootyMultiplier;
                break;
            default:

                break;
        }
        return multiplier;
    }
    IEnumerator WaitForCinemachineTransition()
    {
        do
        {
            yield return null;
        } while (GameManager.Instance.cameraBrain.IsBlending);

        streamViewCanvas.SetActive(true);
        StartCoroutine(LerpText(startingViewersValue, currentViewers, viewersText));
        StartCoroutine(LerpText(startingFollowersValue, currentFollowers, followersText));

        if (feedbackPositive)
        {
            feedbackTextNumber.color = positiveFColor;
            feedbackTextType.color = positiveFColor;
            feedbackTextNumber.text = "+";
            SoundManager.Instance.SpawnSound(SoundManager.SoundName.VICTORY_SOUND);
        }
        else
        {
            feedbackTextNumber.color = negativeFColor;
            feedbackTextType.color = negativeFColor;
            feedbackTextNumber.text = "";
            SoundManager.Instance.SpawnSound(SoundManager.SoundName.LOSING_SOUND);
        }

        int followersGained = currentFollowers - startingFollowersValue;
        feedbackTextNumber.text += followersGained.ToString();
        feedbackTextNumber.GetComponent<RectTransform>().DOScale(1, .4f);

        feedbackTextType.text = "followers";
        feedbackTextType.GetComponent<RectTransform>().DOScale(1, .4f);

        yield return new WaitForSeconds(1.1f);
        feedbackTextNumber.GetComponent<RectTransform>().localScale = Vector3.zero;
        feedbackTextType.GetComponent<RectTransform>().localScale = Vector3.zero;

        if (feedbackPositive)
        {
            feedbackTextNumber.text = "+";
        }
        else
        {
            feedbackTextNumber.text = "";
        }
        int viewersGained = currentViewers - startingViewersValue;
        feedbackTextNumber.text += viewersGained.ToString();
        feedbackTextNumber.DOScale(1, .4f);

        feedbackTextType.text = "viewers";
        feedbackTextType.GetComponent<RectTransform>().DOScale(1, .4f);

        yield return new WaitForSeconds(1.1f);
        feedbackTextNumber.GetComponent<RectTransform>().localScale = Vector3.zero;
        feedbackTextType.GetComponent<RectTransform>().localScale = Vector3.zero;

        if (ActivityManager.Instance.GetIfActivitiesCompleted())
        {
            //end game
            Debug.Log("Ended Game");
            streamViewCanvas.SetActive(false);
            GameManager.Instance.EndGame();
        }
    }
    public int GetFollowers()
    {
        return currentFollowers;
    }
    public int GetViewers()
    {
        return currentViewers;
    }
}
