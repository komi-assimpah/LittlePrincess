using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;

    public float speed = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        bool isWalking = Input.GetKey(KeyCode.Z);
        bool isWalkingBackward = Input.GetKey(KeyCode.S);
        bool isWalkingLeft = Input.GetKey(KeyCode.Q);
        bool isWalkingRight = Input.GetKey(KeyCode.D);
        
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isWalkingBackward", isWalkingBackward);
        animator.SetBool("isWalkingLeft", isWalkingLeft);
        animator.SetBool("isWalkingRight", isWalkingRight);
        
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.Z)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.Q)) moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        transform.position += moveDirection.normalized * speed * Time.deltaTime;
    }
}