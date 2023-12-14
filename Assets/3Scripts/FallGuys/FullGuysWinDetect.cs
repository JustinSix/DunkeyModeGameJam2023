using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullGuysWinDetect : MonoBehaviour
{
    [SerializeField] FullGuysManager fullGuysManager;
    [SerializeField] private GameObject fireworksObject;
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            fullGuysManager.CalculateResults(true);

            fireworksObject.SetActive(true);
            SoundManager.Instance.SpawnSound(SoundManager.SoundName.MARIOKURTVICTORY);

            StartCoroutine(DelayedLoadScene());
        }
    }
    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(1.4f);
        Loader.Load(Loader.Scene.StreamerScene);
    }
}
