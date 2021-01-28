using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FantomLib;

public class BackgroundController : MonoBehaviour
{
    public int currentBackground;
    public ProductAddition currentBackgroundAddition;
    [Space(15)]
    [SerializeField] private float startBackgroundSpeed;
    [SerializeField] private float backgroundStopStartTime;
    [Space(15)]    
    [SerializeField] private BlotSpawner blotSpawner;
    [SerializeField] private SpriteRenderer[] backgrounds;   
    [Space(15)]
    [SerializeField] private PanesController panesController;
    [SerializeField] private ButtonsController buttonsController;
    [SerializeField] private TrianglesController trianglesController;
    [SerializeField] private ShopController shopController;

    [HideInInspector] public bool isPlay = false;
    [HideInInspector] public bool isPause = false;

    public static float backgroundSpeed = 0;

    private ProductAddition additionBeforePreview;
    private Sprite spriteBeforePreview;

    void Start()
    {
        backgrounds = GetComponentsInChildren<SpriteRenderer>();

        backgroundSpeed = 0;

        if (PlayerPrefs.HasKey("Background")) currentBackground = PlayerPrefs.GetInt("Background");
        else currentBackground = 0;

        if (PlayerPrefs.HasKey("BackgroundAddition")) currentBackgroundAddition = (ProductAddition)PlayerPrefs.GetInt("BackgroundAddition");
        else currentBackgroundAddition = 0;

        StartCoroutine(loadBackgroundAfterInit(3));
    }

    private IEnumerator loadBackgroundAfterInit(float time)
    {
        float currentTime = 0;

        if (currentBackground == 3)
        {
            while (currentTime < time)
            {
                if (shopController.realMoneyProductsIsInit) break;
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            if (currentTime >= time) AndroidController.showToast("Timeout");
        }

        ProductController findedBackground = shopController.findProduct(currentBackground, ShopProductType.background);

        if (findedBackground.isBought) setBackgroundSprite(findedBackground.product);
        else setCurrentBackground(0, shopController.findProduct(0, ShopProductType.background).product, ProductAddition.blot, false);
    }

    public void setCurrentBackground(int id, Sprite backgroundSprite, ProductAddition addition, bool playAnim)
    {
        if (currentBackground != id)
        {
            currentBackground = id;
            currentBackgroundAddition = addition;

            if (playAnim) StartCoroutine(setCurrentBackgroundCoroutine(backgroundSprite));   
            else setBackgroundSprite(backgroundSprite);

            PlayerPrefs.SetInt("Background", currentBackground);
            PlayerPrefs.SetInt("BackgroundAddition", (int)currentBackgroundAddition);
        }
    }

    public void previewBackground(Sprite backgroundSprite, int id, ProductAddition addition)
    {
        StartCoroutine(previewBackgroundCoroutine(backgroundSprite, id, addition));
    }

    public void stopPreviewBackground()
    {
        StartCoroutine(stopPreviewBackgroundCoroutine());
    }

    public void play()
    {
        if (isPlay && isPause)
        {
            playAddition();
            StartCoroutine(startBackground());
        }

        if (!isPlay) {
            isPlay = true;
            playAddition();
            StartCoroutine(startBackground());
        }            
        isPause = false;
    }

    private void playAddition()
    {
        switch (currentBackgroundAddition)
        {
            case ProductAddition.blot: blotSpawner.play(); break;
            case ProductAddition.triangles: trianglesController.play(); break;
        }        
    }

    private void stopAddition()
    {
        switch (currentBackgroundAddition)
        {
            case ProductAddition.blot: blotSpawner.stop(); break;
            case ProductAddition.triangles: trianglesController.stop(); break;
        }
    }

    private void pauseAddition()
    {
        switch (currentBackgroundAddition)
        {
            case ProductAddition.blot: blotSpawner.pause(); break;
            case ProductAddition.triangles: trianglesController.pause(); break;
        }
    }

    public void stop()
    {
        if (isPlay)
        {
            isPlay = false;
            if (!isPause)
            {
                StartCoroutine(stopBackground());
                stopAddition();
            }
        }
    }

    public void pause()
    {
        if (isPlay)
        {
            isPause = true;
            StartCoroutine(stopBackground());
            pauseAddition();
        }
    }

    private void setBackgroundSprite(Sprite sprite)
    {
        for (int i = 0; i < backgrounds.Length - 1; i++) backgrounds[i].sprite = sprite;

        if (currentBackground == 1) Camera.main.backgroundColor = new Color(46f / 255f, 46f / 255f, 46f / 255f);
        else Camera.main.backgroundColor = Color.black;
    }

    IEnumerator setCurrentBackgroundCoroutine(Sprite backgroundSprite)
    {
        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));
        setBackgroundSprite(backgroundSprite);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(panesController.transitionPaneCoroutine(false));
    }

    IEnumerator stopBackground()
    {
        float t = 0;
        while (t < backgroundStopStartTime)
        {
            backgroundSpeed = Mathf.Lerp(startBackgroundSpeed, 0, t / backgroundStopStartTime);
            t += Time.deltaTime;
            yield return null;
        }
        backgroundSpeed = 0;
    }

    IEnumerator startBackground()
    {
        float t = 0;
        while (t < backgroundStopStartTime)
        {
            backgroundSpeed = Mathf.Lerp(0, startBackgroundSpeed, t / backgroundStopStartTime);
            t += Time.deltaTime;
            yield return null;
        }
        backgroundSpeed = startBackgroundSpeed;
    }

    IEnumerator previewBackgroundCoroutine(Sprite backgroundSprite, int id, ProductAddition addition)
    {
        additionBeforePreview = currentBackgroundAddition;
        spriteBeforePreview = backgrounds[0].sprite;

        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));
        setBackgroundSprite(backgroundSprite);

        if (id == 1) Camera.main.backgroundColor = new Color(46f / 255f, 46f / 255f, 46f / 255f);
        else Camera.main.backgroundColor = Color.black;

        currentBackgroundAddition = addition;
        panesController.setActiveShopPane(false);
        playAddition();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(panesController.transitionPaneCoroutine(false));

        yield return new WaitUntil(() => Input.touchCount > 0 || Input.GetMouseButtonDown(0));

        stopPreviewBackground();
    }

    IEnumerator stopPreviewBackgroundCoroutine()
    {
        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));
        stopAddition();
        panesController.setActiveShopPane(true);
        currentBackgroundAddition = additionBeforePreview;
        setBackgroundSprite(spriteBeforePreview);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(panesController.transitionPaneCoroutine(false));
    }
}
