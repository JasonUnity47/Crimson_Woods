using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage to apply to the player
    public string playerTag = "Player"; // Tag of the player GameObject

    private void OnParticleCollision(GameObject other)
    {
        // Check if the collided GameObject is the player
        if (other.CompareTag(playerTag))
        {
            // Apply damage to the player
            ApplyDamage(other);
        }
    }

    private void ApplyDamage(GameObject player)
    {
        // Check if the player has a health component
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            // Apply damage to the player's health
            health.TakeDamage(1);
        }
    }
}
