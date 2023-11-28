using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class DeathDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger happened");
        if(other.gameObject.CompareTag("Kart"))
        {
            Debug.Log("tag of kart triggered");
            Loader.Load(Loader.Scene.StreamerScene);
        }
    }
}
