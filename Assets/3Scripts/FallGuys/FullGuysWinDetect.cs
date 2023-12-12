using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullGuysWinDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            PlayerPrefs.SetInt("ActivityResult", 1);

            PlayerPrefs.SetInt("CompletedActivityPoints", 10000);

            Loader.Load(Loader.Scene.StreamerScene);
        }
    }
}
