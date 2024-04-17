using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFinalProject : MonoBehaviour
{
    public GameManagerFinalProject gameManager;

    public float playerHealth;
    public float playScore;
    // public Transform spawnPoint;
    public GameObject player;
    public bool hasCollided = false;
    public bool isHit;
    
    // Start is called before the first frame update
    private void Awake()
    {
        // making sure that game manager dont destroy itself
        // we keeping the same manager
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
       //  player = GameObject.FindWithTag("Player");
       // spawnPoint = GameObject.FindGameObjectWithTag("Start").transform;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("playScore: " + playScore);
        Debug.Log("playerHealth: " + playerHealth);
        // if (player.transform.position == spawnPoint.position && hasCollided)
       // {
       //     hasCollided = false;
       // }
    }

    public void AddScore(float score)
    {
        playScore += score;
    }

    public void TakeDamage(float damage)
    {
        // Need to check if collided so it just only apply damage only once!
        if (!hasCollided)
        {
            playerHealth -= damage;
            Debug.Log("Damage Taken: " + damage + ", Remaining Health: " + playerHealth);
            hasCollided = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        hasCollided = false;
    }
}
