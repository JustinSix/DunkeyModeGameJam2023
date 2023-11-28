using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamManager : MonoBehaviour
{
    public static StreamManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
