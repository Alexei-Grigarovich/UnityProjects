using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    private Transform backgroundTransform;

    private float backgroundWidth;
    private float screenWidth;

    void Start()
    {
        backgroundTransform = GetComponent<Transform>();

        setBackgroundWidth();
    }

    void setBackgroundWidth()
    {
        backgroundWidth = GetComponent<RectTransform>().rect.width;
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x * 2;
    }

    void FixedUpdate()
    {
        backgroundTransform.Translate(Vector2.left * BackgroundController.backgroundSpeed * Time.deltaTime);

        if (backgroundTransform.position.x <= -(screenWidth / 2) - backgroundWidth / 2)
            backgroundTransform.position = new Vector2(backgroundTransform.position.x + backgroundWidth * 2, backgroundTransform.position.y);
    }
}
