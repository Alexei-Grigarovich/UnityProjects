using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public enum ShopProductType
{
    background,
    skin
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
    [SerializeField] private Text moneyText;
    [Space(15)]
    [SerializeField] private BackgroundController backgroundController;
    [SerializeField] private SkinController skinController;
    [SerializeField] private PurchaseManager purchaseManager;
    [SerializeField] private MoneyController moneyController;
    [Space(15)]
    [SerializeField] private ProductController[] products;

    private ProductController lastProduct;

    void Start()
    {
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;

        initializeProducts();
    }

    #region OnPurchase

    private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    {
        
    }

    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id != "no_ads")
        {
            lastProduct.setBought();
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

    void initializeProducts()
    {
        foreach (ProductController product in products)
        {
            product.initialize();

            product.selectButton.onClick.AddListener(() => selectProduct(product));
            product.buyButton.onClick.AddListener(() => buyProduct(product));
            product.previewButton.onClick.AddListener(() => previewProduct(product));
        }
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
                    moneyController.addToMoney(-product.price);
                    product.setBought();
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
                    backgroundController.previewBackground(product.product, product.addition);
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
}
