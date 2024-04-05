using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDied;
    public float health, maxHealth;
    [SerializeField] BuffContent buffContent;
   
    private Animator myAnimator;
    private bool dead;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

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

        }

        if (health > 0)
        {
            health -= amount;
            OnPlayerDamaged?.Invoke();
            myAnimator.SetTrigger("HurtTrigger");

        }
        else
        {
            if (!dead)
            {
                OnPlayerDied?.Invoke();
                myAnimator.SetTrigger("DeadTrigger");
                GetComponent<PlayerController>().enabled = false;               
                dead = true;
            }
        }



    }
}
    