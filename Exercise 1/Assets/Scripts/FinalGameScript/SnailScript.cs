using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnailScript : MonoBehaviour
{
    public float shellSpeed = 7;
    public Sprite shellSprite;
    public GameManagerFinalProject gameManager;
    private bool shelled;
    private bool shellMoving = false;
    private HashSet<GameObject> processedObjects = new HashSet<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!shelled && other.gameObject.CompareTag("Player") && !gameManager.difPower)
        {
            float playerHeight = other.collider.bounds.size.y;
            float playerPosition = other.transform.position.y - playerHeight / 2;
            
            if (playerPosition > transform.position.y)
            {
                Debug.Log("Hit From TOP ");
                EnterShell();
                gameManager.playScore++;
            }
            else {
                Debug.Log("EnterNoShell collision take damage");
                gameManager.TakeDamage();
            }
        } else if ( other.gameObject.CompareTag("Player") && gameManager.difPower)
        {
            Debug.Log("gameManager.difPower" + gameManager.difPower);
            // If the collider belongs to the player and the shell is moving,
            // let the shell pass through without pushing the player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider, true);
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (processedObjects.Contains(other.gameObject))
        {
            return;
        }
        
        // Add the GameObject to the set of processed objects
        processedObjects.Add(other.gameObject);
        
        if (shelled && other.CompareTag("Player"))
        {
            if (!shellMoving)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else {
                gameManager.TakeDamage();
            }
        }
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (processedObjects.Contains(other.gameObject))
        {
            // This GameObject has already been processed and is now exiting the trigger, so we remove it from the set
            processedObjects.Remove(other.gameObject);
        }
    }

    private void EnterShell()
    {
        shelled = true;
        
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void PushShell(Vector2 direction)
    {
        shellMoving = true;

        GetComponent<Rigidbody2D>().isKinematic = false;
        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }
    
    private void Hit()
    {
        GetComponent<DeadAnimationScript>().enabled = true;
        gameManager.playScore += 3;
        Destroy(gameObject,3f);
    }

    private void OnBecameInvisible()
    {
        if (shelled)
        {
            Destroy(gameObject);
        }
    }
}

