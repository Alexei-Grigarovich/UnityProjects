using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlotSpawner : MonoBehaviour
{
    private RectTransform rectTransform;
    private Coroutine coroutine;

    [SerializeField] private GameObject blotPrefab;
    [SerializeField] private GameObject parent;

    [SerializeField] private float timeSpawnBlots;
    [SerializeField] private float startBlotSpeed;
    [SerializeField] private float blotStartStopTime;

    public static float blotSpeed = 0;

    [HideInInspector] public bool isPlay = false;
    [HideInInspector] public bool isPause = false; 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void play()
    {
        if (isPause && isPlay) StartCoroutine(startRideBlots());

        if (!isPlay)
        {
            isPlay = true;
            coroutine = StartCoroutine(spawnCoroutine());
            StartCoroutine(startRideBlots());
        }             
        isPause = false;
    }

    public void stop()
    {
        if (coroutine != null && isPlay)
        {
            isPlay = false;
            if (!isPause)
            {
                StopCoroutine(coroutine);
                StartCoroutine(stopRideBlots());
                GameObject[] blots = GameObject.FindGameObjectsWithTag("Blot");
                foreach (GameObject blot in blots) Destroy(blot, 0);
            }
        }
    }

    public void pause()
    {
        if (isPlay && !isPause)
        {
            isPause = true;
            StartCoroutine(stopRideBlots());
        }
    }

    IEnumerator startRideBlots()
    {
        float t = 0;
        while (t < blotStartStopTime)
        {
            blotSpeed = Mathf.Lerp(0, startBlotSpeed, t / blotStartStopTime);
            t += Time.deltaTime;
            yield return null;
        }
        blotSpeed = startBlotSpeed;
    }

    IEnumerator stopRideBlots()
    {
        float t = 0;
        while (t < blotStartStopTime)
        {
            blotSpeed = Mathf.Lerp(startBlotSpeed, 0, t / blotStartStopTime);
            t += Time.deltaTime;
            yield return null;
        }
        blotSpeed = 0;
    }

    IEnumerator spawnCoroutine()
    {
        while (true) {           
            if (isPlay && !isPause) spawnRandomBlot();
            yield return new WaitForSeconds(timeSpawnBlots);
        }
    }

    void spawnRandomBlot()
    {
        GameObject blot = Instantiate(blotPrefab, new Vector2(rectTransform.position.x + Random.Range(0, rectTransform.rect.width), rectTransform.position.y + Random.Range(0, rectTransform.rect.height)), Quaternion.identity);
        blot.GetComponent<ChangeBlotSprite>().setRandomBlotColor();
        blot.transform.SetParent(parent.transform);
    }
}
