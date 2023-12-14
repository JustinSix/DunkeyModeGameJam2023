using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class DisplayLeaderboard : MonoBehaviour
{
    [SerializeField] PodiumPeopleManager podiumPeopleManager;
    [SerializeField] TMP_Text top10Text;
    [SerializeField] TMP_Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        LeaderboardManager.Instance.OnLeaderboardPulled += Instance_OnLeaderboardPulled;

        if (LeaderboardManager.Instance.ranOnce)
        {
            LeaderboardManager.Instance.GetScores();
        }
    }

    private void Instance_OnLeaderboardPulled(object sender, LeaderboardManager.OnLeaderboardPulledEventargs e)
    {
        DisplayScores(e.scores);
    }

    private void DisplayScores(string scores)
    {
        // Deserialize JSON string into C# object
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(scores);

        // Dictionary to store combined scores for each creator
        Dictionary<string, double> combinedScoresByCreator = new Dictionary<string, double>();

        // Iterate through the list of PlayerInfo objects
        foreach (PlayerInfo playerInfo in playerData.results)
        {
            // Remove '#' and any numbers following it from the player name
            string cleanedPlayerName = Regex.Replace(playerInfo.playerName, @"#\d*", "");

            // Extract the creator from metadata
            string creator = GetCreatorFromMetadata(playerInfo.metadata);

            // If the creator is not in the dictionary, add it with the current score
            if (!combinedScoresByCreator.ContainsKey(creator))
            {
                combinedScoresByCreator.Add(creator, playerInfo.score);
            }
            // If the creator is already in the dictionary, accumulate the score
            else
            {
                combinedScoresByCreator[creator] += playerInfo.score;
            }
        }

        // Sort creators by their combined scores in descending order
        var sortedCreators = combinedScoresByCreator.OrderByDescending(kv => kv.Value);

        // Assign scores to separate string variables
        string xqcScore = combinedScoresByCreator.ContainsKey("XQC") ? combinedScoresByCreator["XQC"].ToString("F") : "0";
        string destinyScore = combinedScoresByCreator.ContainsKey("Destiny") ? combinedScoresByCreator["Destiny"].ToString("F") : "0";
        string h3h3Score = combinedScoresByCreator.ContainsKey("H3H3") ? combinedScoresByCreator["H3H3"].ToString("F") : "0";
        string hasanScore = combinedScoresByCreator.ContainsKey("Hasan") ? combinedScoresByCreator["Hasan"].ToString("F") : "0";
        string amouranthScore = combinedScoresByCreator.ContainsKey("Amouranth") ? combinedScoresByCreator["Amouranth"].ToString("F") : "0";
        //string h3h3Score = combinedScoresByCreator.ContainsKey("H3H3") ? combinedScoresByCreator["H3H3"].ToString("F") : "0";

        //string destinyScoreText, string xqcScoreText, string hasanScoreText, string amouranthScoreText, string h3h3Score
        podiumPeopleManager.TakeScoresAndOrganizeStreamersAndScores(destinyScore, xqcScore, hasanScore, amouranthScore, h3h3Score);
        // Display the combined scores
        foreach (var kvp in sortedCreators)
        {
            top10Text.text += $"\n{kvp.Key}: {kvp.Value.ToString("F")} combined score!";
        }

        top10Text.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);
    }

    private string GetCreatorFromMetadata(string metadata)
    {
        // Deserialize metadata JSON string into C# object
        Metadata metadataObject = JsonUtility.FromJson<Metadata>(metadata);

        // Return the creator from metadata
        return metadataObject.creator;
    }

    private void OnDestroy()
    {
        LeaderboardManager.Instance.OnLeaderboardPulled -= Instance_OnLeaderboardPulled;
    }

    [Serializable]
    public class Metadata
    {
        public string creator;
    }

    [Serializable]
    public class PlayerData
    {
        public int limit;
        public int total;
        public List<PlayerInfo> results;
    }

    [Serializable]
    public class PlayerInfo
    {
        public string playerId;
        public string playerName;
        public int rank;
        public double score;
        public string metadata;
    }
}
