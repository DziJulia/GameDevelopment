using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIScriptFinal : MonoBehaviour
{
    public GameManagerFinalProject gameManagerFinal;
    public TMP_Text scoreText;
    public TMP_Text healthText;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManagerFinal = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }


    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameManagerFinal.playScore;
        healthText.text = "Health: " + gameManagerFinal.playerHealth;
    }
}
