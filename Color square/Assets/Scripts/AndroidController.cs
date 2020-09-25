using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidController : MonoBehaviour
{
    [SerializeField] private float maxTimeToDoubleTap;

    private float startTapTime;

    private bool isDoubleTap = false;

    void Update()
    {
        if (TimeCounter.isMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.time - startTapTime > maxTimeToDoubleTap) isDoubleTap = false;

                if (isDoubleTap)
                {
                    if (Time.time - startTapTime <= maxTimeToDoubleTap)
                    {
                        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) Application.Quit();
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
    }
}
