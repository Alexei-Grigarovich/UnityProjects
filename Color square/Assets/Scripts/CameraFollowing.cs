using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    private Transform cameraTransform;

    [SerializeField] private Transform objectTransform;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;  

    private static float speedCameraFollowing; 

    void Start()
    {
        cameraTransform = GetComponent<Transform>();

        speedCameraFollowing = 0;
    }

    public static void setSpeedCameraFollowing(float speed)
    {
        speedCameraFollowing = speed;
    }

    void Update()
    {
        cameraTransform.position = new Vector3(objectTransform.position.x + offsetX, Mathf.Lerp(cameraTransform.position.y, objectTransform.position.y + offsetY, speedCameraFollowing * Time.unscaledDeltaTime), cameraTransform.position.z);
    }
}
