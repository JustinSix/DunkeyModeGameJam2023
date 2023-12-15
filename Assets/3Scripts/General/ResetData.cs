using UnityEngine;
public class ResetData : MonoBehaviour
{
    [SerializeField] private bool isMainMenu = false;
    private void Start()
    {
        if(isMainMenu)
        {
            PlayerPrefs.DeleteKey("ChosenStreamer");
            PlayerPrefs.DeleteKey("CurrentViewers");
            PlayerPrefs.DeleteKey("CurrentFollowers");
            PlayerPrefs.DeleteKey("CompletedActivity");
            PlayerPrefs.DeleteKey("CompletedActivityPoints");
            PlayerPrefs.DeleteKey("ActivtiesCompleted");
            PlayerPrefs.DeleteKey("ActivityResult");

        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("ChosenStreamer");
        PlayerPrefs.DeleteKey("CurrentViewers");
        PlayerPrefs.DeleteKey("CurrentFollowers");
        PlayerPrefs.DeleteKey("CompletedActivity");
        PlayerPrefs.DeleteKey("CompletedActivityPoints");
        PlayerPrefs.DeleteKey("ActivtiesCompleted");
        PlayerPrefs.DeleteKey("ActivityResult");

    }
}
