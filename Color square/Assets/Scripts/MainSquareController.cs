using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSquareController : MonoBehaviour
{
    private Transform squareTransform;
    private SpriteRenderer squareSpriteRenderer;

    [SerializeField] private ChangeTriangleSprite[] trianglesScripts;
    [SerializeField] private PanesController panesController;
    [Space(15)]
    public int squareState;
    [SerializeField] private float rotateDuration;
    [Space(15)]
    [SerializeField] private Sprite whiteSquareSprite;
    [SerializeField] private Sprite coloredSquareSprite;
    [Space(15)]
    public GameObject startPlatform;
    [SerializeField] private GameObject teachObject;
    [Space(15)]
    [SerializeField] private AudioSource whooshAudio;
    [Space(15)]
    public Color[] colors;
    [Space(15)]
    public ParticleSystem rideParticles;
    public ParticleSystem deadParticles;
    public ParticleSystem.MainModule rideParticlesMain;
    public ParticleSystem.MainModule deadParticlesMain;

    [HideInInspector] public bool isCanPress = true;
    [HideInInspector] public bool isTurn;
    [HideInInspector] public bool isGrounded;

    private static bool isPlay;

    void Start()
    {
        squareTransform = GetComponent<Transform>();
        squareSpriteRenderer = GetComponent<SpriteRenderer>();

        rideParticlesMain = rideParticles.main;
        deadParticlesMain = deadParticles.main;

        rideParticlesMain.startColor = colors[squareState];

        setWhiteSquare();

        rideParticles.Stop();
        deadParticles.Stop();
    }

    public static bool getIsPlay()
    {
        return isPlay;
    }

    public static void setIsPlay(bool setPLay)
    {
        isPlay = setPLay;
    }

    public void setRandomSquareRotate()
    {
        squareTransform.rotation = Quaternion.Euler(0, 0, (float)(Random.Range(0, 4)) * 90f);
    }

    private void setActiveTriangleFromSquareState()
    {
        for (int i = 0; i < trianglesScripts.Length; i++)
        {
            if (i == squareState - 1) trianglesScripts[i].setFilledTriangle();
            else trianglesScripts[i].setFadeTriangle();
        }       
    }

    private void setNonActiveTriangle()
    {
        for (int i = 0; i < trianglesScripts.Length; i++) trianglesScripts[i].setFadeTriangle();
    }

    private void setWhiteSquare() {
        squareState = 0;
        squareSpriteRenderer.sprite = whiteSquareSprite;

        for (int i = 0; i < trianglesScripts.Length; i++) trianglesScripts[i].setWhiteTriangle();
    }

    public void setColoredSquare(bool withActiveTriangle)
    {
        squareSpriteRenderer.sprite = coloredSquareSprite;

        if (withActiveTriangle) setActiveTriangleFromSquareState();
            else setNonActiveTriangle();
    }

    public void setSquareStateFromRotate()
    {
        squareState = (int)((squareTransform.rotation.eulerAngles.z + 90) / 90);
    }

    private void setSquareStateToSide(int side)
    {
        squareState -= side;

        if (squareState == 0) squareState = 4;
        else if (squareState == 5) squareState = 1;
    }

    private IEnumerator turnSquare(int side, float duration)
    {
        isTurn = true;

        setSquareStateToSide(side);

        whooshAudio.pitch = Random.Range(0.9f, 1.1f);
        whooshAudio.Play();

        Quaternion fromRotate = squareTransform.rotation;
        Quaternion toRotate = squareTransform.rotation * Quaternion.Euler(new Vector3(0, 0, 1) * (90 * -side));

        float t = 0;
        while (t < duration)
        {
            squareTransform.rotation = Quaternion.Slerp(fromRotate, toRotate, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        squareTransform.rotation = toRotate;
        //setSquareStateFromRotate();

        isTurn = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {          
            isGrounded = true;

            if (isPlay)
            {
                rideParticlesMain.gravityModifier = -0.1f;
                rideParticlesMain.startColor = colors[squareState];
                rideParticles.Play();

                int platformState = collision.gameObject.GetComponent<PlatformState>().getPlatformState();

                //Lose
                if (platformState != squareState)
                {
                    if (platformState != 0)
                    {
                        isPlay = false;

                        rideParticles.Stop();

                        deadParticlesMain.startColor = colors[platformState];
                        deadParticles.Play();
                        Invoke("stopDeadParticles", deadParticlesMain.startLifetime.constant);

                        startPlatform = collision.gameObject;

                        if (teachObject == null) panesController.showDeadPane();
                    }
                    else setWhiteSquare();
                }
                else
                {
                    setActiveTriangleFromSquareState();
                }
            }
        }
    }

    void stopDeadParticles()
    {
        deadParticles.Stop();
    }

    void OnCollisionExit2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "Platform")
        {
            isGrounded = false;

            if (isPlay)
            {
                rideParticlesMain.gravityModifier = 0.7f;
                rideParticles.Stop();

                setNonActiveTriangle();
            }            
        }
    }

    void Update()
    {
        if (isPlay && isCanPress)
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!isTurn && !isGrounded) StartCoroutine(turnSquare(1, rotateDuration));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!isTurn && !isGrounded) StartCoroutine(turnSquare(-1, rotateDuration));
            }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x > Screen.width / 2)
                    {
                        if (!isTurn && !isGrounded) StartCoroutine(turnSquare(1, rotateDuration));
                    }
                    else
                    {
                        if (!isTurn && !isGrounded) StartCoroutine(turnSquare(-1, rotateDuration));
                    }
                }
            }
#endif
        }
    }
}
