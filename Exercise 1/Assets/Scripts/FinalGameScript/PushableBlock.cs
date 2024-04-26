using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    public float pushForce = 9.0f;
    private bool isColliding = false;
    private Vector2 pushDirection;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = true;
            // Calculate push direction
            pushDirection = (collision.transform.position - transform.position).normalized;
            Debug.Log("Collision with " + collision.gameObject.name);
            Debug.Log("Push direction: " + pushDirection);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
        }
    }

    private void FixedUpdate()
    {
        if (isColliding)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("Push force: " + (-pushDirection * pushForce));
                // Apply opposite push force to the block
                rb.AddForce(-pushDirection * pushForce, ForceMode2D.Force);
            }
        }
    }
}