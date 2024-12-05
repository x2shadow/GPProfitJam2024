using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

    Vector2 moveDirection;

    bool isJumping;

    void Start()
    {
        inputReader.MoveEvent += HandleMove;

        inputReader.JumpEvent += HandleJump;
        inputReader.JumpCanceledEvent += HandleJumpCanceled;
    }

    void Update()
    {
        Move();
        Jump();
    }

    void HandleMove(Vector2 direction)
    {
        moveDirection = direction;
    }

    void HandleJump()
    {
        isJumping = true;
    }

    void HandleJumpCanceled()
    {
        isJumping = false;
    }

    void Move()
    {
        if(moveDirection == Vector2.zero) return;

        transform.position += new Vector3(moveDirection.x, 0, moveDirection.y) * speed * Time.deltaTime;
    }

    void Jump()
    {
        if(isJumping) transform.position += Vector3.up * jumpSpeed * Time.deltaTime;
    }
}
