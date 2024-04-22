using System;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{
    public Sprite flatSprite;
    public GameManagerFinalProject gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mushroom collision");
            //player falling down landing on head
            if (other.transform.position.y > transform.position.y)
            {
                Flatten();
            }
            else if (gameManager.difPower)
            {
                Hit();
            }
            else {
                Debug.Log("Mushroom collision take damage");
                gameManager.TakeDamage();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell") || other.gameObject.layer == LayerMask.NameToLayer("YellowNinja") )
        {
            Hit();
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        GetComponent<DeadAnimationScript>().enabled = true;
        Destroy(gameObject,3f);
    }
}
