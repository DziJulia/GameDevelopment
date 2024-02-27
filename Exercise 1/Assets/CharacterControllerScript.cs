using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float maxSpeed;

    public float accelartion;

    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;

    public float secondaryJumpForce;
    public float secondaryJumTime;

    public Animator anim;

    public bool secondaryJump;
    // Start is called before the first frame update
    void Start()
    {
        // looking for component with rigid body we can import or tag it here 
        myRb = GetComponent<Rigidbody2D>(); // look for component called Rigide Body 2D
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
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

        //For jump code
        if (isGrounded && Input.GetButton("Jump"))
        {
            // add force in the y direction
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            // Add backgorund  threads
            StartCoroutine(SecondaryJump());
        }

        if (isGrounded == false && Input.GetButton("Jump"))
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
    }

    // when the collider exist the trigger the player is no longer grounded
    private void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }

    IEnumerator SecondaryJump()
    {
        secondaryJump = true;
        //how long i allow it to wait which i will set to add littlebit of force to the jump
        yield return new WaitForSeconds(secondaryJumTime);
        secondaryJump = false;
       // yield return null;
    }
}
