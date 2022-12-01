using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private CharacterController controller;
    //public GameObject chicken;
    [SerializeField] private float speed = 3f;
    
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }
    
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
        
        controller.Move(move * (Time.deltaTime * speed));
    }
}
