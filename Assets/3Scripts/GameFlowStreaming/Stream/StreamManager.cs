using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StreamManager : MonoBehaviour
{
    public static StreamManager Instance { get; private set; }

    [SerializeField] private Image chatImage;
    [SerializeField] private Sprite[] positiveChatMessages;
    [SerializeField] private Sprite[] negativeChatMessages;

    [SerializeField] private GameObject streamingWebVCameraO;
    [SerializeField] private GameObject streamViewCanvas;

    [SerializeField] private TMP_Text followersText;
    [SerializeField] private TMP_Text viewersText;

    private int currentFollowers = 10000;
    private int currentViewers = 1000;
    public string completedActivity;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        completedActivity = PlayerPrefs.GetString("CompletedActivity", "");
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
        Debug.Log("POSITIVE REACTION");
        int rNum = UnityEngine.Random.Range(0, positiveChatMessages.Length);

        chatImage.sprite = positiveChatMessages[rNum];

        currentViewers += GetViewerChanges(true);

        currentFollowers += GetFollowerChanges(true);


        followersText.text = currentFollowers.ToString();
        viewersText.text = currentViewers.ToString();
    }
    private void NegativeReaction()
    {
        Debug.Log("NEGATIVE REACTION");
        int rNum = UnityEngine.Random.Range(0, negativeChatMessages.Length);

        chatImage.sprite = negativeChatMessages[rNum];

        currentViewers -= GetViewerChanges(false);

        currentFollowers -= GetFollowerChanges(false);


        followersText.text = currentFollowers.ToString();

        viewersText.text = currentViewers.ToString();
    }

    private int GetFollowerChanges(bool isPositive)
    {
        int totalFollowers = 0;

        if (isPositive)
        {

        }
        //determine streamer x activity multiplier
        switch (GameManager.Instance.GetCurrentStreamer())
        {
            case "Amouranth":

                break;
            case "EthanH3H3":

                break;
            case "XQC":

                break;
            case "Destiny":

                break;

        }

        //determine activity success level
        

        return totalFollowers;
    }

    private int GetViewerChanges(bool isPositive)
    {
        int totalViewers = 0;
        if (isPositive)
        {
            //determine streamer x activity multiplier
            //if activity is synergistic with streamer more points
            //if activity isnt then less points
            float multiplier = 1f;
            switch (GameManager.Instance.GetCurrentStreamer())
            {
                case "Amouranth":
                    switch (completedActivity)
                    {
                        case "MarioKurt":
                            multiplier = .20f;
                            break;
                        case "FullGuys":
                            multiplier = .20f;
                            break;
                        case "PlayPiano":
                            multiplier = .20f;
                            break;
                        case "HotTub":
                            multiplier = .20f;
                            break;

                        //case "Eat":

                        //    break;

                        case "Dance":
                            multiplier = 1.50f;
                            break;

                        //case "PoliticalRant":

                        //    break;

                        case "Vape":
                            multiplier = 1f;
                            Debug.Log("Vape");

                            break;

                        case "Gamble":
                            multiplier = 1.50f;
                            break;
                        case "FactoryOH":
                            multiplier = .10f;
                            break;
                        default:

                            break;
                    }
                    break;
                case "EthanH3H3":
                    switch (completedActivity)
                    {
                        case "MarioKurt":

                            break;

                        case "FullGuys":

                            break;

                        case "PlayPiano":

                            break;

                        case "HotTub":

                            break;

                        //case "Eat":

                        //    break;

                        case "Dance":

                            break;

                        //case "PoliticalRant":

                        //    break;

                        case "Vape":
                            Debug.Log("Vape");

                            break;

                        case "Gamble":

                            break;
                        case "FactoryOH":

                            break;
                        default:

                            break;
                    }
                    break;
                case "XQC":
                    switch (completedActivity)
                    {
                        case "MarioKurt":

                            break;

                        case "FullGuys":

                            break;

                        case "PlayPiano":

                            break;

                        case "HotTub":

                            break;

                        //case "Eat":

                        //    break;

                        case "Dance":

                            break;

                        //case "PoliticalRant":

                        //    break;

                        case "Vape":
                            Debug.Log("Vape");

                            break;

                        case "Gamble":

                            break;
                        case "FactoryOH":

                            break;
                        default:

                            break;
                    }
                    break;
                case "Destiny":
                    switch (completedActivity)
                    {
                        case "MarioKurt":

                            break;

                        case "FullGuys":

                            break;

                        case "PlayPiano":

                            break;

                        case "HotTub":

                            break;

                        //case "Eat":

                        //    break;

                        case "Dance":

                            break;

                        //case "PoliticalRant":

                        //    break;

                        case "Vape":
                            Debug.Log("Vape");

                            break;

                        case "Gamble":

                            break;
                        case "FactoryOH":

                            break;
                        default:

                            break;
                    }
                    break;

            }

            //determine activity success level
            //multiply success level by modifier

        }


        return totalViewers;
    }

    IEnumerator WaitForCinemachineTransition()
    {
        do
        {
            yield return null;
        } while (GameManager.Instance.cameraBrain.IsBlending);

        streamViewCanvas.SetActive(true);
    }
}
