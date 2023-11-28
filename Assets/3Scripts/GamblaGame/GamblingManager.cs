using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GamblingManager : MonoBehaviour
{
    [SerializeField] private Button betOneButton;
    [SerializeField] private Button betMaxButton;
    [SerializeField] private Button spinButton;
    public void Start()
    {
        betOneButton.onClick.AddListener(() =>
        {

        });
        betMaxButton.onClick.AddListener(() =>
        {

        });
        spinButton.onClick.AddListener(() =>
        {

        });
    }
    public void Spin()
    {
        //initiate spinning
    }
    
}
