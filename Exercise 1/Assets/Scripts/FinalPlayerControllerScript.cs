using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPlayerControllerScript : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public GameManagerFinalProject gameManager;
    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;
    public int jumpCount;
    public int maxJumpCount = 2;
    public Animator anim;
    public bool canMove = true;
    
    // Start is called before the first frame update
    void Start()
    {
        // looking for component with rigid body we can import or tag it here 
        myRb = GetComponent<Rigidbody2D>(); // look for component called Rigide Body 2D
        anim = GetComponentInChildren<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
        jumpCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        
        if (!gameManager.isPaused)
        {
            // sets the speed parameter in the animator to the absolute value of the player's x velocity
            anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x));

            //if the player moving to right
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                anim.transform.localScale = new Vector3(1, 1, 1);
            }

            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                anim.transform.localScale = new Vector3(-1, 1, 1);
            }

            //if the absolute value of the input is greater than 0:1 and playr is not moving faster
            // we adding Mathf.Abs to do absolute value to check right and left
            // second statement we are checking about the speed that the character is not moving faster
            // than maximum speed
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && Mathf.Abs(myRb.velocity.x) < maxSpeed)
            {
                myRb.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * acceleration, ForceMode2D.Force);
            }
        }
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }
        
        anim.SetFloat("life", gameManager.playerHealth);
        // Pause the game when "P" key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
            
        // Restart the game when "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        
        if (!gameManager.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.SetTrigger("isCrouching");
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.ResetTrigger("isCrouching");
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                Jump(jumpForce);
            }

            if (!isGrounded && jumpCount < maxJumpCount && Input.GetButtonDown("Jump"))
            {
                Jump(jumpForce - 5);
            }

            if (!isGrounded && Input.GetButtonUp("Jump"))
            {
                // Trigger landing animation when releasing the jump button
                anim.ResetTrigger("isJumping");
                anim.SetTrigger("Land");
            }

            if (isGrounded)
            {
                anim.ResetTrigger("Land");
            }
        }
    }

    void Jump(float jumpForceFloat)
    {
        Debug.Log("Jump Trigger Set");
        anim.SetTrigger("isJumping");
        myRb.velocity = new Vector2(myRb.velocity.x, 0);
        myRb.AddForce(Vector2.up * jumpForceFloat, ForceMode2D.Impulse);
        jumpCount++;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        isGrounded = true;
        jumpCount = 0;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Trap"))
        {
            StopCoroutine("HealthDecrementCoroutine");
            StartCoroutine("HealthDecrementCoroutine"); 
        }
    }
    
    public void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Trap"))
        {
            StopCoroutine("HealthDecrementCoroutine");
        }
    }
    
    
    // Coroutine for health decrement
    IEnumerator HealthDecrementCoroutine()
    {
        // This will run indefinitely until stopped
        while (true)
        {
            if (gameManager.playerHealth > 0)
            {
                gameManager.playerHealth -= 1;
            }
            else
            {
                yield break;
            }
            yield return new WaitForSeconds(2f);
        }
    }
    
    void TogglePause()
    {
        gameManager.isPaused = !gameManager.isPaused;

        // Pause or resume game based on the current pause state
        Time.timeScale = gameManager.isPaused ? 0 : 1;

        // You might want to add UI elements or logic to indicate that the game is paused
    }
    
    // Function to restart the game
    void RestartGame()
    {
        gameManager.playerHealth = 5;
        gameManager.playScore = 0;
        // Reload the currently active scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        // Reset any other necessary game state
        gameManager.isPaused = false;
        Time.timeScale = 1;
    }
    
    public void PlayAnimation(AnimationClip clip)
    {
        // Play the specified animation clip
        anim.Play(clip.name);
    }
}
