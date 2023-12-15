using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;

public  class ActivityManager : MonoBehaviour
{
    public static ActivityManager Instance { get; private set; }

    [SerializeField] private GameObject pickActivityVCameraO;
    [SerializeField] private GameObject activityCanvas;
    [SerializeField] private Activity[] activityArray;
    [SerializeField] private ActivityButton[] activityButtonArray;

    [SerializeField] private QTEController qteController;

    [Header("Activity Objects to Enable")]
    [SerializeField] private GameObject hotTub;
    [SerializeField] private GameObject vaping;
    [SerializeField] private GameObject piano;
    [SerializeField] private GameObject dance;
    [Header("Activity Virtual Cameras to Enable")]
    [SerializeField] private GameObject hotTubVCameraO;
    [SerializeField] private GameObject vapingVCameraO;
    [SerializeField] private GameObject pianoVCameraO;
    [SerializeField] private GameObject danceVCameraO;

    [Header("VisualFeedback")]
    [SerializeField] private TMP_Text modeText;

    private bool activityResult = false;
    private int activitiesCompleted = 0;
    private int maxActivities = 8;
    public enum ActivityName
    {
        MarioKurt,
        FullGuys,
        PlayPiano,
        HotTub,
        //Eat,
        Dance,
        //PoliticalRant,
        Vape,
        Gamble,
        FactoryOH,
        AnimalWell,
        CallOfBooty,
    }
    public enum Streamer
    {
        All,
        XQC,
        Destiny,
        Hasan,
        Ethan,
        Amouranth,
    }

    private void Awake()
    {
        Instance = this;      
    }

    private void Start()
    {
        //save pref of outcome in scene of game
        //load pref of outcome here and set activity outcome here
        int resultInt = PlayerPrefs.GetInt("ActivityResult");
        if(resultInt == 0)
        {
            activityResult = false;
        }
        if (resultInt == 1)
        {
            activityResult = true;
        }
        //load pref of activities completed
        activitiesCompleted = PlayerPrefs.GetInt("ActivtiesCompleted",0);
    }

    //shift camera to screen
    //enable ui elements of activities
    //randomly pick 3 activities and set up button/visuals for picking

    public void ShiftToActivities()
    {
        pickActivityVCameraO.SetActive(true);


        AssignRandomActivities();
        StartCoroutine(WaitForStreamerCinemachineTransition());
    }
    IEnumerator WaitForStreamerCinemachineTransition()
    {
        do
        {
            yield return null;
        } while (GameManager.Instance.cameraBrain.IsBlending);

        activityCanvas.SetActive(true);
    }
    public void ShiftOffActivities()
    {
        pickActivityVCameraO.SetActive(false);
    }

    public bool GetActivityResult()
    {
        return activityResult;
    }

    private void AssignRandomActivities()
    {
        activityButtonArray[0].activityButton.onClick.RemoveAllListeners();
        activityButtonArray[1].activityButton.onClick.RemoveAllListeners();
        activityButtonArray[2].activityButton.onClick.RemoveAllListeners();

        Activity[] allActivities = activityArray;

        ShuffleArray(allActivities);

        Activity randomActivity1 = allActivities[0];
        Activity randomActivity2 = allActivities[1];
        Activity randomActivity3 = allActivities[2];

        //assign to button1
        activityButtonArray[0].activityButton.image.sprite = randomActivity1.ActivitySprite;
        activityButtonArray[0].activityText.text = randomActivity1.activityName.ToString();
        activityButtonArray[0].activityButton.onClick.AddListener(() =>
        {
            StartActivity(randomActivity1.activityName.ToString());
        });
        activityButtonArray[0].activityTitle = randomActivity1.activityName.ToString();
        activityButtonArray[0].activityName = randomActivity1.activityName;
        //assign to button2
        activityButtonArray[1].activityButton.image.sprite = randomActivity2.ActivitySprite;
        activityButtonArray[1].activityText.text = randomActivity2.activityName.ToString();
        activityButtonArray[1].activityButton.onClick.AddListener(() =>
        {
            StartActivity(randomActivity2.activityName.ToString());
        });
        activityButtonArray[1].activityTitle = randomActivity2.activityName.ToString();
        activityButtonArray[1].activityName = randomActivity2.activityName;
        //assign to button3
        activityButtonArray[2].activityButton.image.sprite = randomActivity3.ActivitySprite;
        activityButtonArray[2].activityText.text = randomActivity3.activityName.ToString();
        activityButtonArray[2].activityButton.onClick.AddListener(() =>
        {
            StartActivity(randomActivity3.activityName.ToString());
        });
        activityButtonArray[2].activityTitle = randomActivity3.activityName.ToString();
        activityButtonArray[2].activityName = randomActivity3.activityName;
    }

