using UnityEngine;
using System;
using UnityEngine.UI;
public class QTEController : MonoBehaviour
{
    [SerializeField] private GameObject QTECanvasO;
    [SerializeField] QTEKeyManager[] qteKeys;
    [SerializeField] KeyCode[] qteKeyCodesArray;

    private bool qteActive = false;

    private int totalQTE = 0;
    private int qteLeft = 0;
    private int succesfulQTE = 0;

    private QTEKeyManager activeKeyM;
    private KeyCode activeKeyCode;
    private Image activeKeyTimerImage;

    private float qteDuration = 3f;
    private float timer = 0f;
    private void Update()
    {
        if (qteActive)
        {
            HandleQTEInput();
            UpdateTimer();
        }
    }

    private void HandleQTEInput()
    {
        if (Input.GetKeyDown(activeKeyCode))
        {
            // QTE successful
            Debug.Log("QTE Successful!");
            succesfulQTE++;

            ResetQTE();

            qteLeft--;
            if(qteLeft <= 0)
            {
                qteActive = false;
            }
            else
            {
                StartQTE();
            }
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;

        activeKeyTimerImage.fillAmount = timer/qteDuration;

        if (timer >= qteDuration)
        {
            // QTE failed
            FailedQTE();
        }
    }

    private void FailedQTE()
    {
        //QTECanvasO.SetActive(false);
        Debug.Log("QTE Failed!");
        qteLeft--;
        ResetQTE();
        StartQTE();
    }

    public void InitiateQTE()
    {
        QTECanvasO.SetActive(true);

        Debug.Log("QTE Initiated!");

        int rNum = UnityEngine.Random.Range(8, 16);
        qteLeft = rNum;
        totalQTE = rNum;
        StartQTE();
    }


    private void ResetQTE()
    {
        activeKeyM.keyObject.SetActive(false);
        timer = 0f;
    }

    private void StartQTE()
    {
        Debug.Log("QTE Started!");
        //pick random qte key
        int rNumK = UnityEngine.Random.Range(0, qteKeys.Length);
        activeKeyM = qteKeys[rNumK];

        //pick random key code
        int rNumKC = UnityEngine.Random.Range(0, qteKeyCodesArray.Length);
        activeKeyCode = qteKeyCodesArray[rNumKC];

        //pick random time for QTE
        float rFloat = UnityEngine.Random.Range(.4f, 1.5f);
        qteDuration = rFloat;

        //assining fill Image
        activeKeyTimerImage = activeKeyM.fillTimerImage;
        activeKeyM.keyObject.SetActive(true);
        Debug.Log("KeyCode: " + activeKeyCode + "KeyCode to string: " + activeKeyCode.ToString());
        activeKeyM.qteKeyText.text = activeKeyCode.ToString();
        //qte to active
        qteActive = true;
        timer = 0f;
    }
    public void EndQTE()
    {
        float successPercentage = (float)succesfulQTE / (float)totalQTE;
        Debug.Log("success percentage float: " + successPercentage);
        bool wonQTE = false;
        if (successPercentage > .5f)
        {
            wonQTE = true;
        }
        Debug.Log("wonQTE bool is: " + wonQTE);
        ActivityManager.Instance.SetActivityResult(wonQTE);
        ResetQTE();
        qteLeft = 0;
        totalQTE = 0;
        succesfulQTE = 0;
        qteActive = false;
    }
}
