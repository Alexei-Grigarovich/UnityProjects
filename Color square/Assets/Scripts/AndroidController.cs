using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    [SerializeField] private float maxTimeToDoubleTap;
    [SerializeField] private string[] achievementsIDs;

    public static string scoreboardId = "CgkIhaXbp9AHEAIQAg";

    private float startTapTime;

    private bool isDoubleTap = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        GooglePlayServices.initialize(false);
        GooglePlayServices.authenticate((bool success) =>
        {
            setAchievement(achievementsIDs[0], 100f, (bool successGetAchieve) => 
            {
                Debug.Log("Welcome!");
            });
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
        Social.ReportProgress(id, progress, action);
    }

    public static void setScoreboard(string id, long score, System.Action<bool> action)
    {
        Social.ReportScore(score, id, action);
    }
}
