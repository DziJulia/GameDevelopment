using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFinal : MonoBehaviour
{
    public GameManagerFinalProject gameManager;
    public GameObject pickUpEffect;
    //how much the pick up is worth
    public float scoreValue;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        //checks if the object is picked up by the player
        if (col.gameObject.CompareTag("Player"))
        {
            gameManager.AddScore(scoreValue);
            //initiate pick up effect
            Instantiate(pickUpEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
