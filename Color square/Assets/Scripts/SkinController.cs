using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    [SerializeField] private ShopController shopController;
    [SerializeField] private PanesController panesController;
    [Space(15)]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private ProductAddition currentSkinAddition;
    private int currentSkin;

    private ProductAddition additionBeforePreview;
    private Sprite skinBeforePreview;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerPrefs.HasKey("Skin")) currentSkin = PlayerPrefs.GetInt("Skin");
        else currentSkin = 0;

        if (PlayerPrefs.HasKey("SkinAddition")) currentSkinAddition = (ProductAddition)PlayerPrefs.GetInt("SkinAddition");
        else currentSkinAddition = 0;

        ProductController findedSkin = shopController.findProduct(currentSkin, ShopProductType.skin);

        if (findedSkin.isBought) setSkinSprite(findedSkin.product);
        else setCurrentSkin(0, shopController.findProduct(0, ShopProductType.skin).product, ProductAddition.blot); //Product Addition ---SET---
    }

    public void setCurrentSkin(int id, Sprite sprite, ProductAddition addition)
    {
        currentSkin = id;        ;
        currentSkinAddition = addition;
        setSkinSprite(sprite);
    }

    private void setSkinSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void previewSkin(Sprite sprite, ProductAddition addition)
    {
        StartCoroutine(previewSkinCoroutine(sprite, addition));
    }

    private IEnumerator previewSkinCoroutine(Sprite sprite, ProductAddition addition)
    {
        skinBeforePreview = spriteRenderer.sprite;
        additionBeforePreview = currentSkinAddition;

        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));

        setSkinSprite(sprite);
        currentSkinAddition = addition;

        panesController.setActiveShopPane(false);

        //camera zooming

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(panesController.transitionPaneCoroutine(false));

        yield return new WaitUntil(() => Input.touchCount > 0 || Input.GetMouseButtonDown(0));

        StartCoroutine(stopPreviewSkinCoroutine());
    }

    private IEnumerator stopPreviewSkinCoroutine()
    {
        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));

        setSkinSprite(skinBeforePreview);
        currentSkinAddition = additionBeforePreview;

        panesController.setActiveShopPane(true);

        //camera unzooming

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(panesController.transitionPaneCoroutine(false));
    }
}
