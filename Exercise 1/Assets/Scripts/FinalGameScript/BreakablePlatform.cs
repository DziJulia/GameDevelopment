using System.Collections;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public Renderer[] renderers;
    public float breakDuration = 2f;
    public float reappearDuration = 2f;

    private Vector3 originalPosition;
    private bool isBreaking = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isBreaking)
        {
            StartCoroutine(BreakAndReappear());
        }
    }

    IEnumerator BreakAndReappear()
    {
        isBreaking = true;

        // Shake
        float endTime = Time.time + shakeDuration;
        while (Time.time < endTime)
        {
            float x = originalPosition.x + Random.Range(-0.1f, 0.1f);
            float y = originalPosition.y + Random.Range(-0.1f, 0.1f);

            transform.position = new Vector3(x, y, originalPosition.z);

            yield return null;
        }

        // Break
        GetComponent<BoxCollider2D>().enabled = false;
        // Disable all renderers
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        yield return new WaitForSeconds(breakDuration);

        // Reappear
        transform.position = originalPosition;
        GetComponent<BoxCollider2D>().enabled = true;
        // Disable all renderers
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        isBreaking = false;
    }
}