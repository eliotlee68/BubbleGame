using UnityEngine;
using UnityEngine.SceneManagement;

public class DuckController : MonoBehaviour
{
    [Header("Movement variables")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;

    [Header("Surface check")]
    public Transform platformCheck;
    public float platformCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Boundary Check")]
    public float deadzone = -30;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isInBubble;
    private bool facingLeft = true;
    private BubbleScript currentBubble;
    private float minX, maxX; // horizontal bounds for ducky
    private bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CalculateCameraBounds();
    }

    void CalculateCameraBounds()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Calculate the bounds in world coordinates
        float duckWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2; // Half the duck's width
        minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + duckWidth; // Left bound
        maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - duckWidth; // Right bound
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(platformCheck.position, platformCheckRadius, groundLayer);

        // Handle movement input
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);
        FlipLogic(moveInput);
        Jump();

        // Handle death
        if (transform.position.y < deadzone)
        {
            Die();
        }
        ClampPosition();
    }

    private void Move(float moveInput)
    {
        if (!isInBubble)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else
        {
            // Check if the current bubble is valid
            if (currentBubble != null && currentBubble.gameObject != null)
            {
                currentBubble.SetHorizontalInput(moveInput * moveSpeed);
                Debug.Log("Current Bubble: " + currentBubble.gameObject.name + " receiving " + (moveInput * moveSpeed));
            }
            else
            {
                // If the bubble is destroyed, exit the bubble state
                ExitBubble();
            }
        }
    }

    private void FlipLogic(float moveInput)
    {
        if (moveInput > 0 && facingLeft)
        {
            Flip();
        }
        else if (moveInput < 0 && !facingLeft)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isInBubble)
        {
            Debug.Log("jumpin");
            Detach();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void ClampPosition()
    {
        // Get the duck's current position
        Vector3 position = transform.position;

        // Clamp the X position to the camera bounds
        position.x = Mathf.Clamp(position.x, minX, maxX);

        // Update the duck's position
        transform.position = position;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Detach();
            //Debug.Log("detached");
        }
    }

    private void Detach()
    {
        Debug.Log("Detached");
        transform.SetParent(null);
    }

    public void EnterBubble(BubbleScript bubble)
    {
        isInBubble = true;
        currentBubble = bubble;
        rb.gravityScale = 0; // Disable gravity while in the bubble
        rb.linearVelocity = Vector2.zero; // Cancels out horizontal velocity
        Debug.Log("Current Bubble: " + currentBubble.gameObject.name);
    }

    public void ExitBubble()
    {
        isInBubble = false;
        currentBubble = null;
        transform.SetParent(null);
        rb.gravityScale = 2f;
        Debug.Log("Exited bubble");
    }

    public void Die()
    {
        isAlive = false;
        ScoringScript.Instance.enabled = false;
        SceneManager.LoadScene(2);
    }
}
