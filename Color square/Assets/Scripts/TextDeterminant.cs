using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDeterminant : MonoBehaviour
{
    [SerializeField] private Text adButtonText;
    [SerializeField] private Text newRecordText;
    [SerializeField] private Text ratingPaneText;
    [SerializeField] private Text pausePaneText;
    [SerializeField] private Text backgroundsButtonText;
    [SerializeField] private Text skinsButtonText;
    [SerializeField] private Text donateButtonText;

    void Start()
    {
        setAllTexts();
    }

    public void setAllTexts()
    {
        adButtonText.text = LanguageController.langStrings.adButtonText;
        newRecordText.text = LanguageController.langStrings.newRecordText;
        ratingPaneText.text = LanguageController.langStrings.rateGameText;
        pausePaneText.text = LanguageController.langStrings.pausePaneText;
        backgroundsButtonText.text = LanguageController.langStrings.backgroundsButtonText;
        skinsButtonText.text = LanguageController.langStrings.skinsButtonText;
        donateButtonText.text = LanguageController.langStrings.donatesButtonText;
    }
}