    private void StartActivity(string activityName)
    {
        activityCanvas.gameObject.SetActive(false);

        PlayerPrefs.SetString("CompletedActivity", activityName);
        StreamManager.Instance.completedActivity = activityName;
        switch (activityName)
        {
            case "MarioKurt":
                ActivtyCompleted(4);
                Loader.Load(Loader.Scene.MarioKurt); 
                break;

            case "FullGuys":
                ActivtyCompleted(4);
                Loader.Load(Loader.Scene.FullGuysGame);
                break;

            case "PlayPiano":
                ActivtyCompleted(1);
                SoundManager.Instance.PauseMusic();
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.GOODPIANOPLAY);
                StreamManager.Instance.activityEarnedPoints = 1000;
                StartStreamRoomActivity(piano);
                break;

            case "HotTub":
                ActivtyCompleted(1);
                SoundManager.Instance.PauseMusic();
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.SONGHOTTUB);
                StreamManager.Instance.activityEarnedPoints = 1000;
                StartStreamRoomActivity(hotTub);
                break;

            //case "Eat":

            //    break;

            case "Dance":
                ActivtyCompleted(1);
                SoundManager.Instance.PauseMusic();
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.SONGDANCE);
                StreamManager.Instance.activityEarnedPoints = 1000;
                StartStreamRoomActivity(dance);
                break;

            //case "PoliticalRant":

            //    break;

            case "Vape":
                ActivtyCompleted(1);
                StreamManager.Instance.activityEarnedPoints = 1000;
                StartStreamRoomActivity(vaping);
                break;

            case "Gamble":
                ActivtyCompleted(2);
                Loader.Load(Loader.Scene.GambleGame);
                break;
            case "FactoryOH":
                ActivtyCompleted(1);
                Loader.Load(Loader.Scene.FactoryOH);
                break;
            case "AnimalWell":
                ActivtyCompleted(1);
                Loader.Load(Loader.Scene.AnimalWell);
                break;
            case "CallOfBooty":
                ActivtyCompleted(1);
                Loader.Load(Loader.Scene.CallOfBooty);
                break;
            default:

                break;
        }
        GameManager.Instance.ChangeToPlayingActivity();
    }
    private void StartStreamRoomActivity(GameObject activityToEnable)
    {
        activityToEnable.SetActive(true);
        StartCoroutine(WaitForActivityCinemachineTransition(activityToEnable.name));
        StartCoroutine(DisableStreamActivityAfter(activityToEnable));
    }

    IEnumerator WaitForActivityCinemachineTransition(string activityName)
    {
        do
        {
            yield return null;
        } while (GameManager.Instance.cameraBrain.IsBlending);

        PlayCorrectStreamerRoomActivityModeSound(activityName);

        modeText.text = activityName + " MODE";
        RectTransform modeTextRectTransform = modeText.GetComponent<RectTransform>();
        modeTextRectTransform.DOScale(1, .5f);
        StartCoroutine(LerpAnchoredPosition(modeTextRectTransform, new Vector2(0, 435), .4f));

        yield return new WaitForSeconds(1f);
        modeTextRectTransform.localScale = Vector3.zero;
        modeTextRectTransform.anchoredPosition = Vector3.zero;

        qteController.InitiateQTE();
    }
    IEnumerator DisableStreamActivityAfter(GameObject activityToDisable)
    {
        yield return new WaitForSeconds(10f);
        activityToDisable.SetActive(false);
        qteController.EndQTE();
        SoundManager.Instance.ResumeMusic();
        GameManager.Instance.ChangeToStreaming();
    }

    public void SetActivityResult(bool wonActivity)
    {
        activityResult = wonActivity;
    }
    private void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    private void PlayCorrectStreamerRoomActivityModeSound(string mode)
    {
        switch (mode)
        {
            case "PIANO":
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.PIANOMODE);
                break;
            case "HOT TUB":
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.HOTTUBMODE);
                break;
            case "DANCE":
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.DANCEMODE);
                break;
            case "VAPE":
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.VAPEMODE);
                break;
            default:
                Debug.LogError("no mode check acvitty manager");
                break;
        }
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
    }

    private void ActivtyCompleted(int activityWeightToAdd)
    {
        activitiesCompleted += activityWeightToAdd;
        PlayerPrefs.SetInt("ActivtiesCompleted", activitiesCompleted);
    }
    public bool GetIfActivitiesCompleted()
    {
        if(activitiesCompleted >= maxActivities)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
