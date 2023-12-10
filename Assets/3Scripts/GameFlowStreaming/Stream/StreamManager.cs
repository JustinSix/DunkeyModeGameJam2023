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
    private void Awake()
    {
        Instance = this;
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

        int viewersToAdd = Mathf.RoundToInt(currentViewers * .3f);
        currentViewers += viewersToAdd;

        int followersToAdd = Mathf.RoundToInt(currentViewers * .15f);
        currentFollowers += followersToAdd;


        followersText.text = currentFollowers.ToString();
        viewersText.text = currentViewers.ToString();
    }
    private void NegativeReaction()
    {
        Debug.Log("NEGATIVE REACTION");
        int rNum = UnityEngine.Random.Range(0, negativeChatMessages.Length);

        chatImage.sprite = negativeChatMessages[rNum];

        int viewersToAdd = Mathf.RoundToInt(currentViewers * .3f);
        currentViewers -= viewersToAdd;

        int followersToAdd = Mathf.RoundToInt(currentViewers * .15f);

        currentFollowers -= followersToAdd;


        followersText.text = currentFollowers.ToString();

        viewersText.text = currentViewers.ToString();
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
