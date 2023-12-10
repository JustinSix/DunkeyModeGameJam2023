using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioKurtManager : MonoBehaviour
{
    [SerializeField] private GameObject xqcObject;
    [SerializeField] private GameObject ethanObject;
    [SerializeField] private GameObject amouranthObject;
    [SerializeField] private GameObject destinyObject;
    private string chosenStreamer;
    private void Start()
    {
        chosenStreamer = PlayerPrefs.GetString("ChosenStreamer", "XQC");
        switch (chosenStreamer)
        {
            case "Amouranth":
                amouranthObject.SetActive(true);
                break;
            case "EthanH3H3":
                ethanObject.SetActive(true);
                break;
            case "XQC":
                xqcObject.SetActive(true);
                break;
            case "Destiny":
                destinyObject.SetActive(true);  
                break;
            default:
                xqcObject.SetActive(true);
                break;
        }
    }
}
