using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera previewCamera;
    [SerializeField] private Canvas interfaceCanvas;
    [Space(15)]
    [SerializeField] private ShopController shopController;
    [SerializeField] private PanesController panesController;   
    [Space(15)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space(15)]
    [SerializeField] private GameObject bassDetector;

    [HideInInspector] public bool isPlay = false;
    [HideInInspector] public bool isPause = false;

    private SpriteRenderer bassDetectorSpriteRenderer;
    private SkinBassDetectorController bassDetectorController;

    public ProductAddition currentSkinAddition;
    public int currentSkin;

    private ProductAddition additionBeforePreview;
    private Sprite skinBeforePreview;
    private Sprite skinLayerBeforePreview;

    void Start()
    {
        mainCamera.enabled = true;
        previewCamera.enabled = false;
        interfaceCanvas.worldCamera = mainCamera;

        bassDetectorSpriteRenderer = bassDetector.GetComponent<SpriteRenderer>();        
        bassDetectorController = bassDetector.GetComponent<SkinBassDetectorController>();

        if (PlayerPrefs.HasKey("Skin")) currentSkin = PlayerPrefs.GetInt("Skin");
        else currentSkin = 0;

        if (PlayerPrefs.HasKey("SkinAddition")) currentSkinAddition = (ProductAddition)PlayerPrefs.GetInt("SkinAddition");
        else currentSkinAddition = 0;

        ProductController findedSkin = shopController.findProduct(currentSkin, ShopProductType.skin);

        if (findedSkin.isBought) setSkinSprite(findedSkin.product, findedSkin.skinLayer);
        else setCurrentSkin(0, shopController.findProduct(0, ShopProductType.skin).product, null, ProductAddition.none);

        //StartCoroutine(loadSkinAfterInit());
    }

    private IEnumerator loadSkinAfterInit()
    {
        yield return new WaitUntil(() => PurchaseManager.IsInitialized());

        ProductController findedSkin = shopController.findProduct(currentSkin, ShopProductType.skin);

        if (findedSkin.isBought) setSkinSprite(findedSkin.product, findedSkin.skinLayer);
        else setCurrentSkin(0, shopController.findProduct(0, ShopProductType.skin).product, null, ProductAddition.none);
    }

    public void setCurrentSkin(int id, Sprite sprite, Sprite skinLayer, ProductAddition addition)
    {
        currentSkin = id;
        currentSkinAddition = addition;
        setSkinSprite(sprite, skinLayer);

        PlayerPrefs.SetInt("Skin", currentSkin);
        PlayerPrefs.SetInt("SkinAddition", (int)currentSkinAddition);
    }

    private void setSkinSprite(Sprite sprite, Sprite skinLayer)
    {
        spriteRenderer.sprite = sprite;
        bassDetectorSpriteRenderer.sprite = skinLayer;
    }

    public void previewSkin(Sprite sprite, Sprite skinLayer, ProductAddition addition)
    {
        StartCoroutine(previewSkinCoroutine(sprite, skinLayer, addition));
    }

    public void play()
    {
        if (isPlay && isPause)
        {
            playAddition();
        }

        if (!isPlay)
        {
            isPlay = true;
            playAddition();
        }
        isPause = false;
    }

    public void stop()
    {
        if (isPlay)
        {
            isPlay = false;
            if (!isPause)
            {
                stopAddition();
            }
        }
    }

    public void pause()
    {
        if (isPlay)
        {
            isPause = true;
            pauseAddition();
        }
    }

    private void playAddition()
    {
        switch (currentSkinAddition)
        {
            case ProductAddition.detector: bassDetectorController.play(); break;
        }
    }

    private void stopAddition()
    {
        switch (currentSkinAddition)
        {
            case ProductAddition.detector: bassDetectorController.stop(); break;
        }
    }

    private void pauseAddition()
    {
        switch (currentSkinAddition)
        {
            case ProductAddition.detector: bassDetectorController.pause(); break;
        }
    }

    private IEnumerator previewSkinCoroutine(Sprite sprite, Sprite skinLayer, ProductAddition addition)
    {
        skinBeforePreview = spriteRenderer.sprite;
        skinLayerBeforePreview = bassDetectorSpriteRenderer.sprite;
        additionBeforePreview = currentSkinAddition;

        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));

        setSkinSprite(sprite, skinLayer);
        currentSkinAddition = addition;

        panesController.setActiveShopPane(false);
        play();

        mainCamera.enabled = false;
        previewCamera.enabled = true;
        interfaceCanvas.worldCamera = previewCamera;

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(panesController.transitionPaneCoroutine(false));

        yield return new WaitUntil(() => Input.touchCount > 0 || Input.GetMouseButtonDown(0));

        StartCoroutine(stopPreviewSkinCoroutine());
    }

    private IEnumerator stopPreviewSkinCoroutine()
    {
        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));

        setSkinSprite(skinBeforePreview, skinLayerBeforePreview);
        currentSkinAddition = additionBeforePreview;

        panesController.setActiveShopPane(true);
        stop();

        mainCamera.enabled = true;
        previewCamera.enabled = false;
        interfaceCanvas.worldCamera = mainCamera;

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(panesController.transitionPaneCoroutine(false));
    }
}
