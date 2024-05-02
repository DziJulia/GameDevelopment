 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    public float stopPositionX = 301.4255f;
    public GameManagerFinalProject gameManager;
    private float initialX;
    private float initialZ;
    public GameObject pikachu;
    public bool warningTriggered = false;
    public FinalPlayerControllerScript playerMovement;
    private bool returningToPlayer = false; // Flag to check if the camera is returning to the player
    public bool cameraSet = false;
    public int moveCount = 0;
    public Pica pica;
    public GameObject healthBar;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
        playerMovement = player.GetComponent<FinalPlayerControllerScript>();
        initialX = transform.position.x; 
        initialZ = transform.position.z; 
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;

        if (warningTriggered)
        {
            // Smoothly move the camera to the right to show Pikachu
            cameraPosition = Vector3.Lerp(cameraPosition, pikachu.transform.position, Time.deltaTime);
            cameraPosition.z = initialZ;
            if (Mathf.Abs(cameraPosition.x - pikachu.transform.position.x) < 4f)
            {
                // If the camera has reached Pikachu, start returning to the player
                returningToPlayer = true;
                warningTriggered = false;
            }
        }
        else if (returningToPlayer)
        {
            cameraPosition = Vector3.Lerp(cameraPosition, player.position, Time.deltaTime);
            cameraPosition.z = initialZ; // Keep the Z position constant
            if (Mathf.Abs(cameraPosition.x - player.position.x) < 10f)
            {
                // If the camera has reached the player, stop returning to the player
                returningToPlayer = false;
                playerMovement.canMove = true;
                cameraPosition.z = initialZ;
            }
        }
        else if (cameraPosition.x >= stopPositionX && playerMovement.canMove)
        {
            cameraSet = true;
            SetCameraPositionAndZoom();
        }
        else if(!cameraSet)
        {
            // Normal camera follow logic
            cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x, initialX);
            GameObject tileMap = GameObject.FindWithTag("TileMap");
            if (tileMap != null)
            {
                float minY = tileMap.GetComponent<Renderer>().bounds.min.y;
                float offset = 6f;
                if (player.position.y > minY + offset)
                {
                    cameraPosition.y = player.position.y;
                }
                else
                {
                    cameraPosition.y = Mathf.Max(minY + offset, cameraPosition.y);
                }
            }
        }

        transform.position = cameraPosition;
    }

    public void TriggerWarning()
    {
        // Trigger the warning and move the camera to Pikachu
        warningTriggered = true;
        playerMovement.canMove = false; // Disable player movement
        pica.firstThrow = false;
        healthBar.SetActive(true);
    }

    public void SetCameraPositionAndZoom()
    {
        // Set the camera's position
        Vector3 newPosition = new Vector3(311.2f, 5.830172f, initialZ);
        transform.position = newPosition;

        // Set the camera's zoom level
        Camera.main.orthographicSize = 5.32f;
    }

    public void ResetCamera()
    {
        Vector3 newPosition = transform.position;
        moveCount = 0;
        newPosition.x = gameManager.spawnPoint.position.x;
        transform.position = newPosition;
        cameraSet = false;
        Camera.main.orthographicSize = 3.632f; // Reset zoom
    }
}
