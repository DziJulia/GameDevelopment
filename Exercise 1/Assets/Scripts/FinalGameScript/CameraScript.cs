using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    public GameManagerFinalProject gameManager;
    private float initialX;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
        initialX = transform.position.x; 
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x, initialX);

        // Get the TileMap object
        GameObject tileMap = GameObject.FindWithTag("TileMap");

        // Check if the TileMap exists
        if (tileMap != null)
        {
            // Get the bottom y position of the TileMap
            float minY = tileMap.GetComponent<Renderer>().bounds.min.y;

            // Add a small offset to the y position
            float offset = 6f; // Change this value to whatever offset you want

            // Update the y position of the camera only if the player's y position is higher than minY + offset
            if (player.position.y > minY + offset)
            {
                cameraPosition.y = player.position.y;
            }
            else
            {
                cameraPosition.y = Mathf.Max(minY + offset, cameraPosition.y);
            }
        }

        transform.position = cameraPosition;
    }


    
    public void ResetCamera()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = gameManager.spawnPoint.position.x;
        transform.position = newPosition;
    }
}
