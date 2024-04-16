using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;
    // Timer for the blink animation
    private float blinkTimer = 0f;
    // Interval for the blink animation (5 seconds)
    private float blinkInterval = 5f;
    public float damageValue;
    public GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get CharacterControllerScript component of the player
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase the timer by the time since the last frame
        blinkTimer += Time.deltaTime;

        // If 10 seconds have passed
        // If the timer has reached the interval
        if (blinkTimer >= blinkInterval)
        {
            // Set the Blink parameter to true
            animator.SetBool("Blink", true);
            // Reset the timer
            blinkTimer = 0f;
        }
    }
    public void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Player"))
        {
            Vector2 collision = coll.contacts[0].normal;

            if (collision.y > 0.5) {
                Debug.Log("hit bottom");
                animator.SetBool("HitBottomTop", true);
                animator.SetBool("Blink", false);
            } else if (collision.y < -0.5) {
                Debug.Log("hit top");
                animator.SetBool("HitBottomTop", true);
                animator.SetBool("Blink", false);
            } else if (collision.x > 0.5) {
                Debug.Log("hit left");
                animator.SetBool("HitLeftRight", true);
                animator.SetBool("Blink", false);
            } else if (collision.x < -0.5) {
                Debug.Log("hit right");
                animator.SetBool("HitLeftRight", true);
                animator.SetBool("Blink", false);
            }
        }
    }
    
    public void ResetHitLeft() {
        animator.SetBool("HitLeftRight", false);
        animator.SetBool("HitBottomTop", false);
    }
    
    public void ResetBLink() {
        animator.SetBool("Blink", false);
    }
}
