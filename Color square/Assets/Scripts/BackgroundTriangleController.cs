using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriangleController : MonoBehaviour
{   
    private Coroutine coroutine;

    [HideInInspector] public SpriteRenderer spriteRenderer;

    [HideInInspector] public float saturationMin = 0.5f;
    [HideInInspector] public float brightnessMin = 0.5f;

    public bool isShow;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Random.ColorHSV(0, 1, saturationMin, 1, brightnessMin, 1, 0, 0);
        isShow = false;
    }

    public void show(float firstTime, float secondTime)
    {
        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(showCoroutine(firstTime, secondTime));
    }

    private IEnumerator showCoroutine(float firstTime, float secondTime)
    {
        isShow = true;

        Color firstColor = Random.ColorHSV(0, 1, saturationMin, 0.8f, brightnessMin, 0.8f, 1, 1);
        Color secondColor = Random.ColorHSV(0, 1, saturationMin, 0.8f, brightnessMin, 0.8f, 0, 0);    

        float t = 0;
        while (t < firstTime)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, firstColor, t / firstTime);
            t += Time.deltaTime;
            yield return null;
        }
        
        t = 0;
        while (t < secondTime)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, secondColor, t / secondTime);
            t += Time.deltaTime;
            yield return null;
        }
        isShow = false;
    }
}
