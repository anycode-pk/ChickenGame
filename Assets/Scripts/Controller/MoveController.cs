using Cinemachine;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


[System.Serializable]
public class MoveController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool isFacingRight;

    [Header("Vertical Movement")]
    public float jumpSpeed = 10f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;
    
    [Header("Physics")]
    [SerializeField] public float maxSpeed = 7f;
    [SerializeField] public float linearDrag = 4f;
    [SerializeField] public float gravity = 1f;
    [SerializeField] public float fallMultiplier = 5f;

    [Header("Collisions")] 
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;
    public bool onGround;
    public bool isOnPlatform;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public LayerMask groundLayer;
    private BoxCollider2D col;
    public Animator animator;
    public Health health;

    [Header("Audio")] 
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + colliderOffset
                          , Vector2.down, groundLength, groundLayer) || 
                      Physics2D.Raycast(transform.position - colliderOffset
                          , Vector2.down, groundLength, groundLayer);

        if(Input.GetButtonDown("Jump")){
            jumpTimer = Time.time + jumpDelay;
        }
        if (!onGround)
        {
            animator.SetBool("IsJumping", true);
        }

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        OneWayPlatformMovement();
     }

    private void OneWayPlatformMovement()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            onGround = false;
            Physics2D.IgnoreLayerCollision(9, 7, true);
            //rb.AddForce(Vector2.down * 5f);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 7, false);
        }
    }

    private void MoveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * (horizontal * moveSpeed));
        
        animator.SetBool("UserNotMovingChicken", horizontal == 0 ? true : false);

        if((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
            Flip();
        
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) 
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        MoveCharacter(direction.x);
        animator.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
        animator.SetBool("IsJumping", false);
        
        if (jumpTimer > Time.time && (onGround))
            Jump();

        ModifyPhysics(); // deletes effect of sliding on surface 
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
    }


    private void ModifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            } 
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }
    private void Flip() {
        isFacingRight = !isFacingRight;
        transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Platform")) 
        {
            isOnPlatform = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            health.TakeDamage(3);
            ChickenDeath();         
        }
    }

    private void ChickenDeath()
    {
        //rb.bodyType = RigidbodyType2D.Static;
        CinemachineVirtualCamera vCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        rb.transform.position += Vector3.up * 1.7f;
        vCam.Follow = null;
        col.enabled = false;
        animator.SetTrigger("Death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Platform")) 
        {
            isOnPlatform = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Coin"))
        {
            var coinCounter = GameObject.FindWithTag("CoinCounter");
            audioSource.PlayOneShot(coinSound);
            Destroy(other.gameObject);
            if (coinCounter == null)
            {
                return;
            }
            coinCounter.GetComponent<CoinCounter>().AddCoin();
        }
    }
}
