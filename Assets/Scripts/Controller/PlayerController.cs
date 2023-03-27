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
            Health health = GetComponent<Health>();
            Transform transform = GetComponent<Transform>();
            moveController.InitComponents(rb, col, anim, health, transform);
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
            GameObject coin = moveController.PickUpCoin(col);
            GameObject heart = moveController.PickUpHeart(col);
            GameObject diamond = moveController.PickUpDiamond(col);
            Destroy(coin);
            Destroy(heart);
            Destroy(diamond);
        }

    }