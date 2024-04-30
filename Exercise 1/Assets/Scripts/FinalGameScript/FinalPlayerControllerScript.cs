using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    private new Camera camera;
    public ParticleSystem particleSystem;
    public SpriteRenderer spriteBlueRenderer;
    public SpriteRenderer spriteYellowRenderer;
    public SpriteRenderer spritePurpleRenderer;
    public Animator animPurple;
    public Animator animYellow;
    public Animator animBlue;
    public Pica picachu;

    private void Awake()
    {
        camera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        // looking for component with rigid body we can import or tag it here 
        myRb = GetComponent<Rigidbody2D>(); // look for component called Rigide Body 2D
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
        jumpCount = 0;
        animBlue = anim;
        particleSystem.Stop();
        picachu = GameObject.FindGameObjectWithTag("Pikatchu").GetComponent<Pica>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        
        
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
                float clampedPositionX = Mathf.Clamp(myRb.position.x, leftEdge.x, rightEdge.x);
                myRb.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * acceleration, ForceMode2D.Force);
                myRb.position = new Vector2(clampedPositionX, myRb.position.y);
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
        
        if (isGrounded)
        {
            anim.ResetTrigger("isJumping");
            anim.ResetTrigger("Land");
        }
        
        if (!gameManager.isPaused)
        {
            if (isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
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
                StartCoroutine(DelayedLandTrigger());
            }
        }
    }

    void Jump(float jumpForceFloat)
    {
        anim.SetTrigger("isJumping");
        StartCoroutine(DelayedLandTrigger());
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
        
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy") && !gameManager.difPower)
        {
            if (myRb.velocity.y <= 0 || transform.DotTest(coll.transform, Vector2.down))
            {
                // myRb.velocity = new Vector2(0, jumpForce / 2f);  
                myRb.velocity = new Vector2(0, 15);
                anim.SetTrigger("isJumping");
                StartCoroutine(DelayedLandTrigger());
                picachu.Hit();
            }
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
                gameManager.TakeDamage();
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
    }
    
    // Function to restart the game
    void RestartGame()
    {
        particleSystem.Stop();
        gameManager.playerHealth = 3;
        gameManager.playScore = 0;
        gameManager.difPower = false;
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

    public void Death()
    {
        DisablePhysics();
        if (gameManager.playerHealth > 0)
        {
            StartCoroutine(AnimateDead());
        }
        
        Invoke("ResetFromDead", 3f);
    }
    
    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        GetComponent<Rigidbody2D>().isKinematic = true;
        spriteYellowRenderer.enabled = false;
        spritePurpleRenderer.enabled = false;
        anim.transform.localScale = new Vector3(1,1,1);
        anim = GetComponentInChildren<Animator>();
        spriteBlueRenderer.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
    public void ResetFromDead()
    {
        anim = animBlue;
        myRb.velocity = Vector2.zero;
        myRb.angularVelocity = 0f;
        jumpCount = 0;
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = true;
        }

        GetComponent<Rigidbody2D>().isKinematic = false;
        Vector3 newPosition = gameManager.spawnPoint.position + new Vector3(0f, 3f, 0f);
        
        myRb.transform.position = newPosition;
        gameManager.difPower = false;
        // Reset camera position
        Camera.main.GetComponent<CameraScript>().ResetCamera();
    }
    
    private IEnumerator AnimateDead()
    {
        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector3.up * jumpVelocity;

        while (elapsed < duration)
        {
            gameObject.layer = LayerMask.NameToLayer("YellowNinja");
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void DifPower(float duration = 10f)
    {
       StartCoroutine(StartDifPower(duration));
    }

    private IEnumerator StartDifPower(float duration)
    {
        gameManager.difPower = true;
        Debug.Log("POWER Courutine started");
        float elapsed = 0f;
        particleSystem.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                spriteBlueRenderer.enabled = false;
                spritePurpleRenderer.enabled = false;
                spriteYellowRenderer.enabled = true;
                gameObject.layer = LayerMask.NameToLayer("YellowNinja");
                anim = animYellow;
            }

            yield return null;
        }

        spriteYellowRenderer.enabled = false;
        particleSystem.Stop();
        if (gameManager.hasBeenPickedUp)
        {
            spritePurpleRenderer.enabled = true;
            anim = animPurple;
        }
        else
        {
            spriteBlueRenderer.enabled = true;
            anim = animBlue;
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
        
        gameManager.difPower = false;
    }
    
    private IEnumerator DelayedLandTrigger()
    {
        // Wait for one second
        yield return new WaitForSeconds(0.1f);

        // Set the "Land" trigger and reset the "isJumping" trigger
        anim.SetTrigger("Land");
        anim.ResetTrigger("isJumping");
    }
}
