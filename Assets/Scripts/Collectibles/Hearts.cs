using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    public bool moving = true;
    public float speed = 3f;
    public float offset = .2f;
    float startPositionY;

    void Start()
    {
        startPositionY = transform.position.y;
    }

    void Update()
    {
        if (moving)
        {
            float y = Mathf.Sin(Time.time * speed) * offset + startPositionY;
            transform.position = new Vector2(transform.position.x, y);
        }
    }
}
