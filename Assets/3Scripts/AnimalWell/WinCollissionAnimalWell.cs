using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollissionAnimalWell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterController2D characterController = other.GetComponent<CharacterController2D>();

        if (characterController != null)
        {
            PlayerPrefs.SetInt("ActivityResult", 1);

            PlayerPrefs.SetInt("CompletedActivityPoints", 1000);

            Loader.Load(Loader.Scene.StreamerScene);
        }
    }
}
