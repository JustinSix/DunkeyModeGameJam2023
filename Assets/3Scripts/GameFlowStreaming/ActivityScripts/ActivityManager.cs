using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


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

    private bool activityResult = false;
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
    }
    public enum StreamActivity
    {
        Game,
        PlayPiano,
        HotTub,
        //Eat,
        Dance,
        //PoliticalRant,
        Vape,
        Gamble,
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
        Debug.Log("started activity button clicked");
        activityCanvas.gameObject.SetActive(false);

        PlayerPrefs.SetString("CompletedActivity", activityName);
        StreamManager.Instance.completedActivity = activityName;
        switch (activityName)
        {
            case "MarioKurt":
                Debug.Log("MarioKurt");
                Loader.Load(Loader.Scene.MarioKurt); 
                break;

            case "FullGuys":
                Debug.Log("FullGuys");
                Loader.Load(Loader.Scene.FullGuysGame);
                break;

            case "PlayPiano":
                Debug.Log("PlayPiano");
                StartStreamRoomActivity(piano);
                break;

            case "HotTub":
                Debug.Log("HotTub");
                StartStreamRoomActivity(hotTub);
                break;

            //case "Eat":

            //    break;

            case "Dance":
                Debug.Log("Dance");
                StartStreamRoomActivity(dance);
                break;

            //case "PoliticalRant":

            //    break;

            case "Vape":
                Debug.Log("Vape");
                StartStreamRoomActivity(vaping);
                break;

            case "Gamble":
                Debug.Log("Gamble");
                Loader.Load(Loader.Scene.GambleGame);
                break;
            case "FactoryOH":
                Loader.Load(Loader.Scene.FactoryOH);
                break;
            default:

                break;
        }
        SetActivitySuccessCheckIfStreamerAudienceLikesActivity();
        GameManager.Instance.ChangeToPlayingActivity();
    }
    private void StartStreamRoomActivity(GameObject activityToEnable)
    {
        activityToEnable.SetActive(true);
        StartCoroutine(WaitForActivityCinemachineTransition());
        StartCoroutine(DisableStreamActivityAfter(activityToEnable));
    }

    IEnumerator WaitForActivityCinemachineTransition()
    {
        do
        {
            yield return null;
        } while (GameManager.Instance.cameraBrain.IsBlending);
        qteController.InitiateQTE();
    }
    IEnumerator DisableStreamActivityAfter(GameObject activityToDisable)
    {
        yield return new WaitForSeconds(10f);
        activityToDisable.SetActive(false);
        GameManager.Instance.ChangeToStreaming();
    }

    private void SetActivitySuccessCheckIfStreamerAudienceLikesActivity()
    {
        activityResult = true;
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
}
