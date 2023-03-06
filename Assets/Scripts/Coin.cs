using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public bool moving = false;
    public float speed = 1.0f;
    public float offset;
    float startPositionY;
    
    void Start()
    {
        startPositionY = transform.position.y;
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
    [SerializeField] private CoinsCounter counter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Chicken")
        {          
            if (counter != null) { counter.AddCoinsCount(1); }                       
            Destroy(this.gameObject);          
        }
    }
   
    void Update()
    {
        if (moving)
        {
            float y = Mathf.Lerp(startPositionY + offset, startPositionY - offset, Mathf.PingPong(Time.time / speed, 1));
            transform.position = new Vector2(transform.position.x, y);
        }
    }
    /*
    public TextMeshProUGUI coinsDisplay;
    private int count;

    public void AddCoinsCount(int numberOfCoins)
    {
        count += numberOfCoins;
        coinsDisplay.text = "" + count;
    }*/
}

