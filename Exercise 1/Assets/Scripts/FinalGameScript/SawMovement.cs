using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    public float speed = 2f; // Speed of the saw movement
    public float distance = 2f; // Distance the saw will move
    public Vector3 direction = Vector3.right; // Direction of the saw movement

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate new position based on a sine wave
        float movement = Mathf.PingPong(Time.time * speed, distance);
        Vector3 newPosition = startPosition + direction * movement;

        // Update the position of the saw
        transform.position = newPosition;
    }
}