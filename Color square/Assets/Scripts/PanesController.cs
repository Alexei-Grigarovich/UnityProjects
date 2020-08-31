using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanesController : MonoBehaviour
{
    private Coroutine lastDeadPaneCoroutine;
    private MoneyController moneyController;
    private ButtonsController buttonsController;
    private CounterController counterController;

    [SerializeField] private BackgroundController backgroundController;
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private GameObject counter;
    [SerializeField] private GameObject deadPane;
    [SerializeField] private GameObject settingsPane;
    [SerializeField] private GameObject shopPane;
    [SerializeField] private GameObject ratingPane;
    [SerializeField] private GameObject ratingPaneChildren;
    [SerializeField] private GameObject timer;
    [Space(15)]
    [SerializeField] private Image transitionPaneImage;
    [SerializeField] private Sprite smallPane;
    [SerializeField] private Sprite bigPane;
    [Space(15)]
    [SerializeField] private Text counterStringText;
    [SerializeField] private Text counterText;
    [SerializeField] private Text recordStringText;
    [SerializeField] private Text recordText;
    [Space(15)]
    [SerializeField] private float maxTimeToDoubleTap;
    [SerializeField] private float timeTransitionPane;
    [SerializeField] private float deadPaneTime;

    private float deadPaneTimeLeft;
    private float startTapTime;

    private bool timerIsWork;    
    private bool isDoubleTap = false;
    private bool transitionIsEnd = false;

    [HideInInspector] public bool adIsWasUsed;

    void Start()
    {
        buttonsController = GetComponent<ButtonsController>();
        moneyController = GetComponent<MoneyController>();
        counterController = GetComponent<CounterController>();

        deadPane.SetActive(false);
        settingsPane.SetActive(false);

        StartCoroutine(transitionPaneCoroutine(false));
    }

    public void showDeadPane()
    {
        deadPane.SetActive(true);
        buttonsController.pauseButton.gameObject.SetActive(false);

        backgroundController.pause();
        audioController.changeSnapshot(MusicSnapshots.dead, 0.4f);

        lastDeadPaneCoroutine = StartCoroutine(deadPaneCoroutine(adIsWasUsed));
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
    }

    private IEnumerator deadPaneCoroutine(bool adIsUsed)
    {
        if (!adIsUsed && Application.internetReachability != NetworkReachability.NotReachable)
        {
            timer.SetActive(true);
            buttonsController.adButton.gameObject.SetActive(true);
            buttonsController.againButton.gameObject.SetActive(false);
            buttonsController.menuButton.gameObject.SetActive(false);

            deadPane.GetComponent<Image>().sprite = smallPane;
            deadPane.GetComponent<Animator>().SetBool("isHide", false);

            timerIsWork = true;

            deadPaneTimeLeft = deadPaneTime;
            Text childrenText = timer.GetComponentInChildren<Text>();

            while (deadPaneTimeLeft > 0)
            {
                deadPaneTimeLeft -= Time.deltaTime;
                childrenText.text = (Mathf.RoundToInt(deadPaneTimeLeft)).ToString();
                timer.GetComponent<Image>().fillAmount = deadPaneTimeLeft / deadPaneTime;

                yield return null;
            }

            timerIsWork = false;

            yield return StartCoroutine(hidePaneCoroutine(deadPane, false, false));
        }

        backgroundController.stop();

        timer.SetActive(false);
        buttonsController.adButton.gameObject.SetActive(false);
        buttonsController.againButton.gameObject.SetActive(true);
        buttonsController.menuButton.gameObject.SetActive(true);

        counter.SetActive(false);

        reloadStat();

        deadPane.GetComponent<Image>().sprite = bigPane;
        deadPane.GetComponent<Animator>().SetBool("isHide", false);      
    }

    public void stopDeadPaneCoroutine()
    {
        StopCoroutine(lastDeadPaneCoroutine);
    }

    public Animator getDeadPaneAnimator()
    {
        return deadPane.GetComponent<Animator>();
    }

    public IEnumerator showRatingPaneCoroutine()
    {
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

        counterController.saveMaxRecord();
        moneyController.saveMoney();
    }

    void Update()
    {
        if (timerIsWork)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - startTapTime > maxTimeToDoubleTap) isDoubleTap = false;

                if (isDoubleTap)
                {
                    if (Time.time - startTapTime <= maxTimeToDoubleTap)
                    {
                        deadPaneTimeLeft = 0;
                    }
                    else
                    {
                        isDoubleTap = false;
                    }
                }
                else
                {
                    startTapTime = Time.time;
                    isDoubleTap = true;
                }
            }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (Time.time - startTapTime > maxTimeToDoubleTap) isDoubleTap = false;

                    if (isDoubleTap)
                    {
                        if (Time.time - startTapTime <= maxTimeToDoubleTap)
                        {
                            deadPaneTimeLeft = 0;
                        } 
                        else
                        {
                            isDoubleTap = false;
                        }
                    }
                    else
                    {
                        startTapTime = Time.time;
                        isDoubleTap = true;
                    }
                }
            }
#endif
        }
    }
}
