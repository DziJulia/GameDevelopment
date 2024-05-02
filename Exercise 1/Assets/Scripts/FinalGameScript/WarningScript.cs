using UnityEngine;

public class WarningScript : MonoBehaviour
{
    public CameraScript cameraScript;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.gameObject.CompareTag("Player") && cameraScript.moveCount == 0)
        {
            cameraScript.moveCount++;
            // Call the TriggerWarning method
            cameraScript.TriggerWarning();
        }
    }
}