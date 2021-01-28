using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Transform carTransform;

    public float carSpeed;
    public float maxCarSpeed;
    public float carAcc;
    [Space(15)]
    public bool canToRide;

    private bool carCanTurn;
    private bool carIsTurned;

    private TurnZoneInfo turnZone;

    void Start()
    {
        carTransform = GetComponent<Transform>();

        carCanTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Zone")
        {
            Debug.Log("Enter in zone");

            carCanTurn = true;

            turnZone = collision.gameObject.GetComponent<TurnZoneInfo>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Zone")
        {
            Debug.Log("Exit iz zone");

            carCanTurn = false;

            if (!carIsTurned)
            {
                //Loose
                Debug.Log("Loose");
            }

            carIsTurned = false;
        }
    }

    public void turn()
    {
        Debug.Log("Click turn button");

        if (carCanTurn)
        {
            carIsTurned = true;

            //Запуск анимации
            switch (turnZone.side)
            {
                case Turn.left:

                    break;
                case Turn.right:

                    break;
            }
        }
    }

    private IEnumerator turnToLeftCoroutine()
    {
        yield return null;
    }

    private IEnumerator turnToRightCoroutine()
    {
        yield return null;
    }

    void FixedUpdate() //Car move
    {
        if (canToRide)
        {
            if (carSpeed < maxCarSpeed) carSpeed += carAcc * Time.fixedDeltaTime;
            carTransform.Translate(carSpeed * carTransform.InverseTransformVector(carTransform.up) * Time.fixedDeltaTime);
        }
    }
}
