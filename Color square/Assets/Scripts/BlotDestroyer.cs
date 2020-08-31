using UnityEngine;

public class BlotDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Blot")) Destroy(collider.gameObject, 0);
    }
}
