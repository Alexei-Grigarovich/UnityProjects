using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanesController : MonoBehaviour
{
    private MoneyController moneyController;
    private ButtonsController buttonsController;
    private CounterController counterController;

    [SerializeField] private BackgroundController backgroundController;
    [SerializeField] private SkinController skinController;
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private GameObject counter;
    [SerializeField] private GameObject deadPane;    
    [SerializeField] private GameObject settingsPane;
    [SerializeField] private GameObject shopPane;
    [SerializeField] private GameObject ratingPane;
    [SerializeField] private GameObject ratingPaneChildren;
    [Space(15)]
    [SerializeField] private Image deadPaneImage;
    [SerializeField] private Image transitionPaneImage;
    [Space(15)]
    [SerializeField] private Text counterStringText;
    [SerializeField] private Text counterText;
    [SerializeField] private Text recordStringText;
    [SerializeField] private Text recordText;
    [SerializeField] private Text moneyEarnedStringText;
    [SerializeField] private Text moneyEarnedText;
    public Text moneyX2Text;
    [Space(15)]
    [SerializeField] private float timeTransitionPane;
   
    private bool transitionIsEnd = false;

    [HideInInspector] public bool continueGameIsUsed;

    void Start()
    {
        buttonsController = GetComponent<ButtonsController>();
        moneyController = GetComponent<MoneyController>();
        counterController = GetComponent<CounterController>();

        deadPane.SetActive(false);
        settingsPane.SetActive(false);

        StartCoroutine(transitionPaneCoroutineAfterInit(false));
    }

    public void showDeadPane()
    {        
        buttonsController.pauseButton.gameObject.SetActive(false);

        backgroundController.pause();
        skinController.pause();
        audioController.changeSnapshot(MusicSnapshots.dead, 0.4f);

        StartCoroutine(deadPaneCoroutine());
    }

    public void hideDeadPane()
    {
        StartCoroutine(hidePaneCoroutine(deadPane, false, true));
    }

    public void showSettingsPane()
    {
        settingsPane.SetActive(true);
        settingsPane.GetComponent<Animator>().SetBool("isHide", false);
    }

    public void hideSettingsPane()
    {
        StartCoroutine(hidePaneCoroutine(settingsPane, true, true));
    }

    public void setActiveShopPane(bool active)
    {
        shopPane.SetActive(active);
    }

    public void showShopPane()
    {
        shopPane.SetActive(true);
        shopPane.GetComponent<Animator>().SetBool("isHide", false);      
    }

    public void hideShopPane()
    {
        StartCoroutine(shopPaneCoroutine());
    }

    private IEnumerator shopPaneCoroutine()
    {
        yield return StartCoroutine(hidePaneCoroutine(shopPane, true, true));

        buttonsController.playButton.gameObject.SetActive(true);
        buttonsController.noAdsButton.gameObject.SetActive(true);
        buttonsController.shopButton.gameObject.SetActive(true);
        buttonsController.settingsButton.gameObject.SetActive(true);
        buttonsController.achievementsButton.gameObject.SetActive(true);
        buttonsController.scoreboardButton.gameObject.SetActive(true);

        TimeCounter.isMenu = true;
    }

    private IEnumerator deadPaneCoroutine()
    {
        if (!continueGameIsUsed && AdController.isNoAds)
        {

            yield return StartCoroutine(buttonsController.continueGameCoroutine());
        }
        else
        {
            deadPane.SetActive(true);

            backgroundController.stop();
            skinController.stop();

            buttonsController.moneyX2Button.gameObject.SetActive(true);
            buttonsController.againButton.gameObject.SetActive(true);
            buttonsController.menuButton.gameObject.SetActive(true);

            if (Application.internetReachability != NetworkReachability.NotReachable && moneyController.moneyEarned > 0)
            {
                buttonsController.moneyX2Button.gameObject.GetComponentInParent<Animator>().SetBool("IsFade", true);
                moneyX2Text.text = "+" + moneyController.moneyEarned;
                buttonsController.moneyX2Button.interactable = true;
            }
            else
            {
                buttonsController.moneyX2Button.interactable = false;
            }

            counter.SetActive(false);

            reloadStat();

            deadPane.GetComponent<Animator>().SetBool("isHide", false);
        }
    }

    public Animator getDeadPaneAnimator()
    {
        return deadPane.GetComponent<Animator>();
    }

    public IEnumerator showRatingPaneCoroutine()
    {
        yield return new WaitUntil(() => TimeCounter.isMenu);

        yield return StartCoroutine(hidePaneCoroutine(settingsPane, true, true));

        buttonsController.playButton.gameObject.SetActive(false);
        ratingPane.SetActive(true);
        ratingPane.GetComponent<Animator>().SetBool("isHide", false);
        
        yield return new WaitForSeconds(ratingPane.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(1);
        ratingPaneChildren.SetActive(true);
    }

    public void hideRatingPane()
    {
        StartCoroutine(hidePaneCoroutine(ratingPane, true, false));
    }

    private IEnumerator hidePaneCoroutine(GameObject pane, bool setActivePlayButton, bool setPaneActiveFalse)
    {
        pane.GetComponent<Animator>().SetBool("isHide", true);

        yield return new WaitForSeconds(pane.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        if(setPaneActiveFalse) pane.SetActive(false);
        buttonsController.playButton.gameObject.SetActive(setActivePlayButton);
    }

    public IEnumerator transitionPaneCoroutineAfterInit(bool toShow)
    {
        transitionPaneImage.color = new Color(0, 0, 0, 1);

        float currentTime = 0;

        while (currentTime < 1)
        {
            if (PurchaseManager.IsInitialized()) break;
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        StartCoroutine(transitionPaneCoroutine(toShow));
    }

    public IEnumerator transitionPaneCoroutine(bool toShow)
    {
        transitionIsEnd = false;

        float t = 0;

        if (toShow)
        {
            transitionPaneImage.raycastTarget = true;
            transitionPaneImage.color = new Color(0, 0, 0, 0);

            while (t < timeTransitionPane)
            {
                transitionPaneImage.color = new Color(0, 0, 0, t / timeTransitionPane);
                t += Time.deltaTime;

                yield return null;
            }
            transitionPaneImage.color = new Color(0, 0, 0, 1);
        }
        else
        {
            transitionPaneImage.color = new Color(0, 0, 0, 1);

            while (t < timeTransitionPane)
            {
                transitionPaneImage.color = new Color(0, 0, 0, 1 - t / timeTransitionPane);
                t += Time.deltaTime;

                yield return null;
            }
            transitionPaneImage.color = new Color(0, 0, 0, 0);
            transitionPaneImage.raycastTarget = false;
        }

        transitionIsEnd = true;
    }

    private void reloadStat()
    {
        counterController.setMaxRecord();

        counterStringText.text = LanguageController.langStrings.deadPaneCurrentCountText;
        counterText.text = ((int)(counterController.counter)).ToString();
        recordStringText.text = LanguageController.langStrings.deadPaneRecordText;
        recordText.text = counterController.getMaxRecord().ToString();

        moneyEarnedStringText.gameObject.SetActive(true);
        moneyEarnedStringText.text = LanguageController.langStrings.moneyEarnText;
        moneyEarnedText.text = "+" + moneyController.moneyEarned.ToString();

        moneyController.addToMoney(moneyController.moneyEarned);

        TimeCounter.saveTimeInGame();
        moneyController.saveMoney();
    }
}
