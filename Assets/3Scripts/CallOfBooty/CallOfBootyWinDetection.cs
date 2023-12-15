using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;
public class CallOfBootyWinDetection : MonoBehaviour
{
    [SerializeField] private GameObject fireworksObject;
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.gameObject.GetComponent<Character>();

        if (character != null)
        {
            CallOfBootyGameManager.Instance.CalculateResults(true);

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
