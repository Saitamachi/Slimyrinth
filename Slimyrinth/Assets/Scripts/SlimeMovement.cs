using System;
using System.Data.Common;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float speed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public BoxCollider2D hitbox;

    public bool topCol = false;
    public bool botCol = false;
    public bool leftCol = false;
    public bool rightCol = false;
    public bool[] collisions;

    private bool isGrounded;
    void Start()
    {
    }

    void Update()
    {
        Movement();
        Jump();
        //Debug.Log("Slime: " + SlimeTouchingDirection());
        //Debug.Log("Wall: " + WallTouchingDirection());
    }
    void Movement()
    {
        HorizontalMovement();
        VerticalMovement();
        if (TouchingTiles())
        {
            rigidBody.gravityScale = 0;
        }
        else
        {
            rigidBody.gravityScale = 3;
            gameObject.transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.y, 0);
        }
    }


    void HorizontalMovement()
    {
        int zRotationValue = 0;
        bool isTouchingCeiling = Physics2D.Raycast(transform.position, Vector2.up, 0.45f, groundLayer);
        if (isTouchingCeiling)
            zRotationValue = 180;

        bool isTouchingGround = Physics2D.Raycast(transform.position, Vector2.down, 0.45f, groundLayer);
        if (isTouchingGround)
            zRotationValue = 0;

        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0 && (isTouchingCeiling || isTouchingGround))
        {
            if (moveInput < 0) // left
            {
                gameObject.transform.rotation = Quaternion.Euler(0, isTouchingGround ? 180 : 0, zRotationValue); // rotates left 
            }
            else if (moveInput > 0) // right
            {
                gameObject.transform.rotation = Quaternion.Euler(0, isTouchingGround ? 0 : 180, zRotationValue); // rotates right
            }

            // im moving left or right
            rigidBody.linearVelocity = new Vector2(moveInput * speed, rigidBody.linearVelocity.y);
        }
    }

    void VerticalMovement()
    {
        int zRotationValue = 0;
        bool isTouchingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.45f, groundLayer);
        if (isTouchingWallLeft)
            zRotationValue = 270;
        bool isTouchingWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.45f, groundLayer);
        if (isTouchingWallRight)
            zRotationValue = 90;

        float moveInput = Input.GetAxis("Vertical");

        if (moveInput != 0 && (isTouchingWallLeft || isTouchingWallRight))
        {
            Vector2 wallsCollision = SlimeTouchingDirection();
            if (moveInput < 0)
            {
                int xRotationValue = 0;
                if (isTouchingWallRight)
                {
                    xRotationValue = 180;
                }
                gameObject.transform.rotation = Quaternion.Euler(xRotationValue, 0, zRotationValue);
                rigidBody.transform.rotation = Quaternion.Euler(xRotationValue, 0, zRotationValue);
            }
            else if (moveInput > 0)
            {
                int xRotationValue = 180;
                if (isTouchingWallRight)
                {
                    xRotationValue = 0;
                }
                gameObject.transform.rotation = Quaternion.Euler(xRotationValue, 0, zRotationValue);
                rigidBody.transform.rotation = Quaternion.Euler(xRotationValue, 0, zRotationValue);
            }

            transform.position = new Vector3(transform.position.x, transform.position.y + SlimeTouchingDirection().y, transform.position.z);

            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, moveInput * speed);

        }
    }

    bool TouchingTiles()
    {
        bool isTouchingWallRight = Physics2D.Raycast(transform.position, transform.right, 0.45f, groundLayer);
        bool isTouchingWallLeft = Physics2D.Raycast(transform.position, -transform.right, 0.45f, groundLayer);
        bool isTouchingWallDown = Physics2D.Raycast(transform.position, -transform.up, 0.35f, groundLayer);
        return isTouchingWallDown || isTouchingWallLeft || isTouchingWallRight;
    }

    Vector2 SlimeTouchingDirection()
    {
        if (!TouchingTiles())
            return Vector2.zero;
        Vector2 vector = new Vector2();
        if (Physics2D.Raycast(transform.position, -transform.right, 0.45f, groundLayer))
            vector.x = -1;
        if (Physics2D.Raycast(transform.position, transform.right, 0.45f, groundLayer))
            vector.x = 1;
        if (Physics2D.Raycast(transform.position, -transform.up, 0.45f, groundLayer))
            vector.y = -1;
        return vector;
    }
    Vector2 WallTouchingDirection()
    {
        if (!TouchingTiles())
            return Vector2.zero;
        Vector2 vector = new Vector2();
        if (Mathf.Approximately(transform.eulerAngles.z, 0f))
            vector.y = -1;
        if (Mathf.Approximately(transform.eulerAngles.z, 90f))
            vector.x = 1;
        if (Mathf.Approximately(transform.eulerAngles.z, 180f))
            vector.y = 1;
        if (Mathf.Approximately(transform.eulerAngles.z, 270f) || Mathf.Approximately(transform.eulerAngles.z, -90f))
            vector.x = -1;
        return vector;
    }


    void Jump()
    {
        Vector2 slimeUp = transform.up;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jumping");
            Debug.Log("Touching slime: " + SlimeTouchingDirection());
            if (SlimeTouchingDirection().y == -1)
            {
                Debug.Log("Touching Ground:" + slimeUp * jumpForce);
                Debug.Log("Linear Velocity:" + rigidBody.linearVelocity);
                rigidBody.linearVelocity += slimeUp * jumpForce;
                Debug.Log("Linear Velocity:" + rigidBody.linearVelocity);
            }
        }
    }
}
