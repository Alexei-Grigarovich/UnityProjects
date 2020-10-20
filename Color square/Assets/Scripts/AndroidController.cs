using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    [SerializeField] private float maxTimeToDoubleTap;

    private float startTapTime;

    private bool isDoubleTap = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        GooglePlayServices.initialize(true);
        GooglePlayServices.authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Authenticate is success");
                setAchievement(GPGSIds.achievement_welcome, 100f, (bool successGetAchieve) =>
                {
                    if (successGetAchieve) Debug.Log("Welcome!");
                });
            } else
            {
                Debug.Log("Authenticate ERROR");
            }
        });
    }

    void Update()
    {
        if (TimeCounter.isMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.time - startTapTime > maxTimeToDoubleTap) isDoubleTap = false;

                if (isDoubleTap)
                {
                    if (Time.time - startTapTime <= maxTimeToDoubleTap)
                    {
                        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) Application.Quit();
                    }
                    else
                    {
                        isDoubleTap = false;
                    }
                }
                else
                {
                    startTapTime = Time.time;
                    isDoubleTap = true;
                }
            }
        }
    }

    public static void setAchievement(string id, float progress, System.Action<bool> action)
    {
        Debug.Log("Setting achievement...");
        Social.ReportProgress(id, progress, action);
    }

    public static void setScoreboard(string id, long score, System.Action<bool> action)
    {
        Debug.Log("Setting leaderboard...");
        Social.ReportScore(score, id, action);
    }
}
