using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovent : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float jumpHeight = 5.0f;

    private enum MovementState {idle,running,jump,falling }

    Vector2 movementVector;

    Rigidbody2D rbody;
    CapsuleCollider2D capsuleCollider;
    Animator animator;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rbody=GetComponent<Rigidbody2D>();
        capsuleCollider=GetComponent<CapsuleCollider2D>();
        animator=GetComponent<Animator>();
        sprite=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerVelocity = new Vector2(movementVector.x * movementSpeed, rbody.velocity.y);
        rbody.velocity = playerVelocity;

        UpdateAnimationUpdate();
    }

    //Input System
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                return;
            }
            if (Input.GetButtonDown("Jump"))
            {
                rbody.velocity += new Vector2(0f, jumpHeight);
            }
        }
    }
    private void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
        Debug.Log(movementVector);
    }

    //Animation
    public void UpdateAnimationUpdate()
    {
        MovementState state;
        if (movementVector.x > 0.0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (movementVector.x < 0.0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rbody.velocity.y > .1f)
        {
            state = MovementState.jump;
        } 
        else if (rbody.velocity.y < -.1f) 
        {
            state= MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
    }
}
