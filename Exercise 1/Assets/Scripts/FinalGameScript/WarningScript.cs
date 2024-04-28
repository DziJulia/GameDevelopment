using UnityEngine;

public class WarningScript : MonoBehaviour
{
    public CameraScript cameraScript;
    private int count = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player") && count == 0)
        {
            count++;
            // Call the TriggerWarning method
            cameraScript.TriggerWarning();
        }
    }
}