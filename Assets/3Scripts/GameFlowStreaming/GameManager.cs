using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

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
    [SerializeField] private GameObject[] hasanObjects;
    [SerializeField] private GameObject[] pirateObjects;
    [SerializeField] private TMP_Text streamerText;
    [Header("Game over/Stream Over Objects")]
    [SerializeField] private GameObject streamEndCanvas;
    [SerializeField] private TMP_Text viewersGainedText;
    [SerializeField] private TMP_Text followersGainedText;
    [SerializeField] private TMP_Text contributeScoreButtonText;
    [SerializeField] private Button contributeScoreButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button howwemadeitButton;
    float pickingActivityTimer;
    private string chosenStreamer;
    private static bool loadedOnce = false;
    protected enum State
    {
        StartStream,
        StreamView,
        PickingActivity,
        PlayingActivity,
        GameOver,
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
            case "H3H3":
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
            case "Hasan":
                foreach (GameObject obj in hasanObjects)
                {
                    obj.SetActive(true);
                }
                streamerText.text = "Hasan";
                break;
            case "PirateSoftware":
                foreach (GameObject obj in pirateObjects)
                {
                    obj.SetActive(true);
                }
                streamerText.text = "PirateSoftware";
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

        if (LeaderboardManager.Instance != null)
        {
            contributeScoreButton.onClick.AddListener(() =>
            {
                int followersGained = StreamManager.Instance.GetFollowers() - 10000;

                LeaderboardManager.Instance.AddPlayerScore("generic player name", followersGained, chosenStreamer);

                contributeScoreButton.gameObject.SetActive(false);
            });
            contributeScoreButtonText.text = "Contribute Score to " + chosenStreamer;
        }
        else
        {
            contributeScoreButtonText.text = "error";
            Debug.LogError("leaderboard manager null");
        }
        mainMenuButton.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteKey("ChosenStreamer");
            PlayerPrefs.DeleteKey("CurrentViewers");
            PlayerPrefs.DeleteKey("CurrentFollowers");
            PlayerPrefs.DeleteKey("CompletedActivity");
            PlayerPrefs.DeleteKey("CompletedActivityPoints");
            PlayerPrefs.DeleteKey("ActivtiesCompleted");
            PlayerPrefs.DeleteKey("ActivityResult");

            loadedOnce = false;

            Loader.Load(Loader.Scene.MainMenu);
        });
        howwemadeitButton.onClick.AddListener(() =>
        {
            Application.OpenURL("https://www.youtube.com/playlist?list=PLN2-qKAB0Md0jd0plQ2UVMc0UngSfxAd6");
        });
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

                break;
            case State.GameOver:

                break;
            default:
                break;
        }
#if UNITY_EDITOR
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
#endif
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

    public void EndGame()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            // The user is signed in
            Debug.Log("User is signed in.");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        else
        {
            // The user is not signed in
            Debug.Log("User is not signed in.");
        }
        streamEndCanvas.gameObject.SetActive(true);
        int viewersGained = StreamManager.Instance.GetViewers() - 1000;
        viewersGainedText.text = viewersGained.ToString() + " viewers gained!";

        int followersGained = StreamManager.Instance.GetFollowers() - 10000;
        followersGainedText.text = followersGained.ToString() + " followers gained!";

        state = State.GameOver;
    }
}
