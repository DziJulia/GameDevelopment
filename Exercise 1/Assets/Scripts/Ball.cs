using UnityEngine;

public class Ball : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Replace "SideTileMap" with the tag of your side tile map
        if (collision.gameObject.CompareTag("SideTileMap") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}