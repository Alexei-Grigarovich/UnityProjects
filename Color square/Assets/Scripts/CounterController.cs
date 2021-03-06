﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    [SerializeField] private Text counterText;
    [Space(15)]
    [SerializeField] private GameObject newRecordText;
    [SerializeField] private GameObject teachObject;

    [HideInInspector] public float counter;

    private int maxRecord;   
    private bool newRecordIsShowed;    

    void Start()
    {
        if (!PlayerPrefs.HasKey("MaxRecord")) PlayerPrefs.SetInt("MaxRecord", 0);
        else maxRecord = PlayerPrefs.GetInt("MaxRecord");       

        newRecordIsShowed = false;
        counter = 0;

        saveRecordInScoreboard();
    }  

    public void setMaxRecord()
    {
        if ((int)counter > maxRecord)
        {
            maxRecord = (int)counter;
            saveMaxRecord();
        }
    }

    private void saveMaxRecord()
    {
        PlayerPrefs.SetInt("MaxRecord", maxRecord);
        saveRecordInScoreboard();
    }

    public void saveRecordInScoreboard()
    {
        AndroidController.setScoreboard(GPGSIds.leaderboard_best_players, maxRecord, (bool success) =>
        {
            Debug.Log("Record is add in scoreboard!");
        });
    }

    public int getMaxRecord()
    {
        return maxRecord;
    }

    private IEnumerator newRecordShowCoroutine()
    {
        newRecordText.SetActive(true);

        newRecordText.GetComponent<Animator>().Play("Show");
        yield return new WaitForSeconds(newRecordText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        newRecordText.SetActive(false);
    }

    void Update()
    {
        counterText.text = ((int)counter).ToString();

        if (MainSquareController.getIsPlay())
        {
            if (!newRecordIsShowed && counter > maxRecord && teachObject == null)
            {
                newRecordIsShowed = true;
                StartCoroutine(newRecordShowCoroutine());
            }

            counter += PlatformSpawner.getPlatformSpeed() * Time.deltaTime;

            if (counter > 500)
            {
                AndroidController.setAchievement(GPGSIds.achievement_experienced, 100, (success) => { });
                AndroidController.setAchievement(GPGSIds.achievement_conqueror_of_the_top_of_the_rating, 0, (success) => { });
            }
            if (counter > 2000)
            {
                AndroidController.setAchievement(GPGSIds.achievement_conqueror_of_the_top_of_the_rating, 100, (success) => { });
                AndroidController.setAchievement(GPGSIds.achievement_korea, 0, (success) => { });
            }
            if (counter > 10000) AndroidController.setAchievement(GPGSIds.achievement_korea, 100, (success) => { });
        }
    }
}
