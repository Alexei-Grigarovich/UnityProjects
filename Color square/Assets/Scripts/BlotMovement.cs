using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlotMovement : MonoBehaviour
{
    private AudioController audioController;
    private Transform blotTransform;
    private SpriteRenderer spriteRenderer;

    [SerializeField, Range(0, 1)] private float minAlpha;
    [SerializeField] private float maxScale;
    [SerializeField, Range(0, 1)] private float bassTrashhold;

    private Color colorAlpha = Color.white;
    private Vector2 scaleVector;
    private Vector2 minScaleVector;
    private Vector2 maxScaleVector;

    private float bassVolume;

    void Start()
    {
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        blotTransform = GetComponent<Transform>();

        minScaleVector = new Vector2(blotTransform.localScale.x, blotTransform.localScale.y);
        maxScaleVector = new Vector2(maxScale, maxScale);
    }

    void Update()
    {
        bassVolume = audioController.getMusicBassVolume();
        if (BlotSpawner.blotSpeed > 0f && !audioController.audioIsMute && bassVolume > bassTrashhold && Time.timeScale > 0)
        {
            colorAlpha.a = Mathf.Lerp(minAlpha, 1, bassVolume * bassVolume);
            scaleVector = Vector2.Lerp(minScaleVector, maxScaleVector, bassVolume * bassVolume);

            spriteRenderer.color = colorAlpha;
            blotTransform.localScale = scaleVector;
        }

        blotTransform.Translate(blotTransform.InverseTransformDirection(Vector2.left) * BlotSpawner.blotSpeed * Time.deltaTime);
    }
}
