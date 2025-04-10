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

    SwimController swimController;

    public bool movingOverEdge = false;


    void Start()
    {
        swimController = GetComponent<SwimController>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();  // Get the Animator component if not assigned in the Inspector
        }
    }

    void Update()
    {
        Movement();
        Jump();

        bool[] hits = CheckCollisionAtPoints(GetCollisionPointsWithRotation(hitbox), hitbox, groundLayer);

        //Debug.Log("ROTATION: " + GetCollisionDirection(GetCollisionPointsWithRotation(hitbox), groundLayer, 0.02f)); // returns the vector2 of collision with rotation
        //Debug.Log("NO ROTATION: " + GetCollisionDirection(GetCollisionPointsNoRotation(hitbox), groundLayer, 0.02f)); // returns the vector2 of collision without rotation
    }
    void Movement()
    {
        Vector2 collisionDirections = GetCollisionDirection(GetCollisionPointsWithRotation(hitbox), groundLayer);
        Debug.Log("CollisionDirection: " + collisionDirections);


        if (collisionDirections == Vector2.zero)
        {
        }

        if (collisionDirections != Vector2.zero)
        {//if im colliding
            rigidBody.gravityScale = 0;

            if (collisionDirections.y != -1)
            {// if not grounded
                if (collisionDirections.x != 0)
                {
                    transform.Rotate(new Vector3(0, 0, 1), 90f);
                    transform.position += .1f * -transform.up;
                }

                if (collisionDirections.y == 1)
                {// if colliding with top side of slime
                    //transform.Rotate(new Vector3(0, 0, 1), 180f);
                    transform.rotation = new Quaternion(0, rigidBody.linearVelocityX < 0 ? 180 : 0, 180, 0);
                    transform.position += .1f * -transform.up;
                }
            }


        }
        else if (swimController.wet && collisionDirections == Vector2.zero)
        {
            transform.rotation = new Quaternion(0, rigidBody.linearVelocityX < 0 ? 180 : 0, 0, 0);
            rigidBody.gravityScale = 1;
        }
        else if (!swimController.wet && collisionDirections == Vector2.zero)
        {
            transform.rotation = new Quaternion(0, rigidBody.linearVelocityX < 0 ? 180 : 0, 0, 0);
            rigidBody.gravityScale = 3;
        }





        // else if (!isRotating && !swimController.wet)
        // {
        //     gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        //     rigidBody.gravityScale = 3;

        // }
        // else if (!isRotating)
        // {
        //     gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        // }
        // if (TouchingTiles() && !isRotating)
        // {


        //     Vector2 collision = SlimeTouchingDirection();
        //     Vector2 collisionLiterally = SlimeTouchingDirectionLiterally();
        //     // touching right only but player not facing right
        //     if (collision.x == 1 && collisionLiterally.y != -1 && collision.y == 0)
        //     {
        //         gameObject.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 90);
        //         rigidBody.linearVelocity = new Vector2(5, rigidBody.linearVelocity.y);
        //     }
        //     // touching left &&  under is not left   &&   no vertical collision
        //     else if (collision.x == -1 && collisionLiterally.y != -1 && collision.y == 0)
        //     {
        //         Debug.Log("rotating1");
        //         gameObject.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 270);
        //         rigidBody.linearVelocity = new Vector2(-5, rigidBody.linearVelocity.y);
        //     }
        //     // touching bottom only but player not facing bottom
        //     else if (collision.y == -1 && collisionLiterally.y != -1 && collision.x == 0)
        //     {
        //         gameObject.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        //         rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, -5);
        //     }
        //     //touching top      &&  under is not top       &&    no horizontal collision

        //     else if (collision.y == 1 && collisionLiterally.y != -1 && collision.x == 0)
        //     {
        //         gameObject.transform.rotation = Quaternion.Euler(0, Mathf.Approximately(transform.eulerAngles.y, 180f) ? 0 : 180, 180);
        //         rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 5);
        //     }
        //     rigidBody.gravityScale = 0;


        // }


        HorizontalMovement();
        VerticalMovement();

        Handle270Degrees();
    }
    // 
    // 270 degrees feel slow and different from normal angles
    // 270 degrees bug and go in the wall
    // decorations were clearly decoration
    // player could tell what was interactible and what wasnt. improvement: shiny, sound
    // make it more visible what is a door and what is not
    // normal movement felt fluid
    // 


    // what do you think of when you see the design?
    // Playfull and arcadey
    // can you tell the difference between interactibles and decoration?
    // Yes
    // could you tell what was the objective? how fast?
    // No, solution make door more clear and add an end point
    // what did you think of when you grabbed the collectibles?
    // Expectation was he would be able to swap elements
    // Notes: angles were not feeling good, with some bugs as mentioned above. Improvement hold only 1 key
    // Wind slime did not look like wind, Water slime looked like ice

    /*
    - what do you think of when you see the design?
    slimy. cute pixelated style, reminded me of gameboy graphics/NES. also the animal well indie game. locale looke dlike aztec something

    - can you tell the difference between interactibles and decoration?
    no, pillar looked interactible

    - could you tell what was the objective? how fast?
    no i didnt get wha tthe objective was, i ust wanted to use the lever but i didnt know why i would wanna do it.

    - what did you think of when you grabbed the collectibles?
    i thought what just happened to the slime? idk

    */

    void HorizontalMovement()
    {
        int zRotationValue = 0;

        Vector2 collisionDirections = GetCollisionDirection(GetCollisionPointsWithRotation(hitbox), groundLayer);

        bool isTouchingCeiling = (-(Vector2)transform.right == Vector2.up && collisionDirections.x == -1) || ((Vector2)transform.right == Vector2.up && collisionDirections.x == 1) || (-(Vector2)transform.up == Vector2.up && collisionDirections.y == -1);
        Debug.Log("isTouchingCeiling: " + isTouchingCeiling);
        if (isTouchingCeiling)
            zRotationValue = 180;


        bool isTouchingGround = (-(Vector2)transform.right == Vector2.down && collisionDirections.x == -1) || ((Vector2)transform.right == Vector2.down && collisionDirections.x == 1) || (-(Vector2)transform.up == Vector2.down && collisionDirections.y == -1);
        Debug.Log("isTouchingGround: " + isTouchingGround);
        if (isTouchingGround)
            zRotationValue = 0;

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0 && isTouchingCeiling && -(Vector2)transform.up != Vector2.up)
        {//if im touching ceiling but not on ceiling
            transform.Rotate(new Vector3(0, 0, 1), 90f);
            rigidBody.linearVelocity = new Vector2(moveInput * speed, 1.5f);
        }

        if (moveInput != 0 && isTouchingGround && -(Vector2)transform.up != Vector2.down)
        {//if im touching floor but not on floor
            transform.Rotate(new Vector3(0, 0, 1), 90f);
            rigidBody.linearVelocity = new Vector2(moveInput * speed, -1.5f);
        }

        if (moveInput != 0 && (isTouchingCeiling && -(Vector2)transform.up == Vector2.up || isTouchingGround && -(Vector2)transform.up == Vector2.down) || collisionDirections == Vector2.zero)
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
                rigidBody.linearVelocityY += 1f;
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
        int zRotationValue = 0;
        Vector2 collisionDirections = GetCollisionDirection(GetCollisionPointsWithRotation(hitbox), groundLayer);

        bool isTouchingWallLeft = (-(Vector2)transform.right == Vector2.left && collisionDirections.x == -1) || ((Vector2)transform.right == Vector2.left && collisionDirections.x == 1) || (-(Vector2)transform.up == Vector2.left && collisionDirections.y == -1);
        Debug.Log("IsTouchingWallLeft: " + isTouchingWallLeft);
        if (isTouchingWallLeft)
        {
            zRotationValue = 270;
        }
        bool isTouchingWallRight = (-(Vector2)transform.right == Vector2.right && collisionDirections.x == -1) || ((Vector2)transform.right == Vector2.right && collisionDirections.x == 1) || (-(Vector2)transform.up == Vector2.right && collisionDirections.y == -1);
        if (isTouchingWallRight)
        {
            zRotationValue = 90;
        }
        Debug.Log("IsTouchingWallRight: " + isTouchingWallRight);

        float moveInput = Input.GetAxisRaw("Vertical");


        if (moveInput != 0 && (isTouchingWallLeft || isTouchingWallRight))
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


    bool IsTouchingGround()
    {
        Vector2 bottomCenter = GetBottomCenter(gameObject);
        Vector2 bottomLeft = GetBottomLeft(gameObject);
        Vector2 bottomRight = GetBottomRight(gameObject);

        return Physics2D.OverlapPoint(bottomCenter, groundLayer)
            || Physics2D.OverlapPoint(bottomLeft, groundLayer)
            || Physics2D.OverlapPoint(bottomRight, groundLayer);

    }
    Vector2 SlimeTouchingDirectionLiterally()
    {
        float yOffset = (transform.localScale.y > 0) ? 0f : 1f;
        float baseY = transform.position.y - yOffset;

        Vector3 bottomCenter = new Vector3(transform.position.x, baseY, 0f);
        Vector3 bottomLeft = new Vector3(transform.position.x - 0.45f, baseY, 0f);
        Vector3 bottomLeft2 = new Vector3(transform.position.x - 0.4f, baseY, 0f);
        Vector3 bottomRight = new Vector3(transform.position.x + 0.45f, baseY, 0f);
        Vector3 bottomRight2 = new Vector3(transform.position.x + 0.4f, baseY, 0f);

        Vector2 vector = new Vector2();
        if (Physics2D.Raycast(transform.position, -transform.right, 0.45f, groundLayer))
            vector.x = -1;
        if (Physics2D.Raycast(transform.position, transform.right, 0.45f, groundLayer))
            vector.x = 1;
        if (Physics2D.Raycast(bottomCenter, -transform.up, 0.32f, groundLayer) ||
            Physics2D.Raycast(bottomLeft, -transform.up, 0.32f, groundLayer) ||
            Physics2D.Raycast(bottomLeft2, -transform.up, 0.32f, groundLayer) ||
            Physics2D.Raycast(bottomRight2, -transform.up, 0.32f, groundLayer) ||
            Physics2D.Raycast(bottomRight, -transform.up, 0.32f, groundLayer))
        {
            vector.y = -1;
        }
        if (Physics2D.Raycast(transform.position, transform.up, 0.32f, groundLayer))
            vector.y = 1;

        // Debug.DrawRay(bottomCenter, -transform.up * .32f, Color.red);
        // Debug.DrawRay(bottomLeft, -transform.up * .32f, Color.yellow);
        // Debug.DrawRay(bottomLeft2, -transform.up * .32f, Color.yellow);
        // Debug.DrawRay(bottomRight, -transform.up * .32f, Color.green);
        // Debug.DrawRay(bottomRight2, -transform.up * .32f, Color.green);

        // Debug.DrawRay(transform.position, transform.up * .32f, Color.red);

        // Debug.DrawRay(transform.position, -transform.right * .45f, Color.red);
        // Debug.DrawRay(transform.position, transform.right * .45f, Color.red);
        return vector;
    }


    Vector2[] GetCollisionPointsWithRotation(BoxCollider2D collider)
    {
        Transform t = collider.transform;

        Vector2 offset = collider.offset;
        Vector2 size = collider.size;

        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        // Local center
        Vector2 center = offset;

        // Steps for extra points
        float horizontalStep = size.x / 4f; // divides width into 4 parts
        float verticalStep = size.y / 4f;   // divides height into 4 parts

        // Bottom (y = -halfHeight)
        Vector2 bottomLeft = new Vector2(-halfWidth, -halfHeight) + offset;
        Vector2 bottomMidLeft = new Vector2(-horizontalStep, -halfHeight) + offset;
        Vector2 bottomCenter = new Vector2(0f, -halfHeight) + offset;
        Vector2 bottomMidRight = new Vector2(horizontalStep, -halfHeight) + offset;
        Vector2 bottomRight = new Vector2(halfWidth, -halfHeight) + offset;

        // Top (y = +halfHeight)
        Vector2 topLeft = new Vector2(-halfWidth, halfHeight) + offset;
        Vector2 topMidLeft = new Vector2(-horizontalStep, halfHeight) + offset;
        Vector2 topCenter = new Vector2(0f, halfHeight) + offset;
        Vector2 topMidRight = new Vector2(horizontalStep, halfHeight) + offset;
        Vector2 topRight = new Vector2(halfWidth, halfHeight) + offset;

        // Left (x = -halfWidth)
        Vector2 leftTop = new Vector2(-halfWidth, halfHeight) + offset;
        Vector2 leftMidTop = new Vector2(-halfWidth, verticalStep) + offset;
        Vector2 leftCenter = new Vector2(-halfWidth, 0f) + offset;
        Vector2 leftMidBot = new Vector2(-halfWidth, -verticalStep) + offset;
        Vector2 leftBottom = new Vector2(-halfWidth, -halfHeight) + offset;

        // Right (x = +halfWidth)
        Vector2 rightTop = new Vector2(halfWidth, halfHeight) + offset;
        Vector2 rightMidTop = new Vector2(halfWidth, verticalStep) + offset;
        Vector2 rightCenter = new Vector2(halfWidth, 0f) + offset;
        Vector2 rightMidBot = new Vector2(halfWidth, -verticalStep) + offset;
        Vector2 rightBottom = new Vector2(halfWidth, -halfHeight) + offset;

        return new Vector2[]
        {
        t.TransformPoint(bottomLeft),
        t.TransformPoint(bottomMidLeft),
        t.TransformPoint(bottomCenter),
        t.TransformPoint(bottomMidRight),
        t.TransformPoint(bottomRight),

        t.TransformPoint(topLeft),
        t.TransformPoint(topMidLeft),
        t.TransformPoint(topCenter),
        t.TransformPoint(topMidRight),
        t.TransformPoint(topRight),

        t.TransformPoint(leftTop),
        t.TransformPoint(leftMidTop),
        t.TransformPoint(leftCenter),
        t.TransformPoint(leftMidBot),
        t.TransformPoint(leftBottom),

        t.TransformPoint(rightTop),
        t.TransformPoint(rightMidTop),
        t.TransformPoint(rightCenter),
        t.TransformPoint(rightMidBot),
        t.TransformPoint(rightBottom)
        };
    }

    bool[] CheckCollisionAtPoints(Vector2[] points, BoxCollider2D collider, LayerMask groundLayer, float checkRadius = 0.02f)
    {
        bool[] hits = new bool[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            hits[i] = Physics2D.OverlapCircle(points[i], checkRadius, groundLayer);
            if (i <= 4)
            {
                Debug.DrawRay(points[i], -transform.up * checkRadius, hits[i] ? Color.green : Color.red);
            }
            else if (i <= 9)
            {
                Debug.DrawRay(points[i], transform.up * checkRadius, hits[i] ? Color.green : Color.red);
            }
            else if (i <= 14)
            {
                Debug.DrawRay(points[i], -transform.right * checkRadius, hits[i] ? Color.green : Color.red);
            }
            else
            {
                Debug.DrawRay(points[i], transform.right * checkRadius, hits[i] ? Color.green : Color.red);
            }
        }
        return hits;
    }
    Vector2 GetCollisionDirection(Vector2[] points, LayerMask groundLayer, float checkRadius = 0.02f)
    {
        bool[] hits = new bool[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            hits[i] = Physics2D.OverlapCircle(points[i], checkRadius, groundLayer);
        }


        int CountHits(int start, int end)
        {
            int count = 0;
            for (int i = start; i <= end; i++)
                if (hits[i]) count++;
            return count;
        }

        Vector2 direction = Vector2.zero;

        // Bottom: 0–4
        if (CountHits(0, 4) >= 2)
            direction += Vector2.down;

        // Top: 5–9
        if (CountHits(5, 9) >= 2)
            direction += Vector2.up;

        // Left: 10–14
        if (CountHits(10, 14) >= 2)
            direction += Vector2.left;

        // Right: 15–19
        if (CountHits(15, 19) >= 2)
            direction += Vector2.right;

        return direction;
    }


    Vector2 GetBottomCenter(GameObject obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        Transform t = collider.transform;

        Vector2 worldCenter = (Vector2)t.position + collider.offset;

        float halfHeight = collider.size.y / 2f;

        Vector2 down = -transform.up;

        Vector2 bottomCenter = worldCenter + down * halfHeight;

        return bottomCenter;
    }
    Vector2 GetBottomLeft(GameObject obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        Transform t = collider.transform;

        Vector2 worldCenter = (Vector2)t.position + collider.offset;
        float halfHeight = collider.size.y / 2f;
        float halfWidth = collider.size.x / 2f;

        Vector2 down = -t.up;
        Vector2 left = -t.right;

        return worldCenter + down * halfHeight + left * halfWidth;
    }
    Vector2 GetBottomRight(GameObject obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        Transform t = collider.transform;

        Vector2 worldCenter = (Vector2)t.position + collider.offset;
        float halfHeight = collider.size.y / 2f;
        float halfWidth = collider.size.x / 2f;

        Vector2 down = -t.up;
        Vector2 right = t.right;

        return worldCenter + down * halfHeight + right * halfWidth;
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


    void Jump()
    {
        Vector2 slimeUp = transform.up;
        if (Input.GetKeyDown(KeyCode.Space) && !movingOverEdge)
        {
            Debug.Log("Jumping");
            if (GetCollisionDirection(GetCollisionPointsWithRotation(hitbox), groundLayer).y == -1)
            {
                Debug.Log("Touching Ground:" + slimeUp * jumpForce);
                rigidBody.linearVelocity += slimeUp * jumpForce;
            }
        }
    }


    void Handle270Degrees()
    {
        Vector3 bottomCenter = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 bottomLeft = new Vector3(transform.position.x - transform.right.x * 0.45f, transform.position.y, 0f);
        Vector3 bottomLeft2 = new Vector3(transform.position.x - transform.right.x * 0.3f, transform.position.y, 0f);
        Vector3 bottomLeft3 = new Vector3(transform.position.x - transform.right.x * 0.35f, transform.position.y, 0f);
        Vector3 bottomRight = new Vector3(transform.position.x + transform.right.x * 0.45f, transform.position.y, 0f);
        Vector3 bottomRight2 = new Vector3(transform.position.x + transform.right.x * 0.3f, transform.position.y, 0f);

        bool center = Physics2D.Raycast(bottomCenter, -transform.up, 0.32f, groundLayer);
        bool back = Physics2D.Raycast(bottomLeft, -transform.up, 0.32f, groundLayer);
        bool back2 = Physics2D.Raycast(bottomLeft2, -transform.up, 0.32f, groundLayer);
        bool back3 = Physics2D.Raycast(bottomLeft3, -transform.up, 0.32f, groundLayer);
        bool front = Physics2D.Raycast(bottomRight, -transform.up, 0.32f, groundLayer);
        bool front2 = Physics2D.Raycast(bottomRight2, -transform.up, 0.32f, groundLayer);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (back && back3 && !back2 && !front && !front2 && !center && (input.x != 0 || input.y != 0))
        {

            isRotating = true;


            float grid = 1f;
            float newX = 0f;

            Vector2 direction = SlimeTouchingDirection();
            if (direction.x != 0 && input.y == 0) // if on vertical wall and not pressing vertical movement
                return;

            if (direction.y != 0 && input.x == 0) // if on horizontal wall and not pressing vertical movement
                return;


            Vector2 directionToGo = transform.forward;
            if (input.x > 0)
            {
                newX = Mathf.Ceil(transform.position.x / grid) * grid;
                StartCoroutine(MoveEdgeOverTime(new Vector2(newX + hitbox.size.x / 2 - .25f - transform.position.x, direction.y / 2 * 1.1f)));
            }
            else if (input.x < 0)
            {
                newX = Mathf.Floor((float)Math.Floor(transform.position.x) / grid) * grid;
                StartCoroutine(MoveEdgeOverTime(new Vector2(newX - hitbox.size.y / 2 + .1f - transform.position.x, direction.y / 2 * 1.1f)));
            }
            else
            {
                newX = Mathf.Round((float)Math.Floor(transform.position.x) / grid) * grid;
                StartCoroutine(MoveEdgeOverTime(new Vector2(newX - transform.position.x, direction.y / 2 * 1.2f)));
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
        while (elapsedTime < 0.2f)
        {
            GetComponent<Collider2D>().isTrigger = true;
            float fraction = elapsedTime / 0.2f;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        elapsedTime = 0f;

        while (elapsedTime < 0.1f)
        {

            float fraction = elapsedTime / 0.1f;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;

        elapsedTime = 0f;

        while (elapsedTime < 0.1f)
        {
            float fraction = elapsedTime / 0.1f;
            transform.position = Vector3.Lerp(targetPosition, targetPosition2, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition2;


        transform.position = targetPosition2;
        GetComponent<Collider2D>().isTrigger = false;


        yield return new WaitForSeconds(.1f);

        isRotating = false;

    }
}
