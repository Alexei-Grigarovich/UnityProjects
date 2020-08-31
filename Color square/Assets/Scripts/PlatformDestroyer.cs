using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform"))
        {
            Destroy(collider.gameObject, timeToDestroy);
        }
    }
}
