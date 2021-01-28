using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    [SerializeField] private Text counterText;

    [ReadOnly] public int counter;
    [ReadOnly] public int maxRecord;

    void Start()
    {
        counter = 0;

        if (PlayerPrefs.HasKey("MaxRecord"))
        {
            maxRecord = PlayerPrefs.GetInt("MaxRecord");
        }
        else
        {
            maxRecord = 0;
            PlayerPrefs.SetInt("MaxRecord", maxRecord);
        }
    }

    void Update()
    {
        
    }

    public void addPointsInCounter(int count)
    {
        count += count;
    }

    public void updateCounter()
    {
        setCounterTextForInt(counter);
    }

    public void setMaxRecord(int count)
    {
        maxRecord = count;
    }

    public void saveMaxRecord()
    {
        PlayerPrefs.SetInt("MaxRecord", maxRecord);
    }

    private void setCounterTextForInt(int count)
    {
        counterText.text = count.ToString();
    }
}
