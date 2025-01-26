using UnityEngine;

public class DuckController : MonoBehaviour
{
    [Header("Movement variables")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    [Header("Surface check")]
    public Transform platformCheck;
    public float platformCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingLeft = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(platformCheck.position, Vector2.down * platformCheckRadius, Color.red);
        isGrounded = Physics2D.OverlapCircle(platformCheck.position, platformCheckRadius, groundLayer);

        // Handle movement input
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Handle flipping the duck
        if (moveInput > 0 && facingLeft)
        {
            Flip();
        }
        else if (moveInput < 0 && !facingLeft)
        {
            Flip();
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("jumpin");
            Detach();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
            Debug.Log("detached");
        }
    }

    private void Detach()
    {
        Debug.Log("Detached");
        transform.SetParent(null);
    }
}
