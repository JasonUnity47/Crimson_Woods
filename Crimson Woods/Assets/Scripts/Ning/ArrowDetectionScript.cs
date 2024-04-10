using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectionScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Arrow"))
        {
            PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            else
            {
                Debug.Log("PlayerHealth component not found on the arrow parent.");
            }
        }
    }
}