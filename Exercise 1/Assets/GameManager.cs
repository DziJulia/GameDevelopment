using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager gameManager;

    public float playerhealth;
    public float playScore;
    // Start is called before the first frame update

    private void Awake()
    {
        // making sure that game manager dont destroy itself
        // we keeping the same manager
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
       // player = GameObject.FindWithTag("Player");
      //  playerSpawnPoint = GameObject.FindWithTag("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float score)
    {
        playScore += score;
    }

    public void TakeDamage(float damage)
    {
        playerhealth -= damage;
    }

    public void UpdateSpawnPoint(Transform newSpawnPoint)
    {
      //  playerSpawnPoint = newSpawnPoint;
    }
}
