using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    public GameManagerFinalProject gameManager;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x,player.position.x);
        transform.position = cameraPosition;
    }
    
    public void ResetCamera()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = gameManager.spawnPoint.position.x;
        transform.position = newPosition;
    }
}
