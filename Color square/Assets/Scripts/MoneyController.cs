using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoneyController : MonoBehaviour
{
    public int money;

    void Start()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else PlayerPrefs.SetInt("Money", 0);
    }

    public void saveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
    }

    public void addToMoney(int count)
    {
        money += count;
        Debug.Log("add " + count + " money");
    }
}
