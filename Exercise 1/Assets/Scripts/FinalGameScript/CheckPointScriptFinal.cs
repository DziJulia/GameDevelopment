using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScriptFinal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>().spawnPoint = transform;
            gameObject.GetComponent<Animator>().SetTrigger("CheckPointTriggered");
        }
    }
}
