using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Button startStreamingButton;
    // Start is called before the first frame update
    void Start()
    {
        startStreamingButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.StreamerScene);
        });
    }

    public void ChooseStreamer(string streamerClickedOnName)
    {
        //Debug.Log("Chosen streamer: " + streamerClickedOnName);
        PlayerPrefs.SetString("ChosenStreamer", streamerClickedOnName);
    }

}
