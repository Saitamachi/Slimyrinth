using System;
using System.Collections;
using System.Data.Common;
using UnityEditor;
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
            //Debug.Log(transform.right);
            Vector2 collision = SlimeTouchingDirection();
            Vector2 collisionLiterally = WallTouchingDirection();
            // touching right only but player not facing right
            if (collision.x == 1 && collisionLiterally.x != 1 && collision.y == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                rigidBody.linearVelocity = new Vector2(5,rigidBody.linearVelocity.y);
            }
            // touching left only but player not facing left
            if (collision.x == -1 && collisionLiterally.x != -1 && collision.y == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                rigidBody.linearVelocity = new Vector2(-5,rigidBody.linearVelocity.y);
            }
            // touching bottom only but player not facing bottom
            if (collision.y == -1 && collisionLiterally.y != -1 && collision.x == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.y,-5);
            }
            //touching top only
            if (collision.y == 1 && collisionLiterally.y != 1 && collision.x == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.y,5);
            }
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

        //Vector3 pivot = new Vector3(transform.position.x + hitbox.size.x / 2, transform.position.y - hitbox.size.y / 2, transform.position.z);
        //Vector3 relativePosition = transform.position - pivot;

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

            if(isTouchingCeiling)
                rigidBody.linearVelocityY = .5f;

            if(isTouchingGround)
                rigidBody.linearVelocityY = -.5f;
            //Debug.Log("pivot: " + pivot);
            //Debug.Log("relative pos: " + relativePosition);
            //transform.position = pivot + relativePosition;

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

        //Vector3 pivot = new Vector3(transform.position.x + hitbox.size.x / 2, transform.position.y - hitbox.size.y / 2, transform.position.z);
        //Vector3 relativePosition = transform.position - pivot;

        if (moveInput != 0 && (isTouchingWallLeft || isTouchingWallRight))
        {
            if (moveInput < 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(isTouchingWallRight ? 180 : 0, 0, zRotationValue); // rotates right
            }
            else if (moveInput > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(isTouchingWallRight ? 0 : 180, 0, zRotationValue); // rotates right
            }

            //transform.position = pivot + relativePosition;

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

    Vector2 SlimeTouchingDirectionLiterally()
    {
        if (!TouchingTiles())
            return Vector2.zero;
        Vector2 vector = new Vector2();
        if (Physics2D.Raycast(transform.position, -transform.right, 0.45f, groundLayer))
            vector.x = -1;
        if (Physics2D.Raycast(transform.position, transform.right, 0.45f, groundLayer))
            vector.x = 1;
        if (Physics2D.Raycast(transform.position, -transform.up, 0.32f, groundLayer))
            vector.y = -1;
        return vector;
    }

    Vector2 SlimeTouchingDirection()
    {
        if (!TouchingTiles())
            return Vector2.zero;
        Vector2 vector = new Vector2();
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.45f, groundLayer))
            vector.x = -1;
        if (Physics2D.Raycast(transform.position, Vector2.right, 0.45f, groundLayer))
            vector.x = 1;
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.32f, groundLayer))
            vector.y = -1;
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.32f, groundLayer))
            vector.y = 1;
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
            if (SlimeTouchingDirectionLiterally().y == -1)
            {
                Debug.Log("Touching Ground:" + slimeUp * jumpForce);
                rigidBody.linearVelocity += slimeUp * jumpForce;
            }
        }
    }
}
