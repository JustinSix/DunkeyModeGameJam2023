using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryOHManager : MonoBehaviour
{
    [SerializeField] Image factoryImage;
    [SerializeField] TMP_Text infoText;
    [Header("Choice Buttons")]
    [SerializeField] FactoryButton[] choiceButtonArray;
    [SerializeField] Button backButton;

    [Header("Factory Sprites")]
    [SerializeField] Sprite nuclearcarFactory;
    [SerializeField] Sprite carFactory;
    [SerializeField] Sprite futuristicFactory;
    [SerializeField] Sprite abandonedFactory;
    [SerializeField] Sprite artFactory;

    [SerializeField] List<int> indexArray = new List<int>();
    [SerializeField] TMP_Text modeText;
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("ActivityResult", 1);

            PlayerPrefs.SetInt("CompletedActivityPoints", 800);

            Loader.Load(Loader.Scene.StreamerScene);
        });

        backButton.gameObject.SetActive(false);
        AssignFactoryToButtons();

        SoundManager.Instance.SpawnSound(SoundManager.SoundName.GAMINGMODE);
        modeText.text = "GAMING MODE";
        RectTransform modeTextRectTransform = modeText.GetComponent<RectTransform>();
        modeTextRectTransform.DOScale(1, .5f);
        StartCoroutine(LerpAnchoredPosition(modeTextRectTransform, new Vector2(0, 435), .4f));
    }

    private void AssignFactoryToButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            Sprite randomSpriteSelected;
            string factoryName;
            int factoryIndex = RandomIntForFactory();
            switch (factoryIndex)
            {
                case 0:
                    randomSpriteSelected = nuclearcarFactory;
                    factoryName = "Nuclear";
                    break;
                case 1:
                    randomSpriteSelected = carFactory;
                    factoryName = "Car";
                    break;
                case 2:
                    randomSpriteSelected = futuristicFactory;
                    factoryName = "Futuristic";
                    break;
                case 3:
                    randomSpriteSelected = abandonedFactory;
                    factoryName = "Black and White";
                    break;
                case 4:
                    randomSpriteSelected = artFactory;
                    factoryName = "Art";
                    break;
                default:
                    randomSpriteSelected = null;
                    factoryName = "error";
                    break;
            }
            choiceButtonArray[i].factoryButtonText.text = factoryName;
            choiceButtonArray[i].factoryButton.onClick.AddListener(() =>
            {
                PickFactory(factoryName, randomSpriteSelected);
            });

        }
    }
    private int RandomIntForFactory()
    {
        int returnThis;
        int rNum = Random.Range(0, indexArray.Count);
        returnThis = indexArray[rNum];

        indexArray.Remove(rNum);

        return returnThis;     
    }
    private void PickFactory(string factoryName, Sprite factorySprite)
    {
        factoryImage.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        factoryImage.sprite = factorySprite;

        infoText.text = "You built a factory!";

        for (int i = 0; i < 3; i++)
        {
            choiceButtonArray[i].gameObject.SetActive(false);
        }

        switch (factoryName) 
        {
            case "nuclear":
                break;
            case "car":
                break;
            case "futuristic":
                break;
            case "abandoned":
                break;
            case "art":
                break;
            default: 
                break;
        }
    }
    IEnumerator LerpAnchoredPosition(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        rectTransform.anchoredPosition = targetPos;

        Debug.Log("Lerping anchoredPosition complete!");

        yield return new WaitForSeconds(2f);

        modeText.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        modeText.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
