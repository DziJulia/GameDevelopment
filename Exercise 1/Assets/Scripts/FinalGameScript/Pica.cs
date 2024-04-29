using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pica : MonoBehaviour
{
    // Start is called before the first frame update
    public enum State
    {
        Idle,
        Patrol,
        DetectPlayer,
        Chasing,
        AggroIdle,
    }

    public State enemyAIState;
    public float moveSpeed;
    public Vector2 direction = Vector2.left;
    //speed of the enemy when chasing the player
    public float chaseSpeed; 
    private float speed;
    //Time to enemy will stay in detect mode before beginning chasing player
    public float detectedPlayerTime;
    //used if player is out of detection radius enemy will stay in aggro mode for fir this time, and can immediately resume chasing before going back to idle
    public float aggroTime;
    public Animator anim;
    public bool aggro;
    public bool playerDetected;
    private Rigidbody2D _rb;
    public GameObject ballPrefab;
    public Transform tailPosition;
    public float ballSpeed = 20f; 
    
    void Start()
    {
        enemyAIState = State.Idle;
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_rb.Raycast(direction))
        {
            Debug.Log("Change to oposite");
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
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        anim.SetFloat("speed", speed);
        direction.y = 0;
        _rb.velocity = direction * speed;

        switch (enemyAIState)
        {
            case State.Idle:
                //do nothing
                speed = 0;
                break;
            case State.Patrol:
                speed = moveSpeed;
                // move the enemy
                break;
            case State.DetectPlayer:
                speed = 0;
                //when player is detected, start a timer to chase the player
                break;
            case State.Chasing:
                speed = chaseSpeed;
                break;
            case State.AggroIdle:
                // speed = 0;
                //stayes in aggro mode for a set time before going back to idle
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerDetected = true;
            if (aggro == false)
            {
                //need to stop the Coroutine in case it was previously started e.g. if the player quickly enters and exits the detection radius
                StopCoroutine("DetectTimer"); 
                StartCoroutine("DetectTimer");
            }

            if (aggro == true)
            {
                playerDetected = true;
                enemyAIState = State.Chasing;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            if (aggro == true)
            {
                StopCoroutine("AggroTimer");
                StartCoroutine("AggroTimer");
            }
        }
    }

    IEnumerator DetectTimer()
    {
        enemyAIState = State.DetectPlayer;
        yield return new WaitForSeconds(detectedPlayerTime);
        if (playerDetected == true)
        {
            aggro = true;
            enemyAIState = State.Chasing;
        }
        if (playerDetected == false)
        {
            aggro = false;
            enemyAIState = State.Idle;
        }
    }

    IEnumerator AggroTimer()
    {
        yield return new WaitForSeconds(aggroTime);
        if (playerDetected == false & aggro == false)
        {
            aggro = false;
            enemyAIState = State.Idle;
        }
        if (playerDetected == false & aggro == true)
        {
            enemyAIState = State.AggroIdle;
        }
        yield return new WaitForSeconds(aggroTime*2);
        if (playerDetected == false)
        {
            aggro = false;
            enemyAIState = State.Idle;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a TileMap
        if (collision.gameObject.CompareTag("TileMap") & enemyAIState == State.AggroIdle)
        {
            speed = 0;
        }
    }

}
