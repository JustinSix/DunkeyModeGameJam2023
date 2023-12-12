using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private State state;
    [SerializeField] private float countdownToStartTimer;
    [SerializeField] private float countdownToStartTimerMax;
    [SerializeField] private float pickingActivityTimerMax;

    public CinemachineBrain cameraBrain;

    [Header("Streamer Objects")]
    [SerializeField] private GameObject[] xqcObjects;
    [SerializeField] private GameObject[] amouranthObjects;
    [SerializeField] private GameObject[] ethanObjects;
    [SerializeField] private GameObject[] destinyObjects;
    [SerializeField] private TMP_Text streamerText;

    float pickingActivityTimer;
    private string chosenStreamer;
    private static bool loadedOnce = false;
    protected enum State
    {
        StartStream,
        StreamView,
        PickingActivity,
        PlayingActivity,
    }

    private void Awake()
    {
        Instance = this;

        state = State.PickingActivity;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Start()
    {
        chosenStreamer = PlayerPrefs.GetString("ChosenStreamer", "XQC");

        switch (chosenStreamer)
        {
            case "Amouranth":
                foreach (GameObject obj in amouranthObjects)
                {
                    obj.SetActive(true);
                }
                streamerText.text = "Amouranth";
                break;
            case "EthanH3H3":
                foreach (GameObject obj in ethanObjects)
                {
                    obj.SetActive(true);
                }
                streamerText.text = "H3H3";
                break;
            case "XQC":
                foreach (GameObject obj in xqcObjects)
                {
                    obj.SetActive(true);
                }
                streamerText.text = "XQC";
                break;
            case "Destiny":
                foreach (GameObject obj in destinyObjects)
                {
                    obj.SetActive(true);
                }
                streamerText.text = "Destiny";
                break;

        }
        if (!loadedOnce)
        {
            loadedOnce = true;
            pickingActivityTimer = pickingActivityTimerMax;
            ActivityManager.Instance.ShiftToActivities();
            StreamManager.Instance.ShiftOffStreamView();
        }
        else
        {
            state = State.StartStream;
        }

    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.StartStream:
                StreamManager.Instance.ShiftToStreamView();
                state = State.StreamView;
                break;
            case State.StreamView:              
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    pickingActivityTimer = pickingActivityTimerMax;
                    ActivityManager.Instance.ShiftToActivities();
                    StreamManager.Instance.ShiftOffStreamView();
                    state = State.PickingActivity;
                    Debug.Log("COUNTDOWN ENDED");
                }
                break;
            case State.PickingActivity:
                
                break;
           
            case State.PlayingActivity:
                //game ended
                break;
            default:
                break;
        }
    }

    public void ChangeToStreaming()
    {
        countdownToStartTimer = countdownToStartTimerMax;
        state = State.StartStream;
    }

    public void ChangeToPlayingActivity()
    {
        state = State.PlayingActivity;
    }
    public string GetCurrentStreamer()
    {
        return chosenStreamer;
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
