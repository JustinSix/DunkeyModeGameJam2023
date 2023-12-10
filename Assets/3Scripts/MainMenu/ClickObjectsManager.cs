using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObjectsManager : MonoBehaviour
{
    // Event to register for when the object is clicked
    public event EventHandler Clicked;
    [SerializeField] MainMenu mainMenuS;

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse button down registered");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Debug visualization for the raycast line
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

            // Perform a raycast to check if the object is clicked
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Clicked on: " + hit.collider.name);
                mainMenuS.ChooseStreamer(hit.collider.name);
            }
        }
    }


}
