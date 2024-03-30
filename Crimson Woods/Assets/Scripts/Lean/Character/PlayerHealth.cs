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
        if (buffContent.onEvasiveManeuvers)
        {
            int randomNumber = UnityEngine.Random.Range(0, 101);

            Debug.Log(randomNumber);

            if (randomNumber <= buffContent.dodgeChance)
            {
                return;
            }

            else
            {
                health -= amount;
                OnPlayerDamaged?.Invoke();
            }
        }

        else
        {
            health -= amount;
            OnPlayerDamaged?.Invoke();
        }
    }

}
