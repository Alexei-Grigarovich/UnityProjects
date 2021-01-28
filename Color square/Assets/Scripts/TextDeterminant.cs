using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDeterminant : MonoBehaviour
{
    [SerializeField] private Text newRecordText;
    [SerializeField] private Text ratingPaneText;
    [SerializeField] private Text pausePaneText;
    [SerializeField] private Text backgroundsButtonText;
    [SerializeField] private Text skinsButtonText;
    [SerializeField] private Text donateButtonText;
    [SerializeField] private Text dnt0Text;
    [SerializeField] private Text dnt1Text;
    [SerializeField] private Text rewardMoneyText;
    [SerializeField] private Text x2MoneyText;

    void Start()
    {
        setAllTexts();
    }

    public void setAllTexts()
    {
        newRecordText.text = LanguageController.langStrings.newRecordText;
        ratingPaneText.text = LanguageController.langStrings.rateGameText;
        pausePaneText.text = LanguageController.langStrings.pausePaneText;
        backgroundsButtonText.text = LanguageController.langStrings.backgroundsButtonText;
        skinsButtonText.text = LanguageController.langStrings.skinsButtonText;
        donateButtonText.text = LanguageController.langStrings.donatesButtonText;
        dnt0Text.text = LanguageController.langStrings.backgroundsButtonText + "\n&\n" + LanguageController.langStrings.skinsButtonText;
        dnt1Text.text = LanguageController.langStrings.withoutAdsText + "\n&\n+1 " + LanguageController.langStrings.plusLiveText;
        rewardMoneyText.text = LanguageController.langStrings.freeText;
        x2MoneyText.text = LanguageController.langStrings.freeText;
    }
}
