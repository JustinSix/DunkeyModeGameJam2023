using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Streamer", menuName = "New Streamer")]
public class Streamer : ScriptableObject
{
    public float MarioKurtMultiplier = .2f;
    public float FullGuysMultiplier = .2f;
    public float PlayPianoMultiplier = .2f;
    public float HotTubMultiplier = 2f;
    public float DanceMultiplier = 1.5f;
    public float VapeMultiplier = 1f;
    public float GambleMultiplier = 1.5f;
    public float FactoryOhMultiplier = .1f;
}
