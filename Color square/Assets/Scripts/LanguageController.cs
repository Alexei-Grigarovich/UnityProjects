using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    public static LangStrings langStrings;

    public static string ru = "ru_RU";
    public static string en = "en_US";

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Belarusian || Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian)
            {
                PlayerPrefs.SetString("Language", ru);
            }
            else
            {
                PlayerPrefs.SetString("Language", en);
            }
        }

        loadLang();
    }

    public void changeLanguage()
    {
        PlayerPrefs.SetString("Language", PlayerPrefs.GetString("Language") == ru ? en : ru);
    }

    public void loadLang()
    {  
        langStrings = JsonUtility.FromJson<LangStrings>(Resources.Load<TextAsset>("Languages/" + PlayerPrefs.GetString("Language")).text);
    }
}

[System.Serializable]
public class LangStrings
{
    public string gameName;
    public string coolGame;
    public string adButtonText;
    public string deadPaneCurrentCountText;
    public string deadPaneRecordText;
    public string newRecordText;
    public string rateGameText;
    public string pausePaneText;
    public string[] teachingTexts;
    public string backgroundsButtonText;
    public string skinsButtonText;
    public string donatesButtonText;
    public string boughtText;
    public string moneyEarnText;
    public string withoutAdsText;
    public string plusLiveText;
    public string freeText;
}
