using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public GameManagerFinalProject gameManager;
    public GameObject pickUpEffect;
    public FinalPlayerControllerScript player;
    public AnimationClip pickUpAnimation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalPlayerControllerScript>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        //checks if the object is picked up by the player
        if (col.gameObject.CompareTag("Player"))
        {
            //initiate pick up effect
            Instantiate(pickUpEffect, transform.position, transform.rotation);
            if(!gameManager.hasBeenPickedUp && gameObject.CompareTag("BiggerPlayer"))
            { 
                // Play pick up animation
                StopCoroutine("PlayPickUpAnimation");
                StartCoroutine("PlayPickUpAnimation");
                // Set the flag to true once the power-up is picked up
                gameManager.hasBeenPickedUp = true;
                player.spritePurpleRenderer.enabled = true;
                player.spriteBlueRenderer.enabled = false;
                player.transform.localScale *= 1.5f;
                player.anim = player.animPurple;
            } else if (gameObject.CompareTag("LifePower"))
            {
                gameManager.playerHealth++;
            } else if (gameObject.CompareTag("DifPower"))
            {
                Debug.Log("POWER");
                player.DifPower();
            }
            
            Destroy(gameObject);
        }
    }
    
    IEnumerator PlayPickUpAnimation()
    {
        // Play the pick-up animation
        player.PlayAnimation(pickUpAnimation);

        // Wait for the animation to finish
        yield return new WaitForSeconds(pickUpAnimation.length);
    }
}