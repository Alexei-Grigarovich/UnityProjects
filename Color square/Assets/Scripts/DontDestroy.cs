using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static GameObject gameObjectInstance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (gameObjectInstance == null) gameObjectInstance = gameObject;
        else Destroy(gameObject);
    }
}
