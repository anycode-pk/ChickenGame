using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player properties")]
    public MoveController moveController = new MoveController();
    public AttackController attackController = new AttackController();
    public HealthController healthController = new HealthController();



    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Animator anim = GetComponent<Animator>();
        Transform transform = GetComponent<Transform>();
        moveController.InitComponents(rb, col, anim, transform);
        healthController.SetHp();
    }

    private void Update()
    {
        moveController.Update();
    }

    private void FixedUpdate()
    {
        moveController.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        moveController.DrawDebugFeet();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        moveController.ColCheckEnter(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        moveController.ColCheckExit(other);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var collectible = col.gameObject;
        switch (collectible.tag)
        {
            case "Coin":
                GameObject coin = moveController.PickUpCoin(col);
                Destroy(coin);
                break;
            case "Diamond":
                GameObject diamond = moveController.PickUpDiamond(col);
                Destroy(diamond);
                break;
            case "Heart":
                GameObject heart = healthController.PickUpHeart(col);
                Destroy(heart);
                break;

        }
    }
}