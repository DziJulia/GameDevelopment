using UnityEngine;

public class MovingDownPlatform : MonoBehaviour
{
    public float speed = 1.0f; // Speed of the platform's movement
    public float distance = 10.0f; // Distance to move up

    private Vector2 startPosition; // Starting position of the platform
    private bool playerOnPlatform = false;
    private bool movingUp = false;

    void Start()
    {
        // Save the starting position
        startPosition = transform.position;
    }

    void Update()
    {
        if (playerOnPlatform && !movingUp)
        {
            // Move the platform up
            Vector2 targetPosition = new Vector2(transform.position.x, startPosition.y + distance);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if the platform has reached the target position
            if (transform.position.y >= targetPosition.y)
            {
                movingUp = true;
            }
        }
        else if (!playerOnPlatform && movingUp)
        {
            // Move the platform back to the starting position
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

            // Check if the platform has reached the starting position
            if (transform.position.y <= startPosition.y)
            {
                movingUp = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }
}
