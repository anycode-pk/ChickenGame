using Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


[System.Serializable]
public class MoveController
{

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 10f;
    public Vector2 direction;
    private bool isFacingRight = true;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpPower = 10f;
    //[SerializeField] private float jumpDelay = 0.25f;
    //private float jumpTimer;

    [Header("Physics")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float fallMultiplier = 5f;

    [Header("Collisions")]
    [SerializeField] private float groundLength = 0.6f;
    [SerializeField] private Vector3 colliderOffset;
    public bool onGround;
    private float waitTime = 0.7f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private Animator animator;
    [SerializeField] public Transform transform;

    [Header("Audio")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip heartSound;
    [SerializeField] private AudioClip diamondSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource audioSource;
    

    [Header("StringToHash")]
    private static readonly int UserNotMovingChicken = Animator.StringToHash("UserNotMovingChicken");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Death = Animator.StringToHash("Death");

    public void InitComponents(Rigidbody2D rb, BoxCollider2D col, Animator animator, Transform transform)
    {
        this.rb = rb;
        this.col = col;
        this.animator = animator;
        this.transform = transform;
    }
    
    public void FixedUpdate()
    {
        MoveCharacter(direction.x);
        ModifyPhysics();
    }

    public void Update()
    {
        MovementCheck();   
        OneWayPlatformMovement();
    }

    private void MovementCheck()
    {
        CheckGround();
        if (Input.GetButtonDown("Jump")) 
            Jump();
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    private void CheckGround()
    {
        var position = transform.position;
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            onGround = Physics2D.Raycast(position + colliderOffset
                           , Vector2.down, groundLength, groundLayer) ||
                       Physics2D.Raycast(position - colliderOffset
                           , Vector2.down, groundLength, groundLayer);
            animator.SetBool(IsJumping, !onGround);
        }
    }

    private void OneWayPlatformMovement()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (waitTime <= 0)
            {
                Physics2D.IgnoreLayerCollision(9, 7, true);
                rb.AddForce(Vector2.down * 5f);
                waitTime = 0.7f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 7, false);
        }
    }

    private void MoveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * (horizontal * moveSpeed));

        animator.SetBool(UserNotMovingChicken, horizontal == 0);

        if((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
            Flip();

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        
        animator.SetFloat(Speed,Mathf.Abs(rb.velocity.x));
        animator.SetBool(IsJumping, false);
    }
    

    private void Jump()
    {
        if (!onGround) return;

        audioSource.PlayOneShot(jumpSound);
        animator.SetTrigger(Jump1);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }


    private void ModifyPhysics()
    {
        var xVelocity = rb.velocity.x;
        bool changingDirections = (direction.x > 0 && xVelocity < 0) || (direction.x < 0 && xVelocity > 0);

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
    private void Flip() 
    {
        isFacingRight = !isFacingRight;
        transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }

    public void DrawDebugFeet() 
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position + colliderOffset, position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(position - colliderOffset, position - colliderOffset + Vector3.down * groundLength);
    }
    //private void Die()
    // {
    //    //rb.bodyType = RigidbodyType2D.Static;
    //     audioSource.PlayOneShot(deathSound);
    //     CinemachineVirtualCamera vCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    //     rb.transform.position += Vector3.up * 1.7f;
    //     vCam.Follow = null;
    //     col.enabled = false;
    //     animator.SetTrigger(Death);
    // }

     private void RestartLevel()
     {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }

    public void ColCheckEnter(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform")) 
        {
            Physics2D.IgnoreCollision(other.collider, col, true);
        }
    }
    
    public void ColStayBehaviour(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Physics2D.IgnoreCollision(other.collider, col, true);
        }
    }
    public void ColCheckExit(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            Physics2D.IgnoreCollision(other.collider, col, false);
        }
    }

    public GameObject PickUpCoin(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            var coinCounter = GameObject.FindWithTag("CoinCounter");
            coinCounter.GetComponent<CoinCounter>().AddCoin();
            audioSource.PlayOneShot(coinSound);
            return other.gameObject;
        } 
        return null;
    }

    

    public GameObject PickUpDiamond(Collider2D other)
    {
        if (other.gameObject.CompareTag("Diamond"))
        {
            var diamondCounter = GameObject.FindWithTag("DiamondCounter");
            diamondCounter.GetComponent<DiamondCounter>().AddDiamond();
            audioSource.PlayOneShot(diamondSound);
            return other.gameObject;
        }
        return null;
    }
} 
