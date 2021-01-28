using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
    straight,
    left,
    right
}

public class RoadSpawner : MonoBehaviour
{
    public GameObject straightRoad;
    public GameObject leftTurnRoad;
    public GameObject rightTurnRoad;
    public GameObject turnZone;
    [Space(15)]
    public GameObject roadParent;
    [Space(15)]
    public float roadWide;
    public int countSpawnRoads;
    [Space(15)]
    [Range(0, 0.5f)] public float leftRoadWeight;
    [Range(0, 0.5f)] public float rightRoadWeight;
    private float straightRoadWeight;

    private GameObject lastRoad;

    private GameObject lastTurnZone;
    private TurnZoneInfo lastTurnZoneInfo;

    private Vector2 spawnVector;
    private Vector2 lastRoadDirection;
    private Turn lastRoadTurn;

    void Start()
    {
        spawnVector = new Vector2(0, 0);
        lastRoadDirection = Vector2.up;

        straightRoadWeight -= rightRoadWeight + leftRoadWeight;
    }

    void Update()
    {
        
    }

    private void spawnNextRoad()
    {
        Turn turn = getRandomTurn();
        Vector2 spawnPos = spawnVector;

        if (spawnVector == Vector2.zero)
        {
            turn = Turn.straight;
        }

        //Спавн зоны для нажатия
        if (turn == Turn.left || turn == Turn.right && lastRoad != null)
        {
            lastTurnZone = Instantiate(turnZone, lastRoad.transform);
            lastTurnZone.transform.localPosition = Vector2.zero;

            lastTurnZoneInfo = lastTurnZone.GetComponent<TurnZoneInfo>();
            lastTurnZoneInfo.side = turn;
        }     

        switch (turn) //Спавн дороги
        {
            case Turn.straight:
                lastRoad = Instantiate(straightRoad, spawnVector, Quaternion.Euler(0, 0,
                    lastRoadDirection == Vector2.up || lastRoadDirection == Vector2.down ? 0 : 90));

                if (lastRoadDirection == Vector2.zero) lastRoadDirection = Vector2.up;
                break;
            case Turn.left:
                lastRoad = Instantiate(leftTurnRoad, spawnVector, Quaternion.Euler(0, 0,
                    lastRoadDirection == Vector2.down ? 180 : (lastRoadDirection ==
                    Vector2.left ? 90 : (lastRoadDirection == Vector2.right ? -90 : 0))));

                if (lastRoadDirection != Vector2.zero) lastRoadDirection = lastRoadDirection == Vector2.right ? Vector2.up : (lastRoadDirection == Vector2.up ? Vector2.left : (lastRoadDirection == Vector2.left ? Vector2.down : Vector2.right));
                else lastRoadDirection = Vector2.left;
                break;
            case Turn.right:
                lastRoad = Instantiate(rightTurnRoad, spawnVector, Quaternion.Euler(0, 0,
                    lastRoadDirection == Vector2.down ? 180 : (lastRoadDirection ==
                    Vector2.left ? 90 : (lastRoadDirection == Vector2.right ? -90 : 0))));

                if (lastRoadDirection != Vector2.zero) lastRoadDirection = lastRoadDirection == Vector2.left ? Vector2.up : (lastRoadDirection == Vector2.up ? Vector2.right : (lastRoadDirection == Vector2.right ? Vector2.down : Vector2.left));
                else lastRoadDirection = Vector2.right;
                break;
        }

        lastRoad.gameObject.transform.parent = roadParent.transform;

        lastRoadTurn = turn;

        spawnVector += lastRoadDirection * roadWide * 0.7f;
    }

    private Turn getRandomTurn()
    {
        Turn turn = Random.Range(0f, 1f) < rightRoadWeight ? Turn.right : Random.Range(0f, 1f) < leftRoadWeight ? Turn.left : Turn.straight;

        if (lastRoadTurn == Turn.right || lastRoadTurn == Turn.left) turn = Turn.straight;

        return turn;
    }
}
