using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManagerFinalProject gameManager;
    
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.TakeDamage();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("SideTileMap"))
        {
            Destroy(gameObject);
        }
    }
}