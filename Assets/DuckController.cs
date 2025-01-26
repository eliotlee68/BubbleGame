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
    private bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(platformCheck.position, platformCheckRadius, groundLayer);

        // Handle movement input
        float moveInput = Input.GetAxis("Horizontal");

        // Allow movement only if not in a bubble
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

        // Handle flipping the duck
        if (moveInput > 0 && facingLeft)
        {
            Flip();
        }
        else if (moveInput < 0 && !facingLeft)
        {
            Flip();
        }

        // Handle jumping, has to be on platform and not in bubble
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isInBubble)
        {
            Debug.Log("jumpin");
            Detach();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Handle death
        if (transform.position.y < deadzone)
        {
            Die();
        }
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
