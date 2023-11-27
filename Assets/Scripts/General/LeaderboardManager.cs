using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using System;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }
    private const string leaderboardId = "SkibidiOPmanLeaderboard"; //6opmanl6eaderboard6

    public event EventHandler<OnLeaderboardPulledEventargs> OnLeaderboardPulled;
    public bool ranOnce = false;
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
            .GetScoresAsync(leaderboardId);
        OnLeaderboardPulled?.Invoke(this, new OnLeaderboardPulledEventargs
        {
            scores = JsonConvert.SerializeObject(scoresResponse)
        });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        ranOnce = true;
    }




    public void AddPlayerScore(string playerName, float playerTime)
    {
        AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        //await UpdatePlayerName(playerName);
        AddScoreWithMetadata(playerTime);
    }
    async Task UpdatePlayerName(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
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
    public async void AddScoreWithMetadata(float playerTime)
    {

        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(
                leaderboardId,
                playerTime
                );
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
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
