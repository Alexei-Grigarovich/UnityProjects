using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] PanesController panesController;
    [Space(15)]
    [SerializeField, Tooltip("In minutes")] private float timeToShowRatingPane;

    public static int sessionGamesCounter;

    private static float timeInGame = 0;

    public static bool isMenu;
    private bool ratingPaneIsShowed;

    void Start()
    {
        if (!PlayerPrefs.HasKey("RatingPaneIsShowed")) ratingPaneIsShowed = false;
        else ratingPaneIsShowed = true;

        if (PlayerPrefs.HasKey("TimeInGame")) timeInGame = PlayerPrefs.GetFloat("TimeInGame");
        else PlayerPrefs.SetFloat("TimeInGame", 0);

        if (!PlayerPrefs.HasKey("GamesCount")) PlayerPrefs.SetInt("GamesCount", 0);

        if (PlayerPrefs.GetInt("startGameAwake") != 1) isMenu = true;
    }  

    public static void addOneGamesCounter()
    {
        PlayerPrefs.SetInt("GamesCount", PlayerPrefs.GetInt("GamesCount") + 1);
    }

    public static void saveTimeInGame()
    {
        PlayerPrefs.SetFloat("TimeInGame", timeInGame);
    }

    void Update()
    {
        timeInGame += Time.deltaTime;
        if (isMenu && !ratingPaneIsShowed && timeInGame > timeToShowRatingPane * 60f)
        {
            Debug.Log("Rating Pane Show");
            ratingPaneIsShowed = true;
            PlayerPrefs.SetInt("RatingPaneIsShowed", 1);
            StartCoroutine(panesController.showRatingPaneCoroutine());
        }
    }
}
