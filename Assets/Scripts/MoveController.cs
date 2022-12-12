using UnityEngine;

public class MoveController : MonoBehaviour
{
    private float horizontal;
    [SerializeField, Range(1, 20)] private float jumpingPower = 16f;
    [SerializeField, Range(1, 20)] private float speed = 8f;
    private bool isFacingRight;

    
    [SerializeField, Range(0,40)] private float jumpingPower = 16f;
    [SerializeField, Range(0,20)] private float speed = 8f;
    [SerializeField, Range(0,1)] private float fallingVelocity = 0.7f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            var velocity = rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y * 0.5f); // allows to jump higher when longer pushed
            rb.velocity = velocity;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fallingVelocity); // allows to jump higher when longer pushed
        }
        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer); // invisible circle under feet
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.localScale = localScale;
        }
    }

}
