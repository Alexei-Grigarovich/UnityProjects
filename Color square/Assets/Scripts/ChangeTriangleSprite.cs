using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTriangleSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite whiteTriangle;
    [SerializeField] private Sprite fadeTriangle;
    [SerializeField] private Sprite filledTriangle;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setWhiteTriangle()
    {
        spriteRenderer.sprite = whiteTriangle;
    }

    public void setFadeTriangle()
    {
        spriteRenderer.sprite = fadeTriangle;
    }

    public void setFilledTriangle()
    {
        spriteRenderer.sprite = filledTriangle;
    }
}
