using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] private CoinsCounter counter;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
            if (counter == null) { return; }

            counter.AddCoinsCount(1);
        }
    }
}
