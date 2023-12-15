using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;
public class EnemyCollision : MonoBehaviour
{
    [SerializeField] int enemyHealth = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject shotVFX;
    [SerializeField] PlayerHealth playerHealth;
    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if(character != null)
        {
            playerHealth.DamageHealth();
            HandleDeath();
        }
        else
        {
            if(collision.gameObject.name == "P_LPSP_PROJ_Bullet_01(Clone)")
            {
                enemyHealth -= 25;
                if(enemyHealth <= 0)
                {
                    HandleDeath();
                }
                else
                {
                    StartCoroutine(EnableThenDie(shotVFX, false));
                }
            }

        }
    }

    private void HandleDeath()  
    {
        StartCoroutine(EnableThenDie(deathVFX, true));
    }

    IEnumerator EnableThenDie(GameObject vfxToEnable, bool shouldDie)
    {
        vfxToEnable.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (shouldDie)
        {
            Destroy(gameObject);
        }

    }
}
