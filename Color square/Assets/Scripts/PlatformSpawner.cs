using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private RectTransform spawnerTransform;

    [SerializeField] private GameObject[] platformPrefabs;
    [Space(15)]
    [SerializeField] private float stairHeight;
    [SerializeField] private float startingPlatformSpeed;
    [SerializeField] private float accelerationCoefficientPlatform;
    [SerializeField] private float maxPlatformSpeed;
    [Space(15)]
    [SerializeField] private int[] countSpawnColors = { 0, 0, 0, 0 };
    [SerializeField] private int maxCountOfDuplicateColors;

    private int lastPlatformColor;
    private int duplicatesColor;
    public int nextPlatformState = -1;

    private static float platformSpeed;
    private float spawnY;

    private Vector2 spawnerScale = new Vector2();
    private Vector2 spawnerPosition = new Vector2();
    private Vector2 platformPosition = new Vector2();

    public bool isHaveTriggeredPlatform;   

    void Start()
    {
        spawnerTransform = GetComponent<RectTransform>();

        spawnY = - stairHeight;
        spawnerTransform.position = new Vector2(spawnerTransform.position.x, spawnY);

        platformSpeed = startingPlatformSpeed;
    }

    public static float getPlatformSpeed()
    {
        return platformSpeed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform"))
        {
            isHaveTriggeredPlatform = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform"))
        {
            isHaveTriggeredPlatform = false;
        }
    }

    GameObject getRandomPlatform(bool isWhite)
    {
        GameObject platform = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        if (isWhite) platform.GetComponent<PlatformState>().setPlatformState(0);
        else platform.GetComponent<PlatformState>().setPlatformState(getRandomColor());

        return platform;
    }

    int getRandomColor()
    {
        int randNum = Random.Range(1, 5);

        if (lastPlatformColor == randNum)
        {
            duplicatesColor++;

            if (duplicatesColor >= maxCountOfDuplicateColors)
            {
                randNum--;
                if (randNum < 1) randNum = 4;               
                duplicatesColor = 0;
            }
        }
        else duplicatesColor = 0;

        lastPlatformColor = randNum;
        countSpawnColors[randNum - 1]++;

        if (nextPlatformState != -1)
        {
            randNum = nextPlatformState;
            nextPlatformState = -1;
        }

        return randNum;
    }

    void spawnRandomPlatform(Vector2 pos)
    {
        Instantiate(getRandomPlatform(false), pos, Quaternion.identity);
    }

    void spawnWhitePlatform(Vector2 pos)
    {
        Instantiate(getRandomPlatform(true), pos, Quaternion.identity);
    }

    void FixedUpdate()
    {
        if (MainSquareController.getIsPlay())
        {
            if (platformSpeed < maxPlatformSpeed) platformSpeed += accelerationCoefficientPlatform;

            spawnerScale.x = platformSpeed / startingPlatformSpeed;
            spawnerScale.y = spawnerTransform.localScale.y;
            spawnerTransform.localScale = spawnerScale;

            if (!isHaveTriggeredPlatform)
            {
                platformPosition.x = spawnerTransform.position.x + (spawnerTransform.rect.width * spawnerTransform.localScale.x) - 0.01f;
                platformPosition.y = spawnerTransform.position.y;
                spawnRandomPlatform(platformPosition);

                spawnY -= stairHeight;

                spawnerPosition.x = spawnerTransform.position.x;
                spawnerPosition.y = spawnY;
                spawnerTransform.position = spawnerPosition;

                isHaveTriggeredPlatform = true;
            }
        }
    }
}
