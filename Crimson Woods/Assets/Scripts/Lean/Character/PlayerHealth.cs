using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    [SerializeField] BuffContent buffContent;

    public float health, maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // IF Evasive Maneuvers Buff is ON THEN can dodge damage.
        if (buffContent.onEvasiveManeuvers)
        {
            int randomNumber = UnityEngine.Random.Range(0, 101);

            Debug.Log(randomNumber);

            // IF meet 25% chance THEN dodge damage.
            if (randomNumber <= buffContent.dodgeChance)
            {
                // Dodge Effect.
                GameObject evadeEffect = Instantiate(buffContent.evadeVFX, transform.position, transform.rotation, transform);
                Destroy(evadeEffect, 0.5f);
                return;
            }

            // ELSE take damage.
            else
            {
                health -= amount;
                OnPlayerDamaged?.Invoke();
            }
        }

        // ELSE take damage.
        else
        {
            health -= amount;
            OnPlayerDamaged?.Invoke();
        }
    }

}
