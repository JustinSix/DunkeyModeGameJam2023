using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarioKurtManager : MonoBehaviour
{
    [SerializeField] TMP_Text modeText;
    [Header("Kart Peoples")]
    [SerializeField] private GameObject xqcObject;
    [SerializeField] private GameObject ethanObject;
    [SerializeField] private GameObject amouranthObject;
    [SerializeField] private GameObject destinyObject;
    [SerializeField] private GameObject hasanObject;
    [SerializeField] private int activityPointsValue;
    
    [Header("Timer Stuff")]
    public TMP_Text timerText;
    private float currentTime = 0.0f;
    public bool gameEnded = false;
    [Header("Reset Game Stuff")]
    [SerializeField] private Transform kartTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform startingKartTransform;
    [SerializeField] private Transform startingCameraTransform;
    private string chosenStreamer;

    private void Start()
    {
        chosenStreamer = PlayerPrefs.GetString("ChosenStreamer", "XQC");
        switch (chosenStreamer)
        {
            case "Amouranth":
                amouranthObject.SetActive(true);
                break;
            case "EthanH3H3":
                ethanObject.SetActive(true);
                break;
            case "XQC":
                xqcObject.SetActive(true);
                break;
            case "Destiny":
                destinyObject.SetActive(true);  
                break;
            case "Hasan":
                hasanObject.SetActive(true);
                break;
            default:
                xqcObject.SetActive(true);
                break;
        }

        SoundManager.Instance.SpawnSound(SoundManager.SoundName.GAMINGMODE);
        modeText.text = "GAMING MODE";
        RectTransform modeTextRectTransform = modeText.GetComponent<RectTransform>();
        modeTextRectTransform.DOScale(1, .5f);
        StartCoroutine(LerpAnchoredPosition(modeTextRectTransform, new Vector2(0, 435), .4f));
    }

    private void Update()
    {
        if (!gameEnded)
        {
            currentTime += Time.deltaTime;
            timerText.text = FormatTime(currentTime);
        }
        if (Input.GetKey(KeyCode.R))
        {
            gameEnded = false;
            currentTime = 0;

            kartTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;

            kartTransform.position = startingKartTransform.position;
            kartTransform.rotation = startingKartTransform.rotation;

            cameraTransform.position = startingCameraTransform.position;
            cameraTransform.rotation = startingCameraTransform.rotation;
        }

    }
    string FormatTime(float time)
    {
        // Format time as minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void CalculateResults(bool won)
    {
        if(won)
        {
            PlayerPrefs.SetInt("ActivityResult", 1);
        }
        else
        {
           PlayerPrefs.SetInt("ActivityResult", 0);
        }
        PlayerPrefs.SetInt("CompletedActivityPoints", activityPointsValue);
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

        yield return new WaitForSeconds(2f);

        modeText.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        modeText.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
