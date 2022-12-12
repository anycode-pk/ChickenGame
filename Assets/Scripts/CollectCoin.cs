using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] private CoinsCounter counter;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
            counter.AddCoinsCount(1);
        }
    }
}
