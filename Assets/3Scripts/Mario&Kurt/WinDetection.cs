using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDetection : MonoBehaviour
{
    [SerializeField] MarioKurtManager marioKurtManager;
    [SerializeField] private GameObject fireWorks;
    [SerializeField] private GameObject loseDetection;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            marioKurtManager.gameEnded = true;
            loseDetection.SetActive(false); 

            fireWorks.SetActive(true);
            SoundManager.Instance.SpawnSound(SoundManager.SoundName.MARIOKURTVICTORY);

            marioKurtManager.CalculateResults(true);

            StartCoroutine(DelayedLoadScene());
        }
    }
    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(1.4f);
        Loader.Load(Loader.Scene.StreamerScene);
    }
}
