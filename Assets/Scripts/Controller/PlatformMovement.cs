using System;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Vector2 movementOffset;
    [SerializeField] private float movementSpeed;
    private Vector2 currentPosition;

    private void Start()
    {
        currentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        var f = Mathf.Sin(Time.time * movementSpeed);
        transform.position = new Vector2( currentPosition.x + f*movementOffset.x, currentPosition.y + f*movementOffset.y);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Chicken"))
        {
            col.gameObject.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Chicken"))
        {
            col.gameObject.transform.parent = null;
        }
    }
}
