using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlatformSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] platformColors; //0 - white, 1 - yellow, 2 - green, 3 - blue, 4 - purple

    public void setPlatformComponentColor(int color)
    {
        if(color >= 0 && color < platformColors.Length) GetComponent<SpriteRenderer>().sprite = platformColors[color];
    }
}
