using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool playerOnPlatform;
    private Vector3 originalPosition;
    public bool platformMoved;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Attach the player to the platform
            collision.transform.SetParent(transform);
            playerOnPlatform = true;
            // Store the original position of the platform
            originalPosition = transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detach the player from the platform
            collision.transform.SetParent(null);
            playerOnPlatform = false;
        }
    }

    private void FixedUpdate()
    {
        if (playerOnPlatform)
        {
            platformMoved = true;
            // Move the platform to the right
            transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TileMap"))
        {
            Debug.Log("Hit TIleMap");
            // Stop the platform if colliding with the stopper
            moveSpeed = 0f;
        }
    }
    
    public void ResetPlatform()
    {
        // Check if the platform has moved from its original position
        if (platformMoved)
        {
            // Reset the platform to the original position
            transform.position = originalPosition;
            // If you want the platform to start moving again, reset the speed
            moveSpeed = 5f;
        }
    }
}