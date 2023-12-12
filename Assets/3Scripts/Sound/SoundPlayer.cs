using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]private AudioSource aS;
    public float timeTillDestroy = 3f;
    private void Start()
    {
        Invoke("DestroySelf", timeTillDestroy);
    }
    public void SetAudioClipAndPlay(AudioClip audioClip, float volume)
    {
        aS.clip = audioClip;
        aS.volume = volume;
        aS.Play();
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
