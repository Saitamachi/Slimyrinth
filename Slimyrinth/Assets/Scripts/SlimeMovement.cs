using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float speed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private bool isGrounded;
    void Start()
    {

    }

    void Update()
    {
        Movement();
        CheckGround();
        Jump();
    }
    void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rigidBody.linearVelocity = new Vector2(moveInput * speed, rigidBody.linearVelocity.y);
        groundCheck.transform.position = gameObject.transform.position + new Vector3(0, -0.4f, 0);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, groundLayer);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        }
    }
}
