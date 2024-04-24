using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDamage : MonoBehaviour
{
    // Damage
    [Header("Damage")]
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collides with an object tagged as a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("PlayerHealth component not found on the arrow parent.");
            }
        }
    }

}
