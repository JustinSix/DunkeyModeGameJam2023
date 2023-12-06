using UnityEngine;
using System;
public class QTEController : MonoBehaviour
{

    public static event EventHandler QTEFailed;
    public static event EventHandler QTEStarted;
    public KeyCode qteKey = KeyCode.Space;  // Change the key as needed
    public float qteDuration = 3f;  // Adjust the duration of the QTE

    private bool qteActive = false;
    private float timer = 0f;

    private void Update()
    {
        if (qteActive)
        {
            HandleQTEInput();
            UpdateTimer();
        }
        else
        {
            if (Input.GetKeyDown(qteKey))
            {
                StartQTE();
            }
        }
    }

    private void HandleQTEInput()
    {
        if (Input.GetKeyDown(qteKey))
        {
            // QTE successful
            Debug.Log("QTE Successful!");
            ResetQTE();
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;

        if (timer >= qteDuration)
        {
            // QTE failed
            FailedQTE();
        }
    }

    private void FailedQTE()
    {
        QTEFailed?.Invoke(this, EventArgs.Empty);
        Debug.Log("QTE Failed!");
        ResetQTE();
    }

    public void StartQTE()
    {
        QTEStarted?.Invoke(this, EventArgs.Empty);  
        qteActive = true;
        timer = 0f;
        Debug.Log("QTE Started!");
    }


    private void ResetQTE()
    {
        qteActive = false;
        timer = 0f;
    }
}
