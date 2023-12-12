using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamViewVisualsManager : MonoBehaviour
{
    [SerializeField] private RectTransform chatRectTransform;
    private void OnEnable()
    {
        //Debug.LogWarning("stream view enabled");
        StartCoroutine(LerpAnchoredPosition(chatRectTransform, new Vector2(-760.0001f, 40), 1f));
    }
    private void OnDisable()
    {
        //Debug.LogWarning("stream view disabled");
        chatRectTransform.anchoredPosition = new Vector3(-760.0001f, 821.79f, 0);
    }

    IEnumerator LerpAnchoredPosition(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        rectTransform.anchoredPosition = targetPos;

        Debug.Log("Lerping anchoredPosition complete!");
    }
}
