using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public enum ShopProductType
{
    background,
    skin,
    donate
}
public enum ProductAddition
{
    blot,
    triangles,
    none,
    detector
}

public class ShopController : MonoBehaviour
{
    public static ShopController instantiate;

    [SerializeField] private Text moneyText;
    [Space(15)]
    [SerializeField] private BackgroundController backgroundController;
    [SerializeField] private SkinController skinController;
    [SerializeField] private PurchaseManager purchaseManager;
    [SerializeField] private MoneyController moneyController;
    [SerializeField] private ButtonsController buttonsController;
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private ProductController[] products;

    private ProductController lastProduct;

    public bool realMoneyProductsIsInit;

    void Start()
    {
        instantiate = this;

        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;

        realMoneyProductsIsInit = false;
        initializeProducts();
    }

    #region OnPurchase

    private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    {
        
    }

    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == "noads_")
        {
            if(lastProduct != null) lastProduct.setBought();           
            buttonsController.noAdsButton.interactable = false;
        }
        else if (args.purchasedProduct.definition.id == "subwoofer_triangles")
        {
            setBoughtPairProducts(lastProduct);
            if (lastProduct != null) lastProduct.setBought();
        }
        else
        {
            if(lastProduct != null) lastProduct.setBought();
        }
    }

    #endregion

    #region find 

    public ProductController findProduct(int id, ShopProductType productType)
    {
        ProductController findProduct = null;

        foreach (ProductController product in products)
        {
            if (product.id == id && product.type == productType)
            {
                findProduct = product;
                break;
            }
        }

        return findProduct;
    }

    ProductController findPurchaseProduct(int purchaseId, ShopProductType productType)
    {
        ProductController findProduct = null;

        foreach (ProductController product in products)
        {
            if (product.purchaseId == purchaseId && product.type == productType)
            {
                findProduct = product;
                break;
            }
        }

        return findProduct;
    }

    #endregion

    #region pair products

    public void setBoughtPairProducts(ProductController product)
    {
        if (product.purchaseStringId == "subwoofer_triangles")
        {
            findProduct(3, ShopProductType.background).setBought();
            findProduct(1, ShopProductType.skin).setBought();
        }
    }

    #endregion

    void initializeProducts()
    {
        foreach (ProductController product in products)
        {
            product.initialize();

            product.selectButton.onClick.AddListener(() => audioController.playButtonSound(1));
            product.selectButton.onClick.AddListener(() => selectProduct(product));

            product.buyButton.onClick.AddListener(() => buyProduct(product));
            product.buyButton.onClick.AddListener(() => audioController.playButtonSound(1));

            product.previewButton.onClick.AddListener(() => previewProduct(product));
            product.previewButton.onClick.AddListener(() => audioController.playButtonSound(1));
        }

        StartCoroutine(checkBuyStatesOfProducts());
    }

    public void updateProductButtonsText()
    {
        foreach (ProductController product in products) product.updateButtonText();
    }

    public void buyProduct(ProductController product)
    {       
        if (product.isAvailable && !product.isBought)
        {
            lastProduct = product;

            if (product.isForRealMoney)
            {
                if (product.isConsumable) purchaseManager.BuyConsumable(product.purchaseId);
                else purchaseManager.BuyNonConsumable(product.purchaseId);
            }
            else
            {
                if (moneyController.money >= product.price)
                {
                    moneyController.addToMoney(-(int)product.price);
                    moneyController.saveMoney();
                    product.setBought();
                    if (product.type == ShopProductType.skin)
                    {
                        AndroidController.setAchievement(GPGSIds.achievement_fashion_is_my_profession, 100, (success) => { });
                        AndroidController.setAchievement(GPGSIds.achievement_shopping_king, 20, (success) => { });
                    }
                    if(product.type == ShopProductType.background) AndroidController.setAchievement(GPGSIds.achievement_glue_wallpaper, 100, (success) => { });
                } else
                {
                    StartCoroutine(buyButtonNoMoney(product));
                }
            }

            updateMoneyText();
        }
    }

    public void selectProduct(ProductController product)
    {
        if (product.isAvailable && product.isBought)
        {
            switch (product.type)
            {
                case ShopProductType.background:
                    backgroundController.setCurrentBackground(product.id, product.product, product.addition, true);
                    break;
                case ShopProductType.skin:
                    skinController.setCurrentSkin(product.id, product.product, product.skinLayer, product.addition);
                    break;
            }
        }
    }

    public void previewProduct(ProductController product)
    {
        if (product.isAvailable)
        {
            switch (product.type)
            {
                case ShopProductType.background:
                    backgroundController.previewBackground(product.product, product.id, product.addition);
                    break;
                case ShopProductType.skin:
                    skinController.previewSkin(product.product, product.skinLayer, product.addition);
                    break;
            }
        }
    }

    public void updateMoneyText()
    {
        moneyText.text = moneyController.money.ToString();
    }

    private IEnumerator buyButtonNoMoney(ProductController product)
    {
        product.buyButton.GetComponent<Animator>().SetBool("isNoMoney", true);
        product.buyButton.interactable = false;

        yield return null;
        yield return new WaitForSeconds(product.buyButton.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);

        product.buyButton.GetComponent<Animator>().SetBool("isNoMoney", false);
        product.buyButton.interactable = true;
    }

    private IEnumerator checkBuyStatesOfProducts()
    {
        yield return new WaitUntil(() => PurchaseManager.IsInitialized());

        foreach (ProductController product in products)
        {
            if (product.isForRealMoney) product.checkBuyState();
        }
        realMoneyProductsIsInit = true;
    }
}
