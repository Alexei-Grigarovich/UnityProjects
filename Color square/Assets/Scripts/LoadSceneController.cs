using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    private AsyncOperation loadSceneAsyncOperation;

    void Start()
    {
        loadSceneAsyncOperation = SceneManager.LoadSceneAsync(1);
    }
}
