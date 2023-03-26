using UnityEngine;
using Random = UnityEngine.Random;

public class Coins : MonoBehaviour
{
    public bool moving = true;
    public float speed = 3f;
    public float offset = .2f;
    float startPositionY;
    void Start()
    {
        startPositionY = transform.position.y;
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
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