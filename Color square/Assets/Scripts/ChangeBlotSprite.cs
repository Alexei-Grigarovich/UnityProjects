using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBlotSprite : MonoBehaviour
{
    private Transform blotTransform;
    private SpriteRenderer blotSpriteRenderer;

    [SerializeField] private Sprite[] blotSprites;

    public void setRandomBlotColor()
    {
        blotTransform = GetComponent<Transform>();
        blotSpriteRenderer = GetComponent<SpriteRenderer>();

        blotSpriteRenderer.sprite = blotSprites[Random.Range(0, blotSprites.Length)];
        blotTransform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}
