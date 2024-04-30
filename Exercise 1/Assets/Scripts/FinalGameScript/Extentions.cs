using UnityEngine;

public static class Extentions
{
    private static LayerMask _layerMask = LayerMask.GetMask(("Default"));

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        //float radius = 0.25f;
        //float distance = 0.375f;
        float radius = 0.25f * Mathf.Max(rigidbody.transform.localScale.x, rigidbody.transform.localScale.y);
        float distance = 0.375f * Mathf.Max(rigidbody.transform.localScale.x, rigidbody.transform.localScale.y);

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, _layerMask);
        bool b = hit.rigidbody != rigidbody;
        Debug.Log("hit bool" + b);
        Debug.Log("hit hit body" + hit.rigidbody);
        Debug.Log("hit pika" + rigidbody);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
    
    public static bool IsHitFromTop(this Transform transform, Transform other)
    {
        return other.position.y < transform.position.y;
    }

}