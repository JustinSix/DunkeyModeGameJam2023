using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class ActivityManager : MonoBehaviour
{
    public static ActivityManager Instance { get; private set; }

    [SerializeField] private Transform pickActivityCamera;
    [SerializeField] private Transform activityCamera;
    [SerializeField] private GameObject activityCanvas;
    [SerializeField] private Activity[] activityArray;
    [SerializeField] private ActivityButton[] activityButtonArray;

    [Header("Activity Objects to Enable")]
    [SerializeField] private GameObject hotTub;
    [SerializeField] private GameObject vaping;
    [SerializeField] private GameObject piano;
    [SerializeField] private GameObject dance;
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

    //shift camera to screen
    //enable ui elements of activities
    //randomly pick 3 activities and set up button/visuals for picking

    public void ShiftToActivities()
    {
        pickActivityCamera.gameObject.SetActive(true);
        activityCanvas.SetActive(true);
        AssignRandomActivities();
    }
    public void ShiftOffActivities()
    {
        pickActivityCamera.gameObject.SetActive(false);
    }
    private void AssignRandomActivities()
    {
        Activity[] allActivities = activityArray;

        ShuffleArray(allActivities);

        Activity randomActivity1 = allActivities[0];
        Activity randomActivity2 = allActivities[1];
        Activity randomActivity3 = allActivities[2];

        //assign to buttons
        activityButtonArray[0].activityText.text = randomActivity1.activityName.ToString();
        activityButtonArray[0].activityButton.onClick.AddListener(() =>
        {
            StartActivity(randomActivity1.activityName.ToString());
        });

        activityButtonArray[1].activityText.text = randomActivity2.activityName.ToString();

        activityButtonArray[2].activityText.text = randomActivity3.activityName.ToString();
    }

    private void StartActivity(string activityName)
    {
        Debug.Log("started activity button clicked");
        switch (activityName)
        {
            case "MarioKurt":
                Loader.Load(Loader.Scene.MarioKurt); 
                break;

            case "FullGuys":
                Loader.Load(Loader.Scene.FullGuysGame);
                break;

            case "PlayPiano":
                StartStreamRoomActivity(piano);
                break;

            case "HotTub":
                //X
                StartStreamRoomActivity(hotTub);
                break;

            //case "Eat":

            //    break;

            case "Dance":
                StartStreamRoomActivity(dance);
                break;

            //case "PoliticalRant":

            //    break;

            case "Vape":
                StartStreamRoomActivity(vaping);
                break;

            case "Gamble":
                Loader.Load(Loader.Scene.GambleGame);
                break;

            default:

                break;
        }

    }
    private void StartStreamRoomActivity(GameObject activityToEnable)
    {
        activityToEnable.SetActive(true);
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
