using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }
    private const string leaderboardId = "StreamerOfTheUniverse"; 

    public event EventHandler<OnLeaderboardPulledEventargs> OnLeaderboardPulled;
    public bool ranOnce = false;

    private Dictionary<string, string> metadataXQC = new Dictionary<string, string>() { { "creator", "XQC" } };

    public class OnLeaderboardPulledEventargs : EventArgs
    {
        public string scores;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);

        SetupEvents();

        await SignInAnonymouslyAsync();

        GetScores();
        GetPlayerScoreByMetaData();
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"PLAYERID:  {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"Access Token:  {AuthenticationService.Instance.AccessToken}");
        };
    }
    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async void GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(
            leaderboardId,
            new GetScoresOptions { IncludeMetadata = true, Limit = 1000});
        OnLeaderboardPulled?.Invoke(this, new OnLeaderboardPulledEventargs
        {
            scores = JsonConvert.SerializeObject(scoresResponse)
        });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        ranOnce = true;
    }

    public void AddPlayerScore(string playerName, float playerScore, string creatorName)
    {
        AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);

        AddScoreWithMetadata(playerScore, creatorName);
    }

    public async void AddScoreWithMetadata(float playerScore, string creatorName)
    {
        var metadata = new Dictionary<string, string>() { { "creator", creatorName } };
        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(
                leaderboardId,
                playerScore,
                new AddPlayerScoreOptions 
                { Metadata = metadata });
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }


    public async void GetPlayerScoreByMetaData()
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(
                leaderboardId,
                new GetPlayerScoreOptions { IncludeMetadata = true });
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    [System.Serializable]
    public class ScoreData
    {
        public string playerId;
        public string playerName;
        public int rank;
        public float score;
    }
}
