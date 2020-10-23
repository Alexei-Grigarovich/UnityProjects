using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FantomLib;

public class AndroidController : MonoBehaviour
{
    private static ToastController toastController;
    private static VibratorController vibratorController;

    [SerializeField] private float maxTimeToDoubleTap;

    private float startTapTime;

    private bool isDoubleTap = false;        

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (toastController == null) toastController = new ToastController();
        if (vibratorController == null) vibratorController = new VibratorController();

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

    public static void showToast(string text)
    {
        toastController.Show(text);
    }

    public static void showToast(string text, bool longDuration)
    {
        toastController.Show(text, longDuration);
    }

    public static void vibrate(long duration)
    {
        vibratorController.Duration = duration;
        vibratorController.StartVibrator();
    }
}
