using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private State state;
    [SerializeField] private float countdownToStartTimer;
    [SerializeField] private float pickingActivityTimerMax;
    float pickingActivityTimer;
    protected enum State
    {
        StreamView,
        PickingActivity,
        PlayingActivity,
    }

    private void Awake()
    {
        Instance = this;
        state = State.StreamView;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.StreamView:
                StreamManager.Instance.ShiftToStreamView();
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    pickingActivityTimer = pickingActivityTimerMax;
                    ActivityManager.Instance.ShiftToActivities();
                    StreamManager.Instance.ShiftOffStreamView();
                    state = State.PickingActivity;
                    Debug.Log("COUNTDOWN ENDED");
                }
                break;
            case State.PickingActivity:
                
                break;
           
            case State.PlayingActivity:
                //game ended
                break;
            default:
                break;
        }
        Debug.Log("state: " + state);
    }
}
