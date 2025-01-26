using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public float riseSpeed = 1f; // Speed at which the bubble rises
    public float bubbleDuration = 2.5f; // Time before the bubble pops
    public Transform duck; // Reference to the duck
    public float bubbleVelMult = 0.9f; // Modifies duck speed while in bubble
    public float bubbleRiseMult = 0.9f;

    private bool isDuckInside = false;
    private float timer = 0f;
    private float horizontalInput = 0f;
    private float deadzone = 29;

    void Update()
    {
        // Move the bubble upward
        if (!isDuckInside)
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            
        }
        else
        {
            // Move the bubble and duck upward together
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime * bubbleRiseMult);

            // Move the bubble and duck horizontally based on input
            float moveAmount = horizontalInput * Time.deltaTime * bubbleVelMult; // Adjust speed as needed
            //Debug.Log(moveAmount);
            transform.Translate(Vector3.right * moveAmount);

            // Update the duck's position to stay inside the bubble
            if (duck != null)
            {
                duck.position = new Vector3(transform.position.x, transform.position.y, duck.position.z);
            }

            // Count down the bubble duration
            timer += Time.deltaTime;
            if (timer >= bubbleDuration)
            {
                PopBubble();
            }
        }

        //Destroy bubble if above screen
        if (transform.position.y > deadzone)
        {
            PopBubble();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the duck enters the bubble
        if (collision.CompareTag("Player") && !isDuckInside)
        {
            Debug.Log("Bubble entered");
            isDuckInside = true;
            duck = collision.transform; // Set the duck reference
            duck.SetParent(transform); // Make the duck a child of the bubble

            //Notify DuckController that bubble has been triggered
            DuckController duckController = duck.GetComponent<DuckController>();
            if (duckController != null)
            {
                duckController.EnterBubble(this);
            }
        }

        // Check if bubble collides with another bubble when duck is inside
        if (collision.CompareTag("Bubble") && isDuckInside)
        {
            Debug.Log("Collision detected between bubbles");
            BubbleScript otherBubble = collision.GetComponent<BubbleScript>();
            if (otherBubble != null && !otherBubble.isDuckInside)
            {
                Debug.Log("Transferring duck to new bubble");
                TransferDuckToBubble(otherBubble);
            }
        }
    }
    
    private void TransferDuckToBubble(BubbleScript newBubble)
    {
        if (duck != null)
        {

            // Transfer the duck to the new bubble
            newBubble.CaptureDuck(duck);
            PopBubble();
        }
    }

    public void CaptureDuck(Transform newDuck)
    {
        isDuckInside = true;
        duck = newDuck; // Set the duck reference

        if (duck != null)
        {
            duck.SetParent(this.transform); // Make the duck a child of the new bubble

            // Notify DuckController that bubble has been triggered
            DuckController duckController = duck.GetComponent<DuckController>();
            if (duckController != null)
            {
                duckController.EnterBubble(this); // Pass the new bubble reference
            }
        }
    }

    private void PopBubble()
    {
        // Release the duck
        if (duck != null && duck.parent == transform)
        {
            duck.SetParent(null); // Unparent the duck
            DuckController duckController = duck.GetComponent<DuckController>();
            if (duckController != null)
            {
                duckController.ExitBubble();
            }
        }

        duck = null;

        // Destroy the bubble
        Destroy(gameObject);
    }
    public void SetHorizontalInput(float input)
    {
        horizontalInput = input; // Store the horizontal input from the duck
    }
}
