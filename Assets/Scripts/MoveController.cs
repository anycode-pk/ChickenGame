using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool isFacingRight = false;

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
    public bool onGround = false;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public LayerMask groundLayer;
    public GameObject characterHolder;

    

    void Update()
    {
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if(Input.GetButtonDown("Jump")){
            jumpTimer = Time.time + jumpDelay;
        }
        
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * (horizontal * moveSpeed));
        
        if((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) 
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter(direction.x);
        if(jumpTimer > Time.time && (onGround))
        {
            Jump();
        }
        ModifyPhysics(); // deletes effect of sliding on surface 
    }

    private void Jump()
    {
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
            rb.gravityScale = 0;
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
}
