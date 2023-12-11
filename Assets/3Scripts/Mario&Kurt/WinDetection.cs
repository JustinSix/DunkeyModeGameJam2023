using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDetection : MonoBehaviour
{
    [SerializeField] MarioKurtManager marioKurtManager;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger happened");
        if (other.gameObject.CompareTag("Kart"))
        {
            Debug.Log("tag of kart triggered");

            marioKurtManager.CalculateResults(true);

            Loader.Load(Loader.Scene.StreamerScene);
        }
    }
}
