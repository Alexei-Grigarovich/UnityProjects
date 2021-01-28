using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowing : MonoBehaviour
{
    private Transform cameraTransform;

    [SerializeField] private Transform objectTransform;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    public float speedXCameraFollowing;
    public float speedYCameraFollowing;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }

    public void setXSpeedCameraFollowing(float speed)
    {
        speedXCameraFollowing = speed;
    }

    public void setYSpeedCameraFollowing(float speed)
    {
        speedYCameraFollowing = speed;
    }

    void FixedUpdate()
    {
        cameraTransform.position = new Vector3(Mathf.Lerp(cameraTransform.position.x, objectTransform.position.x + offsetX, speedXCameraFollowing * Time.fixedUnscaledDeltaTime), 
            Mathf.Lerp(cameraTransform.position.y, objectTransform.position.y + offsetY, speedYCameraFollowing * Time.fixedUnscaledDeltaTime), 
            cameraTransform.position.z);
    }
}
