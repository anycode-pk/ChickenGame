using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private GameObject currentTransparentPlatform;
    [SerializeField] private BoxCollider2D chickenCollider;

    private void Start()
    {
        chickenCollider = GameObject.Find("Chicken").GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentTransparentPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TransparentPlatform"))
        {
            Debug.Log("coll enter");
            currentTransparentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TransparentPlatform"))
        {
            Debug.Log("coll exit");
            currentTransparentPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentTransparentPlatform.GetComponent<BoxCollider2D>();
        
        Physics2D.IgnoreCollision(chickenCollider, platformCollider,true);
        Debug.Log("Before: " + Physics2D.GetIgnoreCollision(chickenCollider, platformCollider));
        
        yield return new WaitForSeconds(0.5f);
        
        Physics2D.IgnoreCollision(chickenCollider, platformCollider, false);
        Debug.Log("After: " + Physics2D.GetIgnoreCollision(chickenCollider, platformCollider));
    }
}
