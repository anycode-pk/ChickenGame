
using UnityEngine;

public class VerticalPlatformMovement : MonoBehaviour
{
    private PlatformEffector2D _effector2D;
    public float waitTime;
    private MoveController _moveController;
    
    void Start()
    {
        _effector2D = GetComponent<PlatformEffector2D>();
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 0.5f;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            if (waitTime <= 0)
            {
                _effector2D.rotationalOffset = 180f;
                waitTime = 0.5f;
            }
            else
            {
                waitTime = waitTime - Time.deltaTime;
            }
        }
        
        if (_moveController)
        {
            _effector2D.rotationalOffset = 0;
        }
    }
}
