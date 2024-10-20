using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float lightRadius = 5f;
    public LayerMask maskLayer;


    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, lightRadius, maskLayer);

        if(colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                // Assuming your objects have SpriteRenderer component
                SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    Debug.Log("CHANGE");
                    renderer.enabled = true; 
                }

            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightRadius);
    }
}
