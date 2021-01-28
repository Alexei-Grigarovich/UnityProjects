using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoneyController : MonoBehaviour
{
    public int money;
    public int moneyEarned;

    private int totalMoney;

    void Start()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else PlayerPrefs.SetInt("Money", 0);

        if (PlayerPrefs.HasKey("TotalMoney"))
        {
            totalMoney = PlayerPrefs.GetInt("TotalMoney");
        }
        else PlayerPrefs.SetInt("TotalMoney", 0);

        moneyEarned = 0;
    }

    public void saveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        Debug.Log("Money saved");
    }

    public void saveTotalMoney()
    {
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        Debug.Log("Total money saved");
    }

    public void addToMoney(int count)
    {
        money += count;
        Debug.Log("add " + count + " money");
        if (count > 0)
        {
            totalMoney += count;
            saveTotalMoney();
        }
        AndroidController.setAchievement(GPGSIds.achievement_i_need_a_more_gold, totalMoney, (success) => { });
        AndroidController.setAchievement(GPGSIds.achievement_cash_is_a_king, totalMoney, (success) => { });
    }

    public void addToMoneyEarned(int count)
    {
        moneyEarned += count;
    }
}
