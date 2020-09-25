using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinBassDetectorController : MonoBehaviour
{
    private Transform transform;

    [SerializeField] private AudioController audioController;
    [Space(15)]
    [SerializeField, Range(1, 5)] private float bassPower;
    [SerializeField, Range(0, 1)] private float bassTrashhold;
    [SerializeField] private Vector2 maxScale;

     public bool isPlay = false;
     public bool isPause = false;

    private float bassVolume;

    private Vector2 startScale;

    void Start()
    {
        transform = GetComponent<Transform>();

        startScale = new Vector2(transform.localScale.x, transform.localScale.y);

        stop();
    }

    public void play()
    {
        if (!isPlay)
        {
            isPlay = true;
        }
        isPause = false;
    }

    public void stop()
    {
        if (isPlay)
        {
            isPlay = false;
        }
    }

    public void pause()
    {
        if (isPlay && !isPause) isPause = true;
    }

    void Update()
    {
        bassVolume = audioController.getMusicBassVolume();

        if (bassVolume > bassTrashhold && !TimeCounter.isMenu && !audioController.audioIsMute && isPlay && !isPause)
        {
            transform.localScale = Vector2.Lerp(startScale, maxScale, Mathf.Pow(bassVolume, bassPower));
        }
    }
}
