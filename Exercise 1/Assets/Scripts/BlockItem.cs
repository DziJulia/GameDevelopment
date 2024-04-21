using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }


    private IEnumerator Animate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;

        float elapsed = 0f;
        float duration = 0.5f;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;
        
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + Vector3.up;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        rigidbody.isKinematic = false;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}