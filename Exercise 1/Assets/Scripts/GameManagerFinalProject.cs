using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFinalProject : MonoBehaviour
{
    public float playerHealth;
    public float playScore;
    public Transform spawnPoint;
    public GameObject player;
    public bool isPaused = false;
    public FinalPlayerControllerScript playerObject;
    public Vector3 originalScale;
    public bool hasBeenPickedUp = false;
    
    // Start is called before the first frame update
    private void Awake()
    {
        // making sure that game manager dont destroy itself
        // we keeping the same manager
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // These lines will find the new player, playerObject, and spawnPoint in the new scene
        player = GameObject.FindGameObjectWithTag("Player");
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalPlayerControllerScript>();
        spawnPoint = GameObject.FindGameObjectWithTag("Start").transform;

        // This line will get the original scale of the new player object
        originalScale = player.transform.localScale; 
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null && playerObject == null && playerObject == null)
        {
            hasBeenPickedUp = false;
            spawnPoint = GameObject.FindGameObjectWithTag("Start").transform;
            player = GameObject.FindGameObjectWithTag("Player");
            playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalPlayerControllerScript>();
        }
    }

    public void AddScore(float score)
    {
        playScore += score;
    }

    public void TakeDamage(float damage)
    { 
        playerHealth -= damage;
        Debug.Log("Damage Taken: " + damage + ", Remaining Health: " + playerHealth);
        if (hasBeenPickedUp)
        {
            hasBeenPickedUp = false;
            playerObject.anim.SetFloat("Reverse", -1);
            playerObject.anim.Play("ninjaGrow", 0, 1);
            player.transform.localScale = originalScale;
            playerObject.anim.SetFloat("Reverse", 1);
        }
    }
}
