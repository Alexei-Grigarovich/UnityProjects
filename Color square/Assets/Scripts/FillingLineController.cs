using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillingLineController : MonoBehaviour
{
    private Slider slider;

    [SerializeField] private CounterController counterController;
    [SerializeField] private MoneyController moneyController;    
    [SerializeField] private Animator moneyToAddText;
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private int moneyToAdd;

    private bool isFull;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
    }

    IEnumerator moneyTextShowCoroutine()
    {
        moneyToAddText.gameObject.SetActive(true);

        moneyToAddText.GetComponent<Animator>().Play("show");
        yield return null;
        yield return new WaitForSeconds(moneyToAddText.GetCurrentAnimatorStateInfo(0).length);

        moneyToAddText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (MainSquareController.getIsPlay())
        {
            slider.value = ((counterController.counter - 1) % 100) / 100 + 0.01f;

            if (slider.value < 0.1f) isFull = false;

            if (slider.value == 1 && !isFull)
            {
                isFull = true;
                moneyController.addToMoneyEarned(moneyToAdd);
                audioController.playMoneySound(1f);
                StartCoroutine(moneyTextShowCoroutine());
            }
        }
    }
}
