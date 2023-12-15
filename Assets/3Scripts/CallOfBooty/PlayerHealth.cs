using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int playerHealth = 100;

    public void DamageHealth()
    {
        playerHealth -= 25;
        if (playerHealth <= 0)
        {
            HandlePlayerDeath();
        }
    }
    private void HandlePlayerDeath()
    {
        CallOfBootyGameManager.Instance.LoseGame();
    }
}
