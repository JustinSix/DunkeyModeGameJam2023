using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity", menuName = "New Activity")]
public class Activity : ScriptableObject
{
    public ActivityManager.ActivityName activityName;
    public Loader.Scene sceneToLoad;
    public Sprite ActivitySprite;
}
