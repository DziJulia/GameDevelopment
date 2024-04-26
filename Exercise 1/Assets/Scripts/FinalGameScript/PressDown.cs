using System.Collections;
using UnityEngine;

public class PressDown : MonoBehaviour
{
    public Animator buttonAnimator;
    public LayerMask playerLayer;
    private bool isColliding;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //player falling down landing on head
        if (other.transform.position.y > transform.position.y)
        {
            isColliding = true;
            buttonAnimator.Play("Down");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
        StartCoroutine(CheckCollision());
    }

    IEnumerator CheckCollision()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // If no collision is detected after 1 second, play the "Default" animation
        if (!isColliding)
        {
            buttonAnimator.Play("Default");
        }
    }
}