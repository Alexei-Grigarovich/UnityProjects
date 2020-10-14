using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductController : MonoBehaviour
{
    private Text priceText;
    private GameObject money;

    public Button buyButton;
    public Button selectButton;
    public Button previewButton;

    public int id;
    public float price;
    [Space(15)]
    public ShopProductType type;
    public ProductAddition addition;
    [Space(15)]
    public bool isAvailable;
    public bool isBought;
    [Space(15)]
    public bool isForRealMoney;
    public bool isConsumable;
    public int purchaseId;
    public string purchaseStringId;
    [Space(15)]
    public Sprite product;
    public Sprite skinLayer;

    public void initialize()
    {
        priceText = GetComponentInChildren<Text>();
        money = GetComponentInChildren<Toggle>(true).gameObject;
        selectButton = GetComponent<Button>();
        buyButton = GetComponentsInChildren<Button>(true)[1];
        previewButton = GetComponentsInChildren<Button>(true)[2];

        if (isBought) setBought();
        
        if (isForRealMoney)
        {
            //checkBuyState();
        }
        else
        {
            if (PlayerPrefs.HasKey("BoughtProduct_" + id + "_Type_" + (int)type)) isBought = true;
            else isBought = false;               
        }

        updateBuyButton();
    }

    public void updateBuyButton()
    {
        if (isForRealMoney)
        {
            if (isBought)
            {
                buyButton.interactable = false;
                money.SetActive(false);
                priceText.rectTransform.offsetMax = new Vector2(-priceText.rectTransform.offsetMin.x, -priceText.rectTransform.offsetMin.y - 5);
                priceText.alignment = TextAnchor.MiddleCenter;
                priceText.text = LanguageController.langStrings.boughtText;               
            }
            else
            {
                money.SetActive(false);
                priceText.text = price.ToString() + " $";
                priceText.rectTransform.offsetMax = new Vector2(-priceText.rectTransform.offsetMin.x, -priceText.rectTransform.offsetMin.y);
                priceText.alignment = TextAnchor.MiddleCenter;
            }
        }
        else
        {
            if (isBought)
            {
                buyButton.interactable = false;
                money.SetActive(false);
                priceText.rectTransform.offsetMax = new Vector2(-priceText.rectTransform.offsetMin.x, -priceText.rectTransform.offsetMin.y - 5);
                priceText.alignment = TextAnchor.MiddleCenter;
                priceText.text = LanguageController.langStrings.boughtText;
            }
            else
            {
                money.SetActive(true);
                priceText.text = price.ToString();
            }
        }
    }

    public void updateButtonText()
    {
        if (isBought)
        {
            priceText.text = LanguageController.langStrings.boughtText;
        }
    }

    public void setBought()
    {
        isBought = true;
        PlayerPrefs.SetInt("BoughtProduct_" + id + "_Type_" + (int)type, 1);
        updateBuyButton();
    }

    public void checkBuyState()
    {
        if (PurchaseManager.CheckBuyState(purchaseStringId)) isBought = true;
        else isBought = false;

        updateBuyButton();
    }
}
