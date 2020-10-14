using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private MainSquareController mainSquareController;
    [SerializeField] private BackgroundController backgroundController;
    [SerializeField] private SkinController skinController;
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private GameObject pausePane;
    [SerializeField] private GameObject fillingLine;
    [SerializeField] private GameObject counter;
    [Space(15)]
    [Header("Buttons")]
    public Button playButton;
    public Button pauseButton;
    public Button soundButton;
    public Button languageButton;
    public Button settingsButton;
    public Button noAdsButton;
    public Button againButton;
    public Button adButton;
    public Button menuButton;
    public Button shopButton;
    public Button achievementsButton;
    public Button scoreboardButton;
    [Header("Shop Pane Buttons")]
    public Button backgroundsButton;
    public Button skinsButton;
    public Button donatesButton;
    public Button closeShopPaneButton;
    [Space(15)]
    [SerializeField] private Sprite[] soundButtonSprites;
    [SerializeField] private Sprite russianButtonSprite;
    [SerializeField] private Sprite usaButtonSprite;
    [Space(15)]
    [SerializeField] Text continueCounter;
    [Space(15)]
    [SerializeField] private int countGamesForOneAd;    
    [SerializeField] private int timeToContinue;   

    private LanguageController languageController;
    private TextDeterminant textDeterminant;
    private PanesController panesController;
    private ShopController shopController;
    private MoneyController moneyController;

    private Scene scene;
    private int timeLeft;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        panesController = GetComponent<PanesController>();

        languageController = GetComponent<LanguageController>();
        textDeterminant = GetComponent<TextDeterminant>();
        shopController = GetComponent<ShopController>();
        moneyController = GetComponent<MoneyController>();

        languageButton.image.sprite = PlayerPrefs.GetString("Language") == LanguageController.ru ? russianButtonSprite : usaButtonSprite;

        if (PlayerPrefs.HasKey("startGameAwake"))
        {
            if (PlayerPrefs.GetInt("startGameAwake") == 1)
            {
                PlayerPrefs.SetInt("startGameAwake", 0);
                playButtonAct(true);
            }
        }
        else PlayerPrefs.SetInt("startGameAwake", 0);

        if (PlayerPrefs.HasKey("sound"))
        {
            soundButton.image.sprite = soundButtonSprites[PlayerPrefs.GetInt("sound")];
            audioController.mutingAllAudio(PlayerPrefs.GetInt("sound") == 1 ? false : true);
        }
        else PlayerPrefs.SetInt("sound", 1);   
    }

    public void menuButtonAct()
    {
        TimeCounter.saveTimeInGame();
        StartCoroutine(menuButtonCoroutine());
    }

    public void againButtonAct()
    {
        menuButtonAct();
        PlayerPrefs.SetInt("startGameAwake", 1);        
    }   

    public void playButtonAct(bool isStartAwake)
    {
        panesController.adIsWasUsed = false;
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        noAdsButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        scoreboardButton.gameObject.SetActive(false);
        achievementsButton.gameObject.SetActive(false);

        StartCoroutine(playButtonCoroutine(isStartAwake));
    }

    public void adButtonAct()
    {
        StopCoroutine(adButtonCoroutine());
        StartCoroutine(adButtonCoroutine());
    }

    public void settingsButtonAct()
    {
        playButton.gameObject.SetActive(false);
        panesController.showSettingsPane();
    }

    public void soundButtonAct()
    {
        PlayerPrefs.SetInt("sound", PlayerPrefs.GetInt("sound") == 1 ? 0 : 1);

        soundButton.image.sprite = soundButtonSprites[PlayerPrefs.GetInt("sound")];
        audioController.mutingAllAudio(PlayerPrefs.GetInt("sound") == 1 ? false : true);
    }

    public void pauseButtonAct()
    {
        MainSquareController.setIsPlay(!MainSquareController.getIsPlay());
        pausePane.SetActive(!MainSquareController.getIsPlay());

        if (!MainSquareController.getIsPlay()) audioController.pauseMusic();
        else audioController.playMusic();

        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    public void languageButtonAct()
    {
        languageController.changeLanguage();
        languageController.loadLang();

        languageButton.image.sprite = PlayerPrefs.GetString("Language") == LanguageController.ru ? russianButtonSprite : usaButtonSprite;

        textDeterminant.setAllTexts();
        shopController.updateProductButtonsText();
    }

    public void instagramButtonAct()
    {
        Application.OpenURL("https://www.instagram.com/gamestepforwardinc/");

        if (!PlayerPrefs.HasKey("InstagramGift")) {           
            PlayerPrefs.SetInt("InstagramGift", 1);
            moneyController.addToMoney(10);
            moneyController.saveMoney();
        }
    }

    public void mailButtonAct()
    {
        string email = "gamestepforwardinc@gmail.com";
        string subject = UnityWebRequest.EscapeURL(LanguageController.langStrings.coolGame).Replace("+", "%20");
        string body = UnityWebRequest.EscapeURL("").Replace("+", "%20");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public void scoreboardButtonAct()
    {
        Social.ShowLeaderboardUI();
    }

    public void achievementsButtonAct()
    {
        Social.ShowAchievementsUI();
    }

    public void shopButtonAct()
    {
        TimeCounter.isMenu = false;
        playButton.gameObject.SetActive(false);
        noAdsButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        achievementsButton.gameObject.SetActive(false);
        scoreboardButton.gameObject.SetActive(false);

        shopController.updateMoneyText();

        panesController.showShopPane();
        invokeOnClickShopButton(0);
    }

    public void invokeOnClickShopButton(int num)
    {
        switch (num)
        {
            case 0: backgroundsButton.onClick.Invoke();  break;
            case 1: skinsButton.onClick.Invoke(); break;
            case 2: donatesButton.onClick.Invoke(); break;
        }
    }

    public void backgroundsButtonAct()
    {
        backgroundsButton.GetComponent<Animator>().SetBool("isHide", false);
        skinsButton.GetComponent<Animator>().SetBool("isHide", true);
        donatesButton.GetComponent<Animator>().SetBool("isHide", true);
    }

    public void skinsButtonAct()
    {
        skinsButton.GetComponent<Animator>().SetBool("isHide", false);
        backgroundsButton.GetComponent<Animator>().SetBool("isHide", true);
        donatesButton.GetComponent<Animator>().SetBool("isHide", true);
    }

    public void donatesButtonAct()
    {
        donatesButton.GetComponent<Animator>().SetBool("isHide", false);
        backgroundsButton.GetComponent<Animator>().SetBool("isHide", true);
        skinsButton.GetComponent<Animator>().SetBool("isHide", true);
    }

    public void closeShopPaneButtonAct()
    {        
        panesController.hideShopPane();
    }

    public void ratingPaneAct()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.gamestepforward.colorsquare");
    }

    private IEnumerator playButtonCoroutine(bool isStartAwake)
    {
        if (isStartAwake) yield return new WaitForFixedUpdate();
        else yield return StartCoroutine(panesController.transitionPaneCoroutine(true));

        counter.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        fillingLine.SetActive(true);

        TimeCounter.isMenu = false;
        CameraFollowing.setSpeedCameraFollowing(5);

        TimeCounter.sessionGamesCounter++;
        if (TimeCounter.sessionGamesCounter % countGamesForOneAd == 0)
        {
            if (AdController.showVideoAd() >= 0) yield return new WaitUntil(() => AdController.lastAdIsFinished());
        }

        mainSquareController.setRandomSquareRotate();
        mainSquareController.setSquareStateFromRotate();
        mainSquareController.setColoredSquare(true);

        mainSquareController.startPlatform.GetComponent<PlatformState>().setPlatformState(mainSquareController.squareState);

        mainSquareController.rideParticlesMain.startColor = mainSquareController.colors[mainSquareController.squareState];
        mainSquareController.rideParticles.Play();

        audioController.changeSnapshot(MusicSnapshots.game, 2);
        TimeCounter.addOneGamesCounter();       

        MainSquareController.setIsPlay(true);
        backgroundController.play();
        skinController.play();

        TimeCounter.saveTimeInGame();

        StartCoroutine(panesController.transitionPaneCoroutine(false));    
    }

    private IEnumerator menuButtonCoroutine()
    {
        yield return StartCoroutine(panesController.transitionPaneCoroutine(true));

        SceneManager.LoadScene(scene.name);
    }

    private IEnumerator adButtonCoroutine()
    {       
        panesController.stopDeadPaneCoroutine();

        if (AdController.showRewardedVideoAd() >= 0) yield return new WaitUntil(() => AdController.lastRewAdIsFinished());         

        panesController.adIsWasUsed = true;           

        mainSquareController.startPlatform.GetComponent<PlatformState>().setPlatformState(mainSquareController.squareState);
        panesController.hideDeadPane();

        yield return new WaitForSeconds(panesController.getDeadPaneAnimator().GetCurrentAnimatorStateInfo(0).length);

        audioController.changeSnapshot(MusicSnapshots.game, timeToContinue + 1);

        timeLeft = timeToContinue;
        continueCounter.gameObject.SetActive(true);                      
        while (timeLeft > 0)
        {
            continueCounter.text = timeLeft.ToString();
            continueCounter.GetComponent<Animator>().Play("hide");
            timeLeft--;
            yield return new WaitForSeconds(1);
        }
        continueCounter.gameObject.SetActive(false);

        MainSquareController.setIsPlay(true);

        backgroundController.play();
        skinController.play();

        pauseButton.gameObject.SetActive(true);

        mainSquareController.rideParticlesMain.startColor = mainSquareController.colors[mainSquareController.squareState];
        mainSquareController.rideParticles.Play();    
    }   
}