using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
        }
    }
}
