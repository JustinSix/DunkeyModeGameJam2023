using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObjectsManager : MonoBehaviour
{
    [SerializeField] MainMenu mainMenuS;
    [SerializeField] private GameObject xqcSpotlight;
    [SerializeField] private GameObject amouranthSpotlight;
    [SerializeField] private GameObject destinySpotlight;
    [SerializeField] private GameObject ethanSpotlight;
    [SerializeField] private GameObject hasanSpotlight;
    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Debug visualization for the raycast line
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

            // Perform a raycast to check if the object is clicked
            if (Physics.Raycast(ray, out hit))
            {
                string chosenStreamer = hit.collider.name;
                mainMenuS.ChooseStreamer(hit.collider.name);
                SpotlightSelectedStreamer(chosenStreamer);
            }
        }
    }

    private void SpotlightSelectedStreamer(string chosenStreamer)
    {
        SoundManager.Instance.SpawnSound(SoundManager.SoundName.SELECTEDSTREAMER);
        switch (chosenStreamer)
        {
            case "Amouranth":
                amouranthSpotlight.SetActive(true);

                ethanSpotlight.SetActive(false);
                xqcSpotlight.SetActive(false);
                destinySpotlight.SetActive(false);
                hasanSpotlight.SetActive(false);
                break;
            case "EthanH3H3":
                ethanSpotlight.SetActive(true);

                amouranthSpotlight.SetActive(false);
                xqcSpotlight.SetActive(false);
                destinySpotlight.SetActive(false);
                hasanSpotlight.SetActive(false);
                break;
            case "XQC":
                xqcSpotlight.SetActive(true);

                ethanSpotlight.SetActive(false);
                amouranthSpotlight.SetActive(false);
                destinySpotlight.SetActive(false);
                hasanSpotlight.SetActive(false);
                break;
            case "Destiny":
                destinySpotlight.SetActive(true);

                ethanSpotlight.SetActive(false);
                xqcSpotlight.SetActive(false);
                amouranthSpotlight.SetActive(false);
                hasanSpotlight.SetActive(false);
                break;
            case "Hasan":
                hasanSpotlight.SetActive(true);

                ethanSpotlight.SetActive(false);
                xqcSpotlight.SetActive(false);
                amouranthSpotlight.SetActive(false);
                destinySpotlight.SetActive(false);
                break;

        }
    }
}
