using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglesController : MonoBehaviour
{
    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField] private GameObject[] trianglesContainer;
    [Space(15)]
    [SerializeField] private int minTrianglesToShow;
    [SerializeField] private int maxTrianglesToShow;
    [Space(15)]
    [SerializeField, Range(0, 1)] private float timeUp;
    [SerializeField, Range(0, 5)] private float timeDown;
    [SerializeField, Range(1, 5)] private float bassPower;
    [SerializeField, Range(0, 1)] private float bassTrashhold;
    [Space(15)]
    [SerializeField] private bool useTrianglesIsShow;  

    private List<BackgroundTriangleController> triangles = new List<BackgroundTriangleController>();
    
    private float bassVolume;

    [HideInInspector] public bool isPlay = false;
    [HideInInspector] public bool isPause = false;

    void Start()
    {
        triangles = getBackgroundTrianglesList();

        foreach (GameObject con in trianglesContainer) con.SetActive(false);
    }

    public void play()
    { 
        if (!isPlay)
        {
            isPlay = true;
            foreach (GameObject con in trianglesContainer) con.SetActive(isPlay);
        }
        isPause = false;
    }

    public void stop()
    {
        if (isPlay)
        {
            isPlay = false;
            foreach (BackgroundTriangleController triangle in triangles)
            {
                triangle.spriteRenderer.color = Color.clear;
                triangle.isShow = false;
            }
            foreach (GameObject con in trianglesContainer) con.SetActive(isPlay);
        }
    }

    public void pause()
    {
        if (isPlay && !isPause) isPause = true;
    }

    public void setAllTrianglesNotShow()
    {
        foreach (BackgroundTriangleController triangle in triangles) triangle.isShow = false;
    }

    List<BackgroundTriangleController> getBackgroundTrianglesList()
    {
        List<BackgroundTriangleController> list = new List<BackgroundTriangleController>();

        foreach (GameObject triangle in trianglesContainer)
        {
            BackgroundTriangleController[] controllers = triangle.GetComponentsInChildren<BackgroundTriangleController>();

            foreach (BackgroundTriangleController controller in controllers) list.Add(controller);
        }

        return list;
    }

    void Update()
    {
        bassVolume = audioController.getMusicBassVolume();

        if (bassVolume > bassTrashhold && !TimeCounter.isMenu && !audioController.audioIsMute && isPlay && !isPause)
        {           
            for (int i = 0; i < Mathf.Lerp(minTrianglesToShow, maxTrianglesToShow, Mathf.Pow(bassVolume, bassPower)); i++)
            {               
                int num = Random.Range(0, triangles.Count);
                if (useTrianglesIsShow)
                {
                    if (!triangles[num].isShow) triangles[num].show(timeUp, timeDown);
                }
                else triangles[num].show(timeUp, timeDown);
            }
        }
    }
}
