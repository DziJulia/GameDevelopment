using System;
using System.Collections;
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

    private GameManagerFinalProject gameManager;
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
    public float ballSpeed = 20f;
    public float tailP;
    private bool shouldThrow;
    public bool isThrowing;
    public int picaLife = 5;
    private Vector3 initialPosition;
    public bool firstThrow = true;
    
    void Start()
    {
        enemyAIState = State.Idle;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tailP = transform.position.y;
        initialPosition = transform.position;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }
    
    
    private void FixedUpdate()
    {
        if (_rb.Raycast(direction))
        {
            if (enemyAIState == State.AggroIdle)
            {
                speed = 0;
            }
            direction = -direction;
        }
        
        if (direction.x  > 0)
        {
            transform.localScale = new Vector3(-2, 2, 2); 
        }
        
        if (direction.x  < 0)
        {
            transform.localScale = new Vector3(2, 2, 2); 
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
                shouldThrow = true;
                if (!isThrowing && !firstThrow)
                {
                    StartCoroutine( IdleBallThrow());
                }
               
                speed = 0;
                break;
            case State.Patrol:
                shouldThrow = false;
                speed = moveSpeed;
                // move the enemy
                break;
            case State.DetectPlayer:
                shouldThrow = false;
                speed = 0;
                //when player is detected, start a timer to chase the player
                break;
            case State.Chasing:
                speed = chaseSpeed;
                break;
            case State.AggroIdle:
                shouldThrow = false;
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
    
    IEnumerator IdleBallThrow()
    {
        isThrowing = true;
        yield return new WaitForSeconds(3f);

        if (!shouldThrow)
        {
            isThrowing = false;
            yield break;
        }
        
        anim.SetBool("ball", true);
        anim.Play("Attac2");
        yield return new WaitForSeconds(1f);

        // Use the x-coordinate of Pica for the tail position
        Vector3 tailPosition = new Vector3(transform.position.x, tailP, transform.position.z);
        // Instantiate the ball at the tail position
        GameObject ball = Instantiate(ballPrefab, tailPosition, Quaternion.identity);
        // Get the Rigidbody2D component of the ball
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();

        // Apply a force to the ball in the direction Pica is facing
        ballRb.AddForce(direction * ballSpeed, ForceMode2D.Impulse);
        
        anim.SetBool("ball", false);
        isThrowing = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //player falling down landing on head
            if (other.transform.position.y > transform.position.y)
            {
                Hit();
            }
            else if (gameManager.difPower)
            {
                Hit();
            }
            else {
                gameManager.TakeDamage();
                StartCoroutine(ResetAfterDelay(3.5f));
            }
        }
    }
    
    
    public void Hit()
    {
        gameManager.playScore += 3;
        picaLife--;
        if (picaLife == 0)
        {
            gameObject.layer = LayerMask.NameToLayer("DeadPica");
            Destroy(gameObject,3f);
        }
    }
    
    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyAIState = State.Idle;
        transform.position = initialPosition;
        direction = Vector2.left;
        // to give player a chance
        firstThrow = true;
    }
}
