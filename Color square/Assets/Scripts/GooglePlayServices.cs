using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public static class GooglePlayServices
{
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
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = debug;
        PlayGamesPlatform.Activate();
    }

    public static void authenticate(Action<bool> action)
    {
        Social.localUser.Authenticate(action);
    }
}
