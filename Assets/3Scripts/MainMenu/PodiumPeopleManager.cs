using System;
using UnityEngine;
using TMPro;
public class PodiumPeopleManager : MonoBehaviour
{
    [Header("Streamer Transforms")]
    [SerializeField] private Transform ethanTransform;
    [SerializeField] private Transform xqcTransform;
    [SerializeField] private Transform destinyTransform;
    [SerializeField] private Transform amouranthTransform;
    [SerializeField] private Transform hasanTransform;

    [Header("Placements")]
    [SerializeField] private Transform firstPlaceTransform;
    [SerializeField] private Transform secondPlaceTransform;
    [SerializeField] private Transform thirdPlaceTransform;
    [SerializeField] private Transform fourthPlaceTransform;
    [SerializeField] private Transform fifthPlaceTransform;

    [Header("Streamer Score Texts")]
    [SerializeField] private TMP_Text destinyScoreTMP;
    [SerializeField] private TMP_Text xqcScoreTMP;
    [SerializeField] private TMP_Text hasanScoreTMP;
    [SerializeField] private TMP_Text amouranthScoreTMP;
    [SerializeField] private TMP_Text h3h3ScoreTMP;

    public void TakeScoresAndOrganizeStreamersAndScores(string destinyScoreText, string xqcScoreText, string hasanScoreText, string amouranthScoreText, string h3h3ScoreText)
    {
        //checking for null or empty
        destinyScoreText = string.IsNullOrEmpty(destinyScoreText) ? "0" : destinyScoreText;
        xqcScoreText = string.IsNullOrEmpty(xqcScoreText) ? "0" : xqcScoreText;
        hasanScoreText = string.IsNullOrEmpty(hasanScoreText) ? "0" : hasanScoreText;
        amouranthScoreText = string.IsNullOrEmpty(amouranthScoreText) ? "0" : amouranthScoreText;
        h3h3ScoreText = string.IsNullOrEmpty(h3h3ScoreText) ? "0" : h3h3ScoreText;

        // Assign scores to the respective TMP_Text variables
        destinyScoreTMP.text = destinyScoreText;
        xqcScoreTMP.text = xqcScoreText;
        hasanScoreTMP.text = hasanScoreText;
        amouranthScoreTMP.text = amouranthScoreText;
        h3h3ScoreTMP.text = h3h3ScoreText;

        // Create an array of tuples to store streamer names and scores
        (string, string)[] scoresAndStreamers = {
            ("Destiny", destinyScoreText),
            ("XQC", xqcScoreText),
            ("Hasan", hasanScoreText),
            ("Amouranth", amouranthScoreText),
            ("H3H3", h3h3ScoreText)
        };

        // Convert score strings to integers and sort by scores
        // Use a simple loop to sort the array based on scores
        for (int i = 0; i < scoresAndStreamers.Length - 1; i++)
        {
            for (int j = i + 1; j < scoresAndStreamers.Length; j++)
            {
                float scoreI = float.Parse(scoresAndStreamers[i].Item2);
                float scoreJ = float.Parse(scoresAndStreamers[j].Item2);

                // Compare and swap if necessary
                if (scoreJ > scoreI)
                {
                    var temp = scoresAndStreamers[i];
                    scoresAndStreamers[i] = scoresAndStreamers[j];
                    scoresAndStreamers[j] = temp;
                }
            }
        }

        Debug.Log("first place: " + scoresAndStreamers[0].Item1);
        // Move streamer transforms to their respective places based on rankings
        MoveStreamerToPlace(scoresAndStreamers[0].Item1, firstPlaceTransform);
        MoveStreamerToPlace(scoresAndStreamers[1].Item1, secondPlaceTransform);
        MoveStreamerToPlace(scoresAndStreamers[2].Item1, thirdPlaceTransform);
        MoveStreamerToPlace(scoresAndStreamers[3].Item1, fourthPlaceTransform);
        MoveStreamerToPlace(scoresAndStreamers[4].Item1, fifthPlaceTransform);
    }

    private void MoveStreamerToPlace(string streamerName, Transform targetTransform)
    {
        // Get the corresponding transform based on the streamer name and move it to the target transform
        switch (streamerName)
        {
            case "Destiny":
                Debug.Log("destiny " + destinyTransform.position + "targetTransform postition: " + targetTransform.position);
                destinyTransform.position = targetTransform.position;
                
                break;
            case "XQC":
                xqcTransform.position = targetTransform.position;
                break;
            case "Hasan":
                hasanTransform.position = targetTransform.position;
                break;
            case "Amouranth":
                amouranthTransform.position = targetTransform.position;
                break;
            case "H3H3":
                ethanTransform.position = targetTransform.position;
                break;
        }
    }
}
