using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamViewVisualsManager : MonoBehaviour
{
    [SerializeField] private RectTransform chatObject;
    private void OnEnable()
    {
        Debug.LogWarning("stream view enabled");
        chatObject.DOMoveY(250f, 1f);
        //155 = 300f
        //250 = 39
    }
    private void OnDisable()
    {
        Debug.LogWarning("stream view disabled");
        chatObject.position = new Vector3(chatObject.position.x, 800f, chatObject.position.z);
    }

}
