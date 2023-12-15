using UnityEngine;
public class ResetData : MonoBehaviour
{
    [SerializeField] private bool isMainMenu = false;
    private void Start()
    {
        if(isMainMenu)
        {
            PlayerPrefs.DeleteAll();
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
