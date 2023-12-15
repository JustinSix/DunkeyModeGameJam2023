using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Standard video stuff")]
    [SerializeField] private VideoClip clip;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Button replayIntroButton;
    [SerializeField] private GameObject videoManagerObject;
    [Header("WebGL stuff")]
    [SerializeField] private string videoFileName;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;

        Invoke("PlayOnce", 2f);

        replayIntroButton.onClick.AddListener(() =>
        {
            PlayOnce();
        });
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        videoPlayer.Stop();
        SoundManager.Instance.ResumeMusic();
        videoManagerObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SkipVideo();
        }
    }
    private void PlayOnce()
    {
        videoManagerObject.SetActive(true);
        PlayVideo();
    }
    private void SkipVideo()
    {
        videoPlayer.Stop();
        SoundManager.Instance.ResumeMusic();
        videoManagerObject.SetActive(false);
    }
    public void PlayVideo()
    {
        SoundManager.Instance.PauseMusic();
        loadingPanel.SetActive(false);

#if UNITY_STANDALONE_WIN
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = clip;
        videoPlayer.Play();
#elif UNITY_WEBGL
        videoPlayer.source = VideoSource.Url;
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.Log(videoPath);
        videoPlayer.url = videoPath;
        videoPlayer.Play();
#endif
    }
}
