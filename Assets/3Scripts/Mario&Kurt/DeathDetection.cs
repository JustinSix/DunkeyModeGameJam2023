using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class DeathDetection : MonoBehaviour
{
    [SerializeField] MarioKurtManager marioKurtManager;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger happened");
        if(other.gameObject.CompareTag("Kart"))
        {
            Debug.Log("tag of kart triggered");

            marioKurtManager.CalculateResults(false);

            Loader.Load(Loader.Scene.StreamerScene);
        }
    }
}
