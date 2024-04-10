using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("hit player1");

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        Debug.Log("hit player");
        
        
            
        playerHealth.TakeDamage(1);
        
    }
}
