using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
public class QTEController : MonoBehaviour
{
    [SerializeField] private GameObject QTECanvasO;
    [SerializeField] QTEKeyManager[] qteKeys;
    [SerializeField] KeyCode[] qteKeyCodesArray;
    [SerializeField] Sprite failedKeySprite;
    [SerializeField] Sprite succesfulKeySprite;
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
        // Check if any key is pressed down
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            if (Input.GetKeyDown(activeKeyCode))
            {
                SoundManager.Instance.SpawnSound(SoundManager.SoundName.WIN_QTE);

                succesfulQTE++;

                activeKeyM.outerKeyImage.sprite = succesfulKeySprite;

                ResetQTE();

                qteLeft--;
                if (qteLeft <= 0)
                {
                    qteActive = false;
                }
                else
                {
                    StartQTE();
                }
            }
            //spam clicked wrong keycode
            else
            {
                FailedQTE();
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
        SoundManager.Instance.SpawnSound(SoundManager.SoundName.FAIL_QTE1);

        Debug.Log("QTE Failed!");
        qteLeft--;

        activeKeyM.outerKeyImage.sprite = failedKeySprite;

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
        activeKeyM.FadeKeyOut();
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
        float rFloat = UnityEngine.Random.Range(.5f, 1.4f);
        qteDuration = rFloat;

        //assining fill Image
        activeKeyTimerImage = activeKeyM.fillTimerImage;
        activeKeyM.keyObject.SetActive(true);
        activeKeyM.FadeInKey();

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
