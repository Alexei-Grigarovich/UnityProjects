using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public static class GooglePlayServices
{
    public static PlayGamesPlatform platform;

    public static bool isAuthenticated
    {
        get
        {
            if (PlayGamesPlatform.Instance != null) return PlayGamesPlatform.Instance.IsAuthenticated();
            else return false;
        }
    }

    public static void initialize(bool debug)
    {
        Debug.Log("Initializing...");
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = debug;
        platform = PlayGamesPlatform.Activate();
    }

    public static void authenticate(Action<bool> action)
    {
        Social.Active.localUser.Authenticate(action);
    }
}
