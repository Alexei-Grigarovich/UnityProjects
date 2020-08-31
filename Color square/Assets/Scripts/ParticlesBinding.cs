using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesBinding : MonoBehaviour
{
    [SerializeField] private Transform squareTransform;
    [Space(15)]
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private Transform particlesTransform;
    private Vector2 place;

    void Start()
    {
        particlesTransform = GetComponent<Transform>();
    }

    void Update()
    {
        place = new Vector2(squareTransform.position.x + offsetX, squareTransform.position.y + offsetY);
        particlesTransform.position = place;
    }
}
