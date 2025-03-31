using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float speed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public LayerMask waterLayer;
    public Transform groundCheck;
    public BoxCollider2D hitbox;
    public Animator animator;
    bool isRotating = false;

    public bool movingOverEdge = false;


    private Vector2 edgeMovement;
    private bool isGrounded;
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();  // Get the Animator component if not assigned in the Inspector
        }
    }

    void Update()
    {
        Movement();
        Jump();
    }
    void Movement()
    {

        if (TouchingTiles() && !isRotating)
        {


            Vector2 collision = SlimeTouchingDirection();
            Vector2 collisionLiterally = SlimeTouchingDirectionLiterally();
            // touching right only but player not facing right
            if (collision.x == 1 && collisionLiterally.y != -1 && collision.y == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 90);
                rigidBody.linearVelocity = new Vector2(5, rigidBody.linearVelocity.y);
            }
            // touching left &&  under is not left   &&   no vertical collision
            else if (collision.x == -1 && collisionLiterally.y != -1 && collision.y == 0)
            {
                Debug.Log("rotating1");
                gameObject.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 270);
                rigidBody.linearVelocity = new Vector2(-5, rigidBody.linearVelocity.y);
            }
            // touching bottom only but player not facing bottom
            else if (collision.y == -1 && collisionLiterally.y != -1 && collision.x == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, -5);
            }
            //touching top      &&  under is not top       &&    no horizontal collision

            else if (collision.y == 1 && collisionLiterally.y != -1 && collision.x == 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, Mathf.Approximately(transform.eulerAngles.y, 180f) ? 0 : 180, 180);
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 5);
            }
            rigidBody.gravityScale = 0;


        }
        else if (TouchingWater() && !isRotating)
        {
            rigidBody.gravityScale = 1;
            gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
        else if (!isRotating)
        {
            rigidBody.gravityScale = 3;
            gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
        HorizontalMovement();
        VerticalMovement();
        Handle270Degrees();
    }




    void HorizontalMovement()
    {
        if (isRotating)
            return;
        int zRotationValue = 0;
        bool isTouchingCeiling = Physics2D.Raycast(transform.position, Vector2.up, 0.45f, groundLayer);
        if (isTouchingCeiling)
            zRotationValue = 180;

        bool isTouchingGround = Physics2D.Raycast(transform.position, Vector2.down, 0.45f, groundLayer);
        if (isTouchingGround)
            zRotationValue = 0;

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0 && ((isTouchingCeiling || isTouchingGround) || (Mathf.Approximately(transform.eulerAngles.z, 0f))) && !movingOverEdge && !isRotating)
        {
            if (moveInput < 0) // left
            {

                gameObject.transform.rotation = Quaternion.Euler(0, isTouchingCeiling ? 0 : 180, zRotationValue); // rotates left 
            }
            else if (moveInput > 0) // right
            {
                gameObject.transform.rotation = Quaternion.Euler(0, isTouchingCeiling ? 180 : 0, zRotationValue); // rotates right
            }

            if (isTouchingCeiling && SlimeTouchingDirectionLiterally().y != -1)
            {
                rigidBody.linearVelocityY += 0.5f;
            }
            rigidBody.linearVelocity = new Vector2(moveInput * speed, rigidBody.linearVelocity.y);
        }
        if (moveInput == 0 && (isTouchingCeiling || isTouchingGround))
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * .9f, rigidBody.linearVelocity.y);
        }
    }

    void VerticalMovement()
    {

        if (isRotating)
            return;
        int zRotationValue = 0;
        bool isTouchingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.45f, groundLayer);
        if (isTouchingWallLeft)
        {
            zRotationValue = 270;
        }
        bool isTouchingWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.45f, groundLayer);
        if (isTouchingWallRight)
        {
            zRotationValue = 90;
        }

        float moveInput = Input.GetAxisRaw("Vertical");


        if (moveInput != 0 && (isTouchingWallLeft || isTouchingWallRight) && !movingOverEdge && !isRotating)
        {
            Debug.Log("rotating2");

            if (moveInput < 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(isTouchingWallLeft ? 0 : 180, 0, zRotationValue); // rotates right
            }
            else if (moveInput > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(isTouchingWallLeft ? 180 : 0, 0, zRotationValue); // rotates right
            }

            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, moveInput * speed);

        }
        if (moveInput == 0 && (isTouchingWallLeft || isTouchingWallRight))
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.y * .9f);
        }
    }

    bool TouchingTiles()
    {

        bool isTouchingWallRight = Physics2D.Raycast(transform.position, transform.right, 0.45f, groundLayer);
        bool isTouchingWallLeft = Physics2D.Raycast(transform.position, -transform.right, 0.45f, groundLayer);
        bool isTouchingWallDown = Physics2D.Raycast(transform.position, -transform.up, 0.35f, groundLayer);
        bool isTouchingWallUp = Physics2D.Raycast(transform.position, transform.up, 0.35f, groundLayer);
        return isTouchingWallDown || isTouchingWallLeft || isTouchingWallRight || isTouchingWallUp;
    }

    bool TouchingWater()
    {
        Vector2 colliderSize = hitbox.size;
        Vector2 colliderCenter = hitbox.bounds.center;

        Collider2D[] hits = Physics2D.OverlapBoxAll(colliderCenter, colliderSize, 0f, waterLayer);

        return hits.Length > 0;
    }

    Vector2 SlimeTouchingDirectionLiterally()
    {
        Vector3 bottomCenter = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 bottomLeft = new Vector3(transform.position.x - 0.45f, transform.position.y, 0f);
        Vector3 bottomRight = new Vector3(transform.position.x + 0.45f, transform.position.y, 0f);


        Vector2 vector = new Vector2();
        if (Physics2D.Raycast(transform.position, -transform.right, 0.45f, groundLayer))
            vector.x = -1;
        if (Physics2D.Raycast(transform.position, transform.right, 0.45f, groundLayer))
            vector.x = 1;
        if (Physics2D.Raycast(bottomCenter, -transform.up, 0.32f, groundLayer) ||
            Physics2D.Raycast(bottomLeft, -transform.up, 0.32f, groundLayer) ||
            Physics2D.Raycast(bottomRight, -transform.up, 0.32f, groundLayer))
        {
            vector.y = -1;
        }
        if (Physics2D.Raycast(transform.position, transform.up, 0.32f, groundLayer))
            vector.y = 1;
        return vector;
    }

    Vector2 SlimeTouchingDirection()
    {

        Vector2 vector = new Vector2();
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.45f, groundLayer))
            vector.x = -1;
        else if (Physics2D.Raycast(transform.position, Vector2.right, 0.45f, groundLayer))
            vector.x = 1;
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.32f, groundLayer))
            vector.y = -1;
        else if (Physics2D.Raycast(transform.position, Vector2.up, 0.32f, groundLayer))
            vector.y = 1;
        return vector;
    }
    Vector2 WallTouchingDirection()
    {
        if (!TouchingTiles())
            return Vector2.zero;
        Vector2 vector = new Vector2();
        if (Mathf.Approximately(transform.eulerAngles.z, 0f))
        {
            vector.y = -1;
        }
        else if (Mathf.Approximately(transform.eulerAngles.z, 90f))
        {
            vector.x = 1;
        }
        else if (Mathf.Approximately(transform.eulerAngles.z, 180f))
            vector.y = 1;
        else if (Mathf.Approximately(transform.eulerAngles.z, 270f))
        {
            vector.x = -1;
        }
        return vector;
    }


    void Jump()
    {
        Vector2 slimeUp = transform.up;
        if (Input.GetKeyDown(KeyCode.Space) && !movingOverEdge)
        {
            Debug.Log("Jumping");
            if (SlimeTouchingDirectionLiterally().y == -1)
            {
                Debug.Log("Touching Ground:" + slimeUp * jumpForce);
                rigidBody.linearVelocity += slimeUp * jumpForce;

            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (hitbox == null) return;

        Gizmos.color = Color.green;

        Vector2 center = (Vector2)transform.position + hitbox.offset;
        float halfWidth = hitbox.size.x * 0.5f;
        float halfHeight = hitbox.size.y * 0.5f;


        // Draw Rays
        Gizmos.DrawSphere(new Vector3(transform.position.x - .45f, transform.position.y - .32f, 5), .01f);
        Gizmos.DrawSphere(new Vector3(transform.position.x + .45f, transform.position.y - .32f, 5), .01f);
        Gizmos.DrawSphere(new Vector3(center.x, transform.position.y - .32f, 5), .01f);


        Vector3 bottomCenter = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 bottomLeft = new Vector3(transform.position.x - 0.45f, transform.position.y, 0f);
        Vector3 bottomRight = new Vector3(transform.position.x + 0.45f, transform.position.y, 0f);
        Debug.DrawRay(bottomCenter, -transform.up * 0.33f, Color.red);
        Debug.DrawRay(bottomLeft, -transform.up * 0.33f, Color.red);
        Debug.DrawRay(bottomRight, -transform.up * 0.33f, Color.red);

    }

    void Handle270Degrees()
    {
        Vector3 bottomCenter = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 bottomLeft = new Vector3(transform.position.x - transform.right.x * 0.45f, transform.position.y, 0f);
        Vector3 bottomRight = new Vector3(transform.position.x + transform.right.x * 0.45f, transform.position.y, 0f);

        bool center = Physics2D.Raycast(bottomCenter, -transform.up, 0.32f, groundLayer);
        bool back = Physics2D.Raycast(bottomLeft, -transform.up, 0.32f, groundLayer);
        bool front = Physics2D.Raycast(bottomRight, -transform.up, 0.32f, groundLayer);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (back && center && !front && input.x != 0 && input.y != 0)
        {

            isRotating = true;


            float grid = 1f;
            float newX = 0f;
            Vector2 directionToGo = transform.forward;
            Debug.Log("forward: " + input);
            if (input.x > 0)
            {
                newX = Mathf.Ceil(transform.position.x / grid) * grid;
                Debug.Log(newX);
                StartCoroutine(MoveEdgeOverTime(new Vector2(newX + hitbox.size.x / 2 - .25f - transform.position.x, input.y / 2 * 1.1f)));
            }
            else if (input.x < 0)
            {
                newX = Mathf.Floor(transform.position.x / grid) * grid;
                StartCoroutine(MoveEdgeOverTime(new Vector2(newX - hitbox.size.y / 2 + .1f - transform.position.x, input.y / 2 * 1.1f)));
            }
            else
            {
                newX = Mathf.Round(transform.position.x / grid) * grid;
                StartCoroutine(MoveEdgeOverTime(new Vector2(newX - transform.position.x, input.y / 2 * 1.2f)));
            }
        }
    }

    Quaternion DefineAngles(Vector2 direction)
    {

        // top floor
        if (SlimeTouchingDirection().y == -1)
        {
            if (direction.x < 0 && direction.y < 0) // left and bottom arrow
            {
                return Quaternion.Euler(0, 180, -90);
            }
            else if (direction.x > 0 && direction.y < 0) // right and bottom arrow
            {
                return Quaternion.Euler(0, 0, -90);
            }
        }


        // left floor
        if (SlimeTouchingDirection().x == -1)
        {
            if (direction.x > 0 && direction.y < 0) // right and bottom arrow
            {
                return Quaternion.Euler(0, 180, 180);
            }
            else if (direction.x > 0 && direction.y > 0) // right and top arrow
            {
                return Quaternion.Euler(0, 0, 0);
            }
        }



        // right floor
        if (SlimeTouchingDirection().x == 1)
        {
            if (direction.x < 0 && direction.y < 0) // left and bottom arrow
            {
                return Quaternion.Euler(0, 0, 180);
            }
            else if (direction.x < 0 && direction.y > 0) // left and top arrow
            {
                return Quaternion.Euler(0, 180, 0);
            }
        }



        // bottom floor

        if (SlimeTouchingDirection().y == 1)
        {
            if (direction.x > 0 && direction.y > 0) // right and top arrow
            {
                return Quaternion.Euler(180, 0, -90);
            }
            else if (direction.x < 0 && direction.y > 0) // left and top arrow
            {
                return Quaternion.Euler(-180, -180, -90);
            }
        }

        return Quaternion.Euler(0, 0, 0);

    }

    IEnumerator MoveEdgeOverTime(Vector2 dydx)
    {
        Vector3 startPosition = transform.position;
        Debug.Log(dydx.x + transform.position.x);
        isRotating = true;
        Vector3 targetPosition = startPosition + new Vector3(dydx.x, 0, 0);
        Vector3 targetPosition2 = startPosition + new Vector3(dydx.x, dydx.y, 0);
        Quaternion startRotation = transform.rotation;

        Quaternion targetRotation = DefineAngles(dydx);
        Debug.Log(targetRotation.eulerAngles);
        float elapsedTime = 0f;
        rigidBody.linearVelocity = Vector2.zero;
        while (elapsedTime < 0.5f)
        {
            GetComponent<Collider2D>().isTrigger = true;
            float fraction = elapsedTime / 0.5f;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {

            float fraction = elapsedTime / 0.5f;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;

        elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            Debug.Log(isRotating);
            Debug.Log(transform.rotation.eulerAngles);
            float fraction = elapsedTime / 0.3f;
            transform.position = Vector3.Lerp(targetPosition, targetPosition2, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition2;

        while (elapsedTime < 0.5f)
        {
            Debug.Log(isRotating);
            Debug.Log(transform.rotation.eulerAngles);
            float fraction = elapsedTime / 0.5f;
            transform.position = Vector3.Lerp(targetPosition, targetPosition2, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition2;
        GetComponent<Collider2D>().isTrigger = false;


        yield return new WaitForSeconds(.5f);

        isRotating = false;

    }

}
