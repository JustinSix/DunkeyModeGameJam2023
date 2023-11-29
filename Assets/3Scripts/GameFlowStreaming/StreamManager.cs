using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamManager : MonoBehaviour
{
    public static StreamManager Instance { get; private set; }

    [SerializeField] private Transform streamingWebCamera;
    private void Awake()
    {
        Instance = this;
    }

    public void ShiftOffStreamView()
    {
        streamingWebCamera.gameObject.SetActive(false);
    }

    public void ShiftToStreamView()
    {
        streamingWebCamera.gameObject.SetActive(true);
    }
    //pop in random image of chat have array of negative and positive chats, animate it in 
}
