using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public Vector2 direction = Vector2.left;
    public float speed = 1f;
    private Vector2 velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        
        if (rigidbody.Raycast(direction))
        {
            direction = -direction;
        }

        if (direction.x  > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        
        if (direction.x  < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }


        if (rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }
}
