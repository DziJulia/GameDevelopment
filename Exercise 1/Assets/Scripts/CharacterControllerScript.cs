using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterControllerScript : MonoBehaviour
{
    public float maxSpeed;
    public float accelartion;
    public GameManager gameManager;
    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;
    public bool isHit;
    public int jumpCount;
    public float secondaryJumpForce;
    public float secondaryJumTime;
    public float wallJumpForce;
    public float wallJumpHorizontalForce;
    public LayerMask wallLayer;
    public float wallDetectionDistance;

    public Animator anim;

    public bool secondaryJump;
    // Start is called before the first frame update
    void Start()
    {
        // looking for component with rigid body we can import or tag it here 
        myRb = GetComponent<Rigidbody2D>(); // look for component called Rigide Body 2D
        anim = GetComponentInChildren<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        jumpCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // sets the speed parameter in the animator to the absolute value of the player's x velocity
        anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x));

        if (Input.GetAxis("Horizontal") > 0.1f) //if the player moving to right
        {
            anim.transform.localScale = new Vector3(1, 1, 1); //set the scale of the player to 1,1,1
        }
        
        if (Input.GetAxis("Horizontal") < -0.1f) //if the player moving to left
        {
            anim.transform.localScale = new Vector3(-1, 1, 1); //set the scale of the player to 1,1,1
        }
        //if the absolute value of the input is greater than 0:1 and playr is not moving faster
        // we adding Mathf.Abs to do absolute value to check right and left
        // second statement we are checking about the speed that the character is not moving faster
        // than maximum speed
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && Mathf.Abs(myRb.velocity.x) < maxSpeed)
        {
            // gets Input valuea nd multiplies it by accelaration int x direction
            myRb.AddForce(new Vector2(Input.GetAxis("Horizontal"),0)* accelartion, ForceMode2D.Force);
        }
    }

    void Update()
    {
        //For fall code
        anim.SetFloat("jumping", jumpCount);
        
        //For jump code
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // add force in the y direction
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            // Add backgorund  threads
            StartCoroutine(SecondaryJump());
            jumpCount++;
        }

        if (isGrounded == false && Input.GetButtonDown("Jump"))
        {
            // while button is held, add a force in y direetion
            myRb.AddForce(new Vector2(0, secondaryJumpForce), ForceMode2D.Force);
        }
        
        //end of jump code
    }
    // as long as collider is detected inside the trigger the player is grounded
    // to be more forgiving you can make it bigger
    private void OnTriggerStay2D(Collider2D other)
    {
        isGrounded = true;
        jumpCount = 0;
    }

    // when the collider exist the trigger the player is no longer grounded
    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
        anim.SetBool("isHit", false);
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Trap"))
        {
            anim.SetBool("isHit", true);
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

    IEnumerator SecondaryJump()
    {
        secondaryJump = true;
        //how long i allow it to wait which i will set to add littlebit of force to the jump
        yield return new WaitForSeconds(secondaryJumTime);
        secondaryJump = false;
       // yield return null;
    }
    
    // Coroutine for health decrement
    IEnumerator HealthDecrementCoroutine()
    {
        while (true) // This will run indefinitely until stopped
        {
            if (gameManager.playerhealth > 0)
            {
                gameManager.playerhealth -= 1;
            }
            else
            {
                // Health is 0 or less, stop the coroutine and handle game over logic here
                yield break;
            }
            yield return new WaitForSeconds(2f); // Wait for 1 second before next iteration
        }
    }
}
