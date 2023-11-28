using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class ActivityManager : MonoBehaviour
{
    public static ActivityManager Instance { get; private set; }

    [SerializeField] private Activity[] activityArray;
    [SerializeField] private ActivityButton[] activityButtonArray;

    public enum ActivityName
    {
        MarioKurt,
        FullGuys,
        PlayPiano,
        HotTub,
        Eat,
        Dance,
        PoliticalRant,
        Vape,
        Gamble,
    }
    public enum StreamActivity
    {
        Game,
        PlayPiano,
        HotTub,
        Eat,
        Dance,
        PoliticalRant,
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
        AssignRandomActivities();
    }

    //shift camera to screen
    //enable ui elements of activities
    //randomly pick 3 activities and set up button/visuals for picking

    public void ShiftToActivities()
    {

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
        switch (activityName)
        {
            default:
                break;
        }
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
