using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private Vector2 movementVector;
    [SerializeField] private float jumpStrength;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        movementVector = new Vector2(Input.GetAxis("Horizontal"),0);
        rb.AddForce(movementVector*movementSpeed);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up*jumpStrength);
        }
    }
}
