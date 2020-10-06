using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachingController : MonoBehaviour
{
    [SerializeField] private MainSquareController mainSquareController;
    [SerializeField] private PlatformSpawner platformSpawner;
    [SerializeField] private ButtonsController buttonsController;
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private GameObject[] toOff;
    [Space(15)]
    [SerializeField] private GameObject teachingObject;
    [SerializeField] private GameObject stick;
    [Space(15)]
    [SerializeField] private Animator arrowLeft;
    [SerializeField] private Animator arrowRight;
    [SerializeField] private Animator tapLeft;
    [SerializeField] private Animator tapRight;
    [Space(15)]
    [SerializeField] private Text message;
    [SerializeField] private Text tapLeftText;
    [SerializeField] private Text tapRightText;
    [Space(15)]
    public bool isStarted;

    public int[] nextStates = new int[3];

    void Awake()
    {
        if (PlayerPrefs.HasKey("Teach"))
        {
            Destroy(teachingObject);
            Destroy(gameObject);
        }
    }

    void setTextMessage(string text)
    {
        message.text = text;
    }

    IEnumerator spawnPlatformsWithState()
    {
        platformSpawner.nextPlatformState = nextStates[0];

        yield return new WaitWhile(() => platformSpawner.nextPlatformState != -1);

        platformSpawner.nextPlatformState = nextStates[1];

        yield return new WaitWhile(() => platformSpawner.nextPlatformState != -1);

        platformSpawner.nextPlatformState = nextStates[2];
    }

    IEnumerator teachCoroutine()
    {
        foreach (GameObject obj in toOff) obj.SetActive(false);

        mainSquareController.isCanPress = false;

        yield return new WaitWhile(() => mainSquareController.isGrounded);
        yield return new WaitForSeconds(0.1f);
        mainSquareController.isCanPress = true;

        Time.timeScale = 0;
        
        stick.SetActive(true);
        teachingObject.SetActive(true);
        setTextMessage(LanguageController.langStrings.teachingTexts[0]);
        arrowRight.gameObject.SetActive(true);
        arrowRight.Play("show");
        tapRight.gameObject.SetActive(true);
        tapRight.Play("show");


#if UNITY_ANDROID && !UNITY_EDITOR
        yield return new WaitUntil(() => Input.touchCount > 0);
#else
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow));
#endif
        teachingObject.SetActive(false);
        arrowRight.gameObject.SetActive(false);
        tapRight.gameObject.SetActive(false);

        mainSquareController.isCanPress = false;

        Time.timeScale = 1;

        yield return new WaitUntil(() => mainSquareController.isGrounded);
        if (!MainSquareController.getIsPlay()) buttonsController.againButtonAct();
        yield return new WaitWhile(() => mainSquareController.isGrounded);
        yield return new WaitForSeconds(0.1f);
        mainSquareController.isCanPress = true;

        Time.timeScale = 0;

        teachingObject.SetActive(true);
        setTextMessage(LanguageController.langStrings.teachingTexts[1]);
        arrowLeft.gameObject.SetActive(true);
        arrowLeft.Play("show");
        tapLeft.gameObject.SetActive(true);
        tapLeft.Play("show");

#if UNITY_ANDROID && !UNITY_EDITOR
        yield return new WaitUntil(() => Input.touchCount > 0);
#else
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow));
#endif
        teachingObject.SetActive(false);
        arrowLeft.gameObject.SetActive(false);
        tapLeft.gameObject.SetActive(false);
        mainSquareController.isCanPress = false;

        Time.timeScale = 1;

        yield return new WaitUntil(() => mainSquareController.isGrounded);
        if (!MainSquareController.getIsPlay()) buttonsController.againButtonAct();
        yield return new WaitWhile(() => mainSquareController.isGrounded);
        yield return new WaitForSeconds(0.1f);
        mainSquareController.isCanPress = true;

        Time.timeScale = 0;

        teachingObject.SetActive(true);
        setTextMessage(LanguageController.langStrings.teachingTexts[2]);
        arrowLeft.gameObject.SetActive(true);
        arrowRight.gameObject.SetActive(true);
        tapLeft.gameObject.SetActive(true);
        tapRight.gameObject.SetActive(true);
        arrowLeft.Play("show");
        tapLeft.Play("show");
        arrowRight.Play("show");
        tapRight.Play("show");
        tapLeftText.gameObject.SetActive(true);
        tapRightText.gameObject.SetActive(true);

#if UNITY_ANDROID && !UNITY_EDITOR
        yield return new WaitUntil(() => Input.touchCount > 0);
#else
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow));
#endif

        tapLeftText.gameObject.SetActive(false);
        tapRightText.gameObject.SetActive(false);

        mainSquareController.isCanPress = false;

        Time.timeScale = 1;

        yield return new WaitWhile(() => mainSquareController.isTurn);

        Time.timeScale = 0;

        mainSquareController.isCanPress = true;
#if UNITY_ANDROID && !UNITY_EDITOR
        yield return new WaitUntil(() => Input.touchCount > 0);
#else
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow));
#endif
        teachingObject.SetActive(false);
        arrowLeft.gameObject.SetActive(false);
        arrowRight.gameObject.SetActive(false);
        tapLeft.gameObject.SetActive(false);
        tapRight.gameObject.SetActive(false);

        mainSquareController.isCanPress = false;

        Time.timeScale = 1;

        yield return new WaitUntil(() => mainSquareController.isGrounded);
        yield return new WaitForSeconds(0.1f);
        mainSquareController.isCanPress = true;

        if (MainSquareController.getIsPlay())
        {
            Time.timeScale = 0;

            stick.SetActive(false);
            teachingObject.SetActive(true);
            setTextMessage(LanguageController.langStrings.teachingTexts[3]);

            message.gameObject.GetComponent<Animator>().Play("hide");
            yield return null;
            yield return new WaitForSecondsRealtime(message.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            foreach (GameObject obj in toOff) obj.SetActive(true);

            audioController.playMusic();
            Time.timeScale = 1;
            endOfTeaching();
            Destroy(teachingObject);
            Destroy(gameObject);
        }
        else buttonsController.againButtonAct();
    }

    void endOfTeaching()
    {
        PlayerPrefs.SetInt("Teach", 1);
    }

    void Update()
    {
        if (MainSquareController.getIsPlay() && !isStarted)
        {
            isStarted = true;

            int squareState = mainSquareController.squareState;

            nextStates[0] = squareState - 1 < 1 ? 4 : squareState - 1;
            nextStates[1] = squareState;
            nextStates[2] = squareState == 1 ? 3 : squareState == 2 ? 4 : squareState == 3 ? 1 : 2;
           
            StartCoroutine(spawnPlatformsWithState());
            StartCoroutine(teachCoroutine());
        }
    }
}
