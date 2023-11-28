using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
public class DisplayLeaderboard : MonoBehaviour
{
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


    //sort through all scores
    //combine all scores with same metadata 
    //sort by total creator scores, and show 


    private void Instance_OnLeaderboardPulled(object sender, LeaderboardManager.OnLeaderboardPulledEventargs e)
    {
        DisplayScores(e.scores);
    }

    private void DisplayScores(string scores)
    {
        // Deserialize JSON string into C# object
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(scores);
        for (int i = 0; i < playerData.results.Count; i++)
        {
            top10Text.text += "\n" + ReturnFormattedString(playerData, i);
        }
        top10Text.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);
    }
    private string ReturnFormattedString(PlayerData playerData, int index)
    {
        PlayerInfo playerInfo = playerData.results[index];

        // Remove '#' and any numbers following it
        string cleanedPlayerName = Regex.Replace(playerInfo.playerName, @"#\d*", "");

        string formattedString = $"{cleanedPlayerName}: {playerInfo.score.ToString("F")} total score!";

        return formattedString;
    }
    private void OnDestroy()
    {
        LeaderboardManager.Instance.OnLeaderboardPulled -= Instance_OnLeaderboardPulled;
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
    }
}
