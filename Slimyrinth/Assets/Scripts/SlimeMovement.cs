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

        if (TouchingTiles())
        {


            //Debug.Log(transform.right);
            Vector2 collision = SlimeTouchingDirection();
            Vector2 collisionLiterally = SlimeTouchingDirectionLiterally();
            // touching right only but player not facing right
            if (collision.x == 1 && collisionLiterally.y != -1 && collision.y == 0)
            {
                Debug.Log("right: " + collisionLiterally);
                gameObject.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 90);
                rigidBody.linearVelocity = new Vector2(5, rigidBody.linearVelocity.y);
            }
            // touching left &&  under is not left   &&   no vertical collision
            else if (collision.x == -1 && collisionLiterally.y != -1 && collision.y == 0)
            {
                Debug.Log("left: " + collisionLiterally);
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
                Debug.Log("EULER BEFORE: " + transform.rotation.eulerAngles);
                Debug.Log(Mathf.Approximately(transform.eulerAngles.y, 180f));
                gameObject.transform.rotation = Quaternion.Euler(0, Mathf.Approximately(transform.eulerAngles.y, 180f) ? 0 : 180, 180);
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 5);
                Debug.Log("EULER AFTER: " + transform.rotation.eulerAngles);
            }
            rigidBody.gravityScale = 0;


        }
        else
        {
            rigidBody.gravityScale = 3;
            gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
        HorizontalMovement();
        VerticalMovement();
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

        float moveInput = Input.GetAxisRaw("Horizontal");

        //Vector3 pivot = new Vector3(transform.position.x + hitbox.size.x / 2, transform.position.y - hitbox.size.y / 2, transform.position.z);
        //Vector3 relativePosition = transform.position - pivot;

        Debug.Log(WallTouchingDirection().y == -1);
        if (moveInput != 0 && ((isTouchingCeiling || isTouchingGround) || (Mathf.Approximately(transform.eulerAngles.z, 0f))))
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
            //Debug.Log("pivot: " + pivot);
            //Debug.Log("relative pos: " + relativePosition);
            //transform.position = pivot + relativePosition;

            // im moving left or right
            rigidBody.linearVelocity = new Vector2(moveInput * speed, rigidBody.linearVelocity.y);
            Debug.Log(rigidBody.linearVelocity);
        }
        if (moveInput == 0 && (isTouchingCeiling || isTouchingGround))
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * .9f, rigidBody.linearVelocity.y);
        }
    }

    void VerticalMovement()
    {
        int zRotationValue = 0;
        bool isTouchingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.45f, groundLayer);
        if (isTouchingWallLeft)
        {
            zRotationValue = 270;
            Debug.Log("xdd");
        }
        bool isTouchingWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.45f, groundLayer);
        if (isTouchingWallRight)
        {
            zRotationValue = 90;
        }

        float moveInput = Input.GetAxisRaw("Vertical");

        //Vector3 pivot = new Vector3(transform.position.x + hitbox.size.x / 2, transform.position.y - hitbox.size.y / 2, transform.position.z);
        //Vector3 relativePosition = transform.position - pivot;

        if (moveInput != 0 && (isTouchingWallLeft || isTouchingWallRight))
        {
            if (moveInput < 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(isTouchingWallLeft ? 0 : 180, 0, zRotationValue); // rotates right
                Debug.Log("isTouchingWallLeft: " + isTouchingWallLeft);
                Debug.Log("zRotationValue: " + zRotationValue);
            }
            else if (moveInput > 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(isTouchingWallLeft ? 180 : 0, 0, zRotationValue); // rotates right
                Debug.Log("isTouchingWallLeft: " + isTouchingWallLeft);
                Debug.Log("zRotationValue: " + zRotationValue);
            }

            //transform.position = pivot + relativePosition;

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
            Debug.Log("TOUCHING UNDER: " + transform.eulerAngles.z);
            vector.y = -1;
        }
        else if (Mathf.Approximately(transform.eulerAngles.z, 90f))
        {
            vector.x = 1;
            Debug.Log("TOUCHING RIGHT: " + transform.eulerAngles.z);
        }
        else if (Mathf.Approximately(transform.eulerAngles.z, 180f))
            vector.y = 1;
        else if (Mathf.Approximately(transform.eulerAngles.z, 270f))
        {
            Debug.Log("TOUCHING LEFT: " + transform.eulerAngles.z);
            vector.x = -1;
        }
        return vector;
    }


    void Jump()
    {
        Vector2 slimeUp = transform.up;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jumping");
            Debug.Log(SlimeTouchingDirectionLiterally().y);
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

}
