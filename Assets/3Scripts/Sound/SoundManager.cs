using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private GameObject soundPlayerPrefab;
    [SerializeField] private Transform defaultSoundSpawnTransform;

    [Header("Audio Clips for SFX")]
    [SerializeField] private AudioClip failQTE1;
    [SerializeField] private AudioClip failQTE2;
    [SerializeField] private AudioClip winQTE;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip losingSound;
    [SerializeField] private AudioClip streamerSelectedSound;
    [Header("Audio Clips for MODES")]
    [SerializeField] private AudioClip danceMode;
    [SerializeField] private AudioClip vapeMode;
    [SerializeField] private AudioClip hottubMode;
    [SerializeField] private AudioClip pianoMode;
    [SerializeField] private AudioClip gamingMode;
    [Header("Audio Clips for MODE Songs")]
    [SerializeField] private AudioClip pianoGoodSound;
    [SerializeField] private AudioClip pianoBadSound;
    [SerializeField] private AudioClip hotTubSong;
    [SerializeField] private AudioClip danceSong;
    public enum SoundName
    {
        FAIL_QTE1,
        FAIL_QTE2,
        WIN_QTE,
        VICTORY_SOUND,
        LOSING_SOUND,
        DANCEMODE,
        VAPEMODE,
        HOTTUBMODE,
        PIANOMODE,
        GAMINGMODE,
        GOODPIANOPLAY,
        BADPIANOPLAY,
        SONGHOTTUB,
        SONGDANCE,
        SELECTEDSTREAMER,
    }

    private void Awake()
    {
        Instance = this;
    }
    public void SpawnSound(SoundName soundName)
    {
        AudioClip clipToPlay;
        float volume = 0.1f;
        float timeTillDestroy = 3f;
        switch (soundName)
        {
            case SoundName.FAIL_QTE1:
                clipToPlay = failQTE1;
                volume = 0.2f;
                break;
            case SoundName.FAIL_QTE2:
                clipToPlay = failQTE2;
                volume = 0.2f;
                break;
            case SoundName.WIN_QTE:
                clipToPlay = winQTE;
                volume = 0.1f;
                break;
            case SoundName.VICTORY_SOUND:
                clipToPlay = victorySound;
                volume = 0.4f;
                break;
            case SoundName.LOSING_SOUND:
                clipToPlay = losingSound;
                volume = 0.4f;
                break;
            case SoundName.DANCEMODE:
                clipToPlay = danceMode;
                volume = 0.3f;
                break;
            case SoundName.VAPEMODE:
                clipToPlay = vapeMode;
                volume = 0.3f;
                break;
            case SoundName.HOTTUBMODE:
                clipToPlay = hottubMode;
                volume = 0.3f;
                break;
            case SoundName.PIANOMODE:
                clipToPlay = pianoMode;
                volume = 0.3f;
                break;
            case SoundName.GAMINGMODE:
                clipToPlay = gamingMode;
                volume = 0.3f;
                break;
            case SoundName.GOODPIANOPLAY:
                clipToPlay = pianoGoodSound;
                volume = 0.2f;
                timeTillDestroy = 10;
                break;
            case SoundName.BADPIANOPLAY:
                clipToPlay = pianoBadSound;
                volume = 0.2f;
                timeTillDestroy = 10;
                break;
            case SoundName.SONGDANCE:
                clipToPlay = danceSong;
                volume = 0.17f;
                timeTillDestroy = 10;
                break;
            case SoundName.SONGHOTTUB:
                clipToPlay = hotTubSong;
                volume = 0.17f;
                timeTillDestroy = 10;
                break;
            case SoundName.SELECTEDSTREAMER:
                clipToPlay = streamerSelectedSound;
                volume = 0.15f;
                break;
            default:
                clipToPlay = null;
                break;
        }


        GameObject spawnedSoundO = Instantiate(soundPlayerPrefab, defaultSoundSpawnTransform);
        spawnedSoundO.GetComponent<SoundPlayer>().SetAudioClipAndPlay(clipToPlay, volume);
        spawnedSoundO.GetComponent<SoundPlayer>().timeTillDestroy = timeTillDestroy;
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }
    public void ResumeMusic()
    {
        musicAudioSource.UnPause();
    }

}
