using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFinalProject : MonoBehaviour
{
    public float playerHealth;
    public int playScore;
    public Transform spawnPoint;
    public GameObject player;
    public bool isPaused = false;
    public FinalPlayerControllerScript playerObject;
    public bool hasBeenPickedUp;
    public bool difPower;
    
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
        Application.targetFrameRate = 60;
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

    public void AddScore()
    {
        playScore ++;
        if (playScore == 100)
        {
            AddLife();
            playScore = 0;
        }
    }

    public void AddLife()
    {
        playerHealth++;
    }

    public void TakeDamage(bool killZone = false)
    {
        // Find the moving platform and reset it
        // Find all moving platforms and reset them
        var movingPlatforms = GameObject.FindObjectsOfType<MovingPlatform>();
        foreach (var movingPlatform in movingPlatforms)
        {
            movingPlatform.ResetPlatform();
        }
        
        if (hasBeenPickedUp)
        {
            hasBeenPickedUp = false;
            playerObject.anim = playerObject.animBlue;
            playerObject.anim.SetFloat("Reverse", -1);
            playerObject.anim.Play("ninjaGrow", 0, 1);
            player.transform.localScale /= 1.5f;
            playerObject.anim.SetFloat("Reverse", 1);
            playerObject.spritePurpleRenderer.enabled = false;
            playerObject.spriteBlueRenderer.enabled = true;
            playerObject.anim = playerObject.animBlue;
        }
        else if (killZone){
            playerHealth--;
            playerObject.Death();
        } else
        {
            playerHealth--;
            playerObject.Death();

        }
    }
}
