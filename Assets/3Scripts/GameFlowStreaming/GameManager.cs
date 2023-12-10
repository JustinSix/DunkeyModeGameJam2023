using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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

    float pickingActivityTimer;
    private string chosenStreamer;
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
        state = State.StartStream;
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
                break;
            case "EthanH3H3":
                foreach (GameObject obj in ethanObjects)
                {
                    obj.SetActive(true);
                }
                break;
            case "XQC":
                foreach (GameObject obj in xqcObjects)
                {
                    obj.SetActive(true);
                }
                break;

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
}
