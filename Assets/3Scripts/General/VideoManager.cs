using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Button replayIntroButton;
    [SerializeField] private GameObject videoManagerObject;
    float timer = 0;
    private bool didOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;

        videoPlayer.Prepare();

        replayIntroButton.onClick.AddListener(() =>
        {
            PlayOnce();
        });
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        videoPlayer.Stop();
        videoManagerObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!didOnce)
        {
            if (videoPlayer.isPrepared)
            {
                loadingPanel.SetActive(false);
                videoPlayer.Play();
                didOnce = true;
            }
        }
        if(Input.anyKeyDown)
        {
            SkipVideo();
        }
    }

    private void PlayOnce()
    {
        videoManagerObject.SetActive(true);
        videoPlayer.Prepare();
        didOnce = false;
    }
    private void SkipVideo()
    {
        videoPlayer.Stop();
        didOnce = true;
        videoManagerObject.SetActive(false);
    }
}
