using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Transform platformTransform;

    private Vector2 moveDirection = new Vector2(-1, 0);
    private static float speed;

    void Start()
    {
        platformTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (MainSquareController.getIsPlay())
        {           
            platformTransform.Translate(moveDirection * PlatformSpawner.getPlatformSpeed() * Time.deltaTime);
        }
    }
}
