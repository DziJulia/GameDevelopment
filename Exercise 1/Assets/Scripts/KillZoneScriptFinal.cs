using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneScriptFinal : MonoBehaviour
{
    public GameManagerFinalProject gameManager;

    public float damageValue;
    private bool canTakeDamage = true;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTakeDamage)
        {
            Debug.Log("Player entered kill zone");
            gameManager.TakeDamage(true);
            canTakeDamage = false;
            Invoke("ResetDamage", 1f);
        }
        if (other.CompareTag("PowerUp") || other.CompareTag("Enemy") )
        {
            Destroy(other.gameObject);
        }
    }
    
    // Method to reset canTakeDamage
    void ResetDamage()
    {
        canTakeDamage = true;
    }
}
