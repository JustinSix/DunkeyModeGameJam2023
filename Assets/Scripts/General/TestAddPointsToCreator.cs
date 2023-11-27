using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddPointsToCreator : MonoBehaviour
{
    [SerializeField] private float scoreToAdd;
    [SerializeField] private string creatorString;
    [SerializeField] private string playerNameString;
    public void TestAddPoints() 
    {
        LeaderboardManager.Instance.AddPlayerScore(playerNameString, scoreToAdd, creatorString);
    }

}
